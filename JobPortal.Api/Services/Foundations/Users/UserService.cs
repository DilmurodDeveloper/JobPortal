namespace JobPortal.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IWebHostEnvironment webHostEnvironment)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async ValueTask<User> AddUserAsync(User user) =>
            await TryCatch(async () =>
            {
                ValidateUserOnAdd(user);

                User? maybeUser = await storageBroker.SelectUserByIdAsync(user.Id);

                if (maybeUser != null)
                {
                    var innerException = new Exception($"User with id {user.Id} already exists.");
                    throw new AlreadyExistsUserException(innerException);
                }

                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                User addedUser = await storageBroker.InsertUserAsync(user);

                return addedUser;
            });

        public async Task<UserSelfViewDto> GetMyUserAsync(int userId) =>
            await TryCatch(async () =>
            {
                ValidateUserId(userId);

                User? user = await storageBroker.SelectUserByIdAsync(userId);
                ValidateStorageUser(user!, userId);

                return new UserSelfViewDto
                {
                    FirstName = user!.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    AvatarUrl = user.AvatarUrl
                };
            });



        public async Task UpdateMyUserAsync(int userId, UserSelfUpdateDto updateDto) =>
            await TryCatch(async () =>
            {
                ValidateUserId(userId);

                if (updateDto == null)
                    throw new InvalidUserException();

                User? user = await storageBroker.SelectUserByIdAsync(userId);
                ValidateStorageUser(user!, userId);

                user!.FirstName = !string.IsNullOrWhiteSpace(updateDto.FirstName)
                    ? updateDto.FirstName
                    : user.FirstName;

                user.LastName = !string.IsNullOrWhiteSpace(updateDto.LastName)
                    ? updateDto.LastName
                    : user.LastName;

                user.AvatarUrl = !string.IsNullOrWhiteSpace(updateDto.AvatarUrl)
                    ? updateDto.AvatarUrl
                    : user.AvatarUrl;

                user.UpdatedAt = DateTime.UtcNow;

                ValidateUserOnModify(user);

                await storageBroker.UpdateUserAsync(user);
            });

        public async Task DeleteMyAccountAsync(int userId) =>
            await TryCatch(async () =>
            {
                ValidateUserId(userId);

                User? user = await storageBroker.SelectUserByIdAsync(userId);
                ValidateStorageUser(user!, userId);

                user!.Status = UserStatus.Blocked;
                user.UpdatedAt = DateTime.UtcNow;

                await storageBroker.UpdateUserAsync(user);
            });

        public async Task<bool> PatchUserAsync(int userId, JsonPatchDocument<UserSelfPatchDto> patchDoc) =>
            await TryCatch(async () =>
            {
                ValidateUserId(userId);

                if (patchDoc == null)
                    throw new InvalidUserException();

                User? user = await storageBroker.SelectUserByIdAsync(userId);

                if (user == null) 
                {
                    return false;
                }

                ValidateStorageUser(user, userId);

                var dto = new UserSelfPatchDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    AvatarUrl = user.AvatarUrl
                };

                patchDoc.ApplyTo(dto);

                user.FirstName = dto.FirstName ?? user.FirstName;
                user.LastName = dto.LastName ?? user.LastName;
                user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;
                user.UpdatedAt = DateTime.UtcNow;

                ValidateUserOnModify(user);

                await storageBroker.UpdateUserAsync(user);

                return true;
            });


        public async Task<string> UploadAvatarAsync(IFormFile file, int userId) =>
            await TryCatch(async () =>
            {
                ValidateUserId(userId);

                if (file == null || file.Length == 0)
                    throw new InvalidUserException();

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    throw new InvalidUserException();

                var fileName = $"avatar_{userId}{extension}";
                var avatarFolder = Path.Combine(webHostEnvironment.WebRootPath, "avatars");

                if (!Directory.Exists(avatarFolder))
                    Directory.CreateDirectory(avatarFolder);

                var filePath = Path.Combine(avatarFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                User? user = await storageBroker.SelectUserByIdAsync(userId);
                ValidateStorageUser(user!, userId);

                user!.AvatarUrl = $"/avatars/{fileName}";
                user.UpdatedAt = DateTime.UtcNow;

                await storageBroker.UpdateUserAsync(user);

                return user.AvatarUrl;
            });
    }
}
