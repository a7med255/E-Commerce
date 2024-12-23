using E_Commerce.Bl;
using E_Commerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Profile
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var user = await _userService.GetCurrentUserAsync(); // استخدم Async هنا
            if (user == null)
            {
                return Unauthorized();
            }

            var userProfile = await _userService.GetUserProfileAsync(user.Id); // استخدم Async هنا
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        // PUT: api/Profile
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto userProfileDto)
        {
            if (userProfileDto == null)
            {
                return BadRequest("Invalid profile data.");
            }

            var user = await _userService.GetCurrentUserAsync(); // استخدم Async هنا
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdateUserProfileAsync(user.Id, userProfileDto); // استخدم Async هنا
            if (!result)
            {
                return BadRequest("Failed to update profile");
            }

            return NoContent();
        }
    }
}
