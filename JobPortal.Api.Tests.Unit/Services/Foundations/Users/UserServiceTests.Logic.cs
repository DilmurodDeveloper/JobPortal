namespace JobPortal.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldReturnUserSelfViewDtoOnGetMyUserAsync()
        {
            // Arrange
            User randomUser = CreateRandomUser();
            int userId = randomUser.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(randomUser);

            // Act
            var result = await this.userService.GetMyUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(randomUser.FirstName, result.FirstName);
            Assert.Equal(randomUser.LastName, result.LastName);
            Assert.Equal(randomUser.Email, result.Email);
            Assert.Equal(randomUser.AvatarUrl, result.AvatarUrl);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldUpdateUserOnUpdateMyUserAsync()
        {
            // Arrange
            User randomUser = CreateRandomUser();
            int userId = randomUser.Id;

            var updateDto = new UserSelfUpdateDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                AvatarUrl = "https://newavatar.com/avatar.png"
            };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(randomUser);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()))
                    .ReturnsAsync((User u) => u);

            // Act
            await this.userService.UpdateMyUserAsync(userId, updateDto);

            // Assert
            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.Is<User>(user =>
                    user.FirstName == updateDto.FirstName &&
                    user.LastName == updateDto.LastName &&
                    user.AvatarUrl == updateDto.AvatarUrl)),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteUserOnDeleteMyAccountAsync()
        {
            // Arrange
            User randomUser = CreateRandomUser();
            int userId = randomUser.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(randomUser);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()))
                    .ReturnsAsync((User u) => u);

            // Act
            await this.userService.DeleteMyAccountAsync(userId);

            // Assert
            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.Is<User>(user =>
                    user.Status == Enums.UserStatus.Blocked)),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldPatchUserAndReturnTrueOnPatchUserAsync()
        {
            // Arrange
            User randomUser = CreateRandomUser();
            UserSelfPatchDto patchedDto = new UserSelfPatchDto
            {
                FirstName = "PatchedFirstName",
                LastName = "PatchedLastName"
            };

            JsonPatchDocument<UserSelfPatchDto> patchDoc = new JsonPatchDocument<UserSelfPatchDto>();
            patchDoc.Replace(u => u.FirstName, patchedDto.FirstName);
            patchDoc.Replace(u => u.LastName, patchedDto.LastName);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(randomUser.Id))
                    .ReturnsAsync(randomUser);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()))
                    .ReturnsAsync(randomUser);

            // Act
            bool result = await this.userService.PatchUserAsync(randomUser.Id, patchDoc);

            // Assert
            Assert.True(result);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUser.Id),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.Is<User>(user =>
                    user.FirstName == "PatchedFirstName" &&
                    user.LastName == "PatchedLastName")),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFalseOnPatchUserAsyncWhenUserNotFound()
        {
            // Arrange
            int userId = GetRandomNumber();

            var patchDoc = new JsonPatchDocument<UserSelfPatchDto>();
            patchDoc.Replace(dto => dto.FirstName, "PatchedFirstName");

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync((User?)null);

            // Act
            bool result = await this.userService.PatchUserAsync(userId, patchDoc);

            // Assert
            Assert.False(result);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldUploadAvatarAsyncAndReturnUrl()
        {
            // Arrange
            User randomUser = CreateRandomUser();
            int userId = randomUser.Id;
            string expectedFileName = $"avatar_{userId}.png";
            string expectedAvatarUrl = $"/avatars/{expectedFileName}";

            var fileMock = new Mock<IFormFile>();
            var content = new MemoryStream(new byte[] { 1, 2, 3 });
            fileMock.Setup(f => f.FileName).Returns("avatar.png");
            fileMock.Setup(f => f.Length).Returns(content.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(content);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                .Returns<Stream, System.Threading.CancellationToken>((stream, token) =>
                {
                    content.Position = 0;
                    return content.CopyToAsync(stream);
                });

            string rootPath = Path.GetTempPath();

            this.webHostEnvironmentMock.Setup(env => env.WebRootPath)
                .Returns(rootPath);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(randomUser);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()))
                    .ReturnsAsync((User u) => u);

            // Act
            string actualUrl = await this.userService.UploadAvatarAsync(fileMock.Object, userId);

            // Assert
            Assert.Equal(expectedAvatarUrl, actualUrl);

            this.webHostEnvironmentMock.Verify(env => env.WebRootPath, Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectUserByIdAsync(userId), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.UpdateUserAsync(It.Is<User>(user =>
                user.AvatarUrl == expectedAvatarUrl)), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
