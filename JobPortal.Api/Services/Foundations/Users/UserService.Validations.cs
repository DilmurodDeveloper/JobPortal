namespace JobPortal.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private void ValidateUserId(int id)
        {
            Validate((Rule: IsInvalid(id), Parameter: nameof(User.Id)));
        }

        private void ValidateUserOnAdd(User user)
        {
            ValidateUserNotNull(user);

            Validate(
                (Rule: IsInvalid(user.FirstName!), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalid(user.LastName!), Parameter: nameof(User.LastName)),
                (Rule: IsInvalid(user.Email!), Parameter: nameof(User.Email)),
                (Rule: IsInvalid(user.PasswordHash!), Parameter: nameof(User.PasswordHash)),
                (Rule: IsInvalidRole(user.Role), Parameter: nameof(User.Role)),
                (Rule: IsInvalidStatus(user.Status), Parameter: nameof(User.Status)));
        }

        private void ValidateUserOnModify(User user)
        {
            ValidateUserNotNull(user);

            Validate(
                (Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
                (Rule: IsInvalid(user.FirstName!), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalid(user.LastName!), Parameter: nameof(User.LastName)),
                (Rule: IsInvalid(user.Email!), Parameter: nameof(User.Email)),
                (Rule: IsInvalidRole(user.Role), Parameter: nameof(User.Role)),
                (Rule: IsInvalidStatus(user.Status), Parameter: nameof(User.Status)));
        }

        private static void ValidateAgainstStorageUserOnModify(User inputUser, User storageUser)
        {
            ValidateStorageUser(storageUser, inputUser.Id);

            Validate(
                (Rule: IsNotSame(inputUser.Id, storageUser.Id, nameof(User.Id)),
                 Parameter: nameof(User.Id)));
        }

        private static void ValidateStorageUser(User maybeUser, int userId)
        {
            if (maybeUser is null)
            {
                throw new NotFoundUserException(userId);
            }
        }

        private void ValidateUserNotNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }

        private static dynamic IsInvalid(int id) => new
        {
            Condition = id <= 0,
            Message = "Id must be greater than zero."
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

        private static dynamic IsInvalidRole(Role role) => new
        {
            Condition = !Enum.IsDefined(typeof(Role), role),
            Message = "Role value is invalid."
        };

        private static dynamic IsInvalidStatus(UserStatus status) => new
        {
            Condition = !Enum.IsDefined(typeof(UserStatus), status),
            Message = "Status value is invalid."
        };

        private static dynamic IsNotSame(int firstId, int secondId, string parameterName) => new
        {
            Condition = firstId != secondId,
            Message = $"User {parameterName} does not match."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserException = new InvalidUserException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserException.ThrowIfContainsErrors();
        }
    }
}
