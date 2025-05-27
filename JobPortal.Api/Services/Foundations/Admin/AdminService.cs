namespace JobPortal.Api.Services.Foundations.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BlockUserAsync(int id, bool block)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.Status = block ? UserStatus.Blocked : UserStatus.Active;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<UserSummaryDto>> GetUsersAsync(
            int page, int pageSize, Role? role, UserStatus? status, string? search)
        {
            var query = _context.Users.AsQueryable();

            if (role.HasValue)
                query = query.Where(u => u.Role == role.Value);

            if (status.HasValue)
                query = query.Where(u => u.Status == status.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(u => u.FirstName!.Contains(search)
                                      || u.LastName!.Contains(search)
                                      || u.Email!.Contains(search));

            var totalUsers = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserSummaryDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    Status = u.Status.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<UserSummaryDto>
            {
                Total = totalUsers,
                Page = page,
                PageSize = pageSize,
                Items = users
            };
        }

        public async Task UpdateUserAsync(int id, AdminUpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;

            if (dto.Role.HasValue)
                user.Role = dto.Role.Value;

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.Status = UserStatus.Blocked;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var blockedUsers = await _context.Users.CountAsync(u => u.Status == UserStatus.Blocked);
            var admins = await _context.Users.CountAsync(u => u.Role == Role.Admin);
            var employers = await _context.Users.CountAsync(u => u.Role == Role.Employer);
            var regularUsers = await _context.Users.CountAsync(u => u.Role == Role.User);

            return new StatisticsDto
            {
                TotalUsers = totalUsers,
                BlockedUsers = blockedUsers,
                AdminCount = admins,
                EmployerCount = employers,
                UserCount = regularUsers
            };
        }

        public async Task UpdateUserRoleAsync(int id, UpdateUserRoleDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!Enum.IsDefined(typeof(Role), dto.Role))
                throw new ArgumentException("Invalid role value");

            user.Role = dto.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new UserDetailsDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,

                Profile = user.Profile == null ? null : new UserProfileDto
                {
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    PhoneNumber = user.Profile.PhoneNumber,
                    Address = user.Profile.Address,
                    Bio = user.Profile.Bio,
                    ProfilePictureUrl = user.Profile.ProfilePictureUrl,
                    Location = user.Profile.Location
                }
            };
        }

        public async Task<PagedResult<ApplicationDto>> GetApplicationsAsync(int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.JobPost)
                .Include(a => a.User);

            var totalApplications = await query.CountAsync();

            var applications = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobPostId,
                    SeekerId = a.UserId,
                    Status = a.Status.ToString(),
                    CreatedAt = a.AppliedAt,
                    JobTitle = a.JobPost.Title,
                    SeekerName = a.User.FirstName + " " + a.User.LastName,
                    SeekerPhoneNumber = a.User.Profile != null ? a.User.Profile.PhoneNumber : null
                })
                .ToListAsync();

            return new PagedResult<ApplicationDto>
            {
                Total = totalApplications,
                Page = page,
                PageSize = pageSize,
                Items = applications
            };
        }

    }
}
