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

        public async Task<User> GetMyUserAsync(int userId)
        {
            var user = await this.storageBroker.SelectUserByIdAsync(userId);

            if (user == null)
            {
                var exception = new KeyNotFoundException("User not found.");
                this.loggingBroker.LogError(exception);
                throw exception;
            }

            return user;
        }

        public async Task UpdateMyUserAsync(int userId, User updatedUser)
        {
            var user = await this.storageBroker.SelectUserByIdAsync(userId);

            if (user == null)
            {
                var exception = new KeyNotFoundException("User not found.");
                this.loggingBroker.LogError(exception);
                throw exception;
            }

            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Email = updatedUser.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;

            await this.storageBroker.UpdateUserAsync(user);
        }

        public async Task DeleteMyAccountAsync(int userId)
        {
            var user = await this.storageBroker.SelectUserByIdAsync(userId);

            if (user == null)
            {
                var exception = new KeyNotFoundException("User not found.");
                this.loggingBroker.LogError(exception);
                throw exception;
            }

            user.Status = UserStatus.Blocked;
            user.UpdatedAt = DateTime.UtcNow;

            await this.storageBroker.UpdateUserAsync(user);
        }

        public async Task<bool> PatchUserAsync(int userId, JsonPatchDocument<User> patchDoc)
        {
            var user = await this.storageBroker.SelectUserByIdAsync(userId);

            if (user == null)
                return false;

            patchDoc.ApplyTo(user);
            user.UpdatedAt = DateTime.UtcNow;

            await this.storageBroker.UpdateUserAsync(user);

            return true;
        }

        public async Task<string> UploadAvatarAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Unsupported file type.");

            var fileName = $"avatar_{userId}{extension}";
            var avatarFolder = Path.Combine(this.webHostEnvironment.WebRootPath, "avatars");

            if (!Directory.Exists(avatarFolder))
                Directory.CreateDirectory(avatarFolder);

            var filePath = Path.Combine(avatarFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await this.storageBroker.SelectUserByIdAsync(userId);

            if (user != null)
            {
                user.AvatarUrl = $"/avatars/{fileName}";
                user.UpdatedAt = DateTime.UtcNow;
                await this.storageBroker.UpdateUserAsync(user);
            }

            return $"/avatars/{fileName}";
        }
    }
}
