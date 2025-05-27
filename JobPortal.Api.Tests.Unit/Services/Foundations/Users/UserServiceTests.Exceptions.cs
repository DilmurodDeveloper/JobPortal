using JobPortal.Api.Models.Foundations.Users.Exceptions;

namespace JobPortal.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowUserValidationExceptionOnGetMyUserAsyncWhenUserIsNotFoundAndLogItAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            User? nullUser = null;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(randomUserId))
                    .ReturnsAsync(nullUser);

            // when
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                this.userService.GetMyUserAsync(randomUserId));

            // then
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is<Exception>(ex => ex.Message.Contains("User validation error occurred"))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUserId),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUserServiceExceptionOnUpdateMyUserAsyncWhenUserIsNotFoundAndLogItAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            var updateDto = new UserSelfUpdateDto
            {
                FirstName = "Test",
                LastName = "User",
                AvatarUrl = "https://avatar.test/avatar.png"
            };

            User? nullUser = null;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(randomUserId))
                    .ReturnsAsync(nullUser);

            // when
            var exception = await Assert.ThrowsAsync<UserServiceException>(() =>
                this.userService.UpdateMyUserAsync(randomUserId, updateDto));

            // then
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is<Exception>(ex => ex.Message.Contains("User service error occurred"))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUserId),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUserServiceExceptionOnDeleteMyAccountAsyncWhenUserIsNotFoundAndLogItAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            User? nullUser = null;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(randomUserId))
                    .ReturnsAsync(nullUser);

            // when
            var exception = await Assert.ThrowsAsync<UserServiceException>(() =>
                this.userService.DeleteMyAccountAsync(randomUserId));

            // then
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is<Exception>(ex => ex.Message.Contains("User service error occurred"))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUserId),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFalseOnPatchUserAsyncWhenUserIsNotFoundAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            var patchDoc = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<UserSelfPatchDto>();

            User? nullUser = null;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(randomUserId))
                    .ReturnsAsync(nullUser);

            // when
            var result = await this.userService.PatchUserAsync(randomUserId, patchDoc);

            // then
            Assert.False(result);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUserId),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUserValidationExceptionOnUploadAvatarAsyncWhenFileTypeIsInvalidAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            var invalidFileMock = new Mock<IFormFile>();
            invalidFileMock.Setup(f => f.FileName).Returns("file.txt");  
            invalidFileMock.Setup(f => f.Length).Returns(100);

            // when
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                this.userService.UploadAvatarAsync(invalidFileMock.Object, randomUserId));

            // then
            Assert.Contains("User validation error occurred", exception.Message);
            Assert.IsType<InvalidUserException>(exception.InnerException);
        }

        [Fact]
        public async Task ShouldThrowUserValidationExceptionOnUploadAvatarAsyncWhenFileIsNullAsync()
        {
            // given
            int randomUserId = GetRandomNumber();
            IFormFile nullFile = null!;

            // when
            var exception = await Assert.ThrowsAsync<UserValidationException>(() =>
                this.userService.UploadAvatarAsync(nullFile!, randomUserId));

            // then
            Assert.Contains("User validation error occurred", exception.Message);
            Assert.IsType<InvalidUserException>(exception.InnerException);
        }
    }
}
