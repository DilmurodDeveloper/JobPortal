using JobPortal.Api.Models.Foundations.Users.Exceptions;

namespace JobPortal.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
        {
            // given
            User nullUser = null!;
            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            // when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(nullUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfUserHasInvalidFieldsAndLogItAsync(string invalidText)
        {
            // given
            var invalidUser = new User
            {
                FirstName = invalidText,
                LastName = invalidText,
                Email = invalidText,
                PasswordHash = invalidText,
                Role = (Role)999,
                Status = (UserStatus)999
            };

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(nameof(User.FirstName), "Text is required.");
            invalidUserException.AddData(nameof(User.LastName), "Text is required.");
            invalidUserException.AddData(nameof(User.Email), "Text is required.");
            invalidUserException.AddData(nameof(User.PasswordHash), "Text is required.");
            invalidUserException.AddData(nameof(User.Role), "Role value is invalid.");
            invalidUserException.AddData(nameof(User.Status), "Status value is invalid.");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ShouldThrowValidationExceptionOnGetIfUserIdIsInvalidAndLogItAsync(int invalidId)
        {
            // given
            var invalidUserException = new InvalidUserException();
            invalidUserException.AddData(nameof(User.Id), "Id must be greater than zero.");

            var expectedUserValidationException = new UserValidationException(invalidUserException);

            // when
            Task<UserSelfViewDto?> getUserTask = this.userService.GetMyUserAsync(invalidId);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                getUserTask);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<int>()),
                    Times.Never);
        }
    }
}
