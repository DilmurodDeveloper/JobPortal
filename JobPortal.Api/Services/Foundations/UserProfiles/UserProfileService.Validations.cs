namespace JobPortal.Api.Services.Foundations.UserProfiles
{
    public partial class UserProfileService
    {
        private void ValidateUserProfileNotNull(UserProfile userProfile)
        {
            if (userProfile is null)
            {
                throw new NullUserProfileException();
            }
        }

        private void ValidateUserProfileOnAdd(UserProfile userProfile)
        {
            ValidateUserProfileNotNull(userProfile);

            Validate(
                (Rule: IsInvalid(userProfile.UserId), Parameter: nameof(UserProfile.UserId)),
                (Rule: IsInvalid(userProfile.FirstName), Parameter: nameof(UserProfile.FirstName)),
                (Rule: IsInvalid(userProfile.LastName), Parameter: nameof(UserProfile.LastName)),
                (Rule: IsInvalidPhoneNumber(userProfile.PhoneNumber), Parameter: nameof(UserProfile.PhoneNumber))
            );
        }

        private void ValidateUserProfileOnModify(UserProfile userProfile)
        {
            ValidateUserProfileNotNull(userProfile);

            Validate(
                (Rule: IsInvalid(userProfile.Id), Parameter: nameof(UserProfile.Id)),
                (Rule: IsInvalid(userProfile.UserId), Parameter: nameof(UserProfile.UserId)),
                (Rule: IsInvalid(userProfile.FirstName), Parameter: nameof(UserProfile.FirstName)),
                (Rule: IsInvalid(userProfile.LastName), Parameter: nameof(UserProfile.LastName)),
                (Rule: IsInvalidPhoneNumber(userProfile.PhoneNumber), Parameter: nameof(UserProfile.PhoneNumber))
            );
        }

        private static dynamic IsInvalid(int number) => new
        {
            Condition = number <= 0,
            Message = "Value must be greater than zero"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidPhoneNumber(string? phoneNumber) => new
        {
            Condition = string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 7,
            Message = "Phone number is required and should be at least 7 characters long"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserProfileException = new InvalidUserProfileException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserProfileException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserProfileException.ThrowIfContainsErrors();
        }
    }
}
