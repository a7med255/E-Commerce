using E_Commerce.Bl; // تأكد من أن هذا المكان يحتوي على كلاس ApplicationUser
using E_Commerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Endpoint لتسجيل المستخدمين الجدد
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName, // إذا كنت تستخدم FirstName
                LastName = registerDto.LastName, // إذا كنت تستخدم LastName
            };

            // إنشاء المستخدم باستخدام UserManager
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors); // في حالة حدوث أخطاء في التسجيل

            return Ok("User registered successfully.");
        }

        // Endpoint لتسجيل الدخول
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            // التحقق من كلمة المرور
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid email or password.");

            // توليد JWT Token بعد التحقق من المستخدم
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        // دالة لتوليد JWT Token
        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", $"{user.FirstName} {user.LastName}") // إضافة الاسم الكامل في الـ claims
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
