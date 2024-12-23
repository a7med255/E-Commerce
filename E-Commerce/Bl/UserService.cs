using E_Commerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Bl
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, UserProfileDto userProfileDto);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return null;

            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId); // استخدم await بدلاً من Result
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); // استخدم await بدلاً من Result
            if (user == null) return null;

            return new UserProfileDto
            {
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UserProfileDto userProfileDto)
        {
            var user = await _userManager.FindByIdAsync(userId); // استخدم await بدلاً من Result
            if (user == null) return false;

            user.FirstName = userProfileDto.FirstName;
            user.LastName = userProfileDto.LastName;

            var result = await _userManager.UpdateAsync(user); // استخدم await بدلاً من Result
            return result.Succeeded;
        }
    }
}
