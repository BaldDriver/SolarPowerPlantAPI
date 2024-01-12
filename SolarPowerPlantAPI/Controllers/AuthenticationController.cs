using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarPowerPlantAPI.Authentication;
using SolarPowerPlantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SolarPowerPlantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthenticationController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserResponse userDto)
        {
            // Validation and error handling can be added here

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = userDto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login(UserResponse userDto)
        {
            var existingUser = _context.Users.SingleOrDefault(u => u.Username == userDto.Username && u.PasswordHash == userDto.Password);

            if (existingUser == null)
                return Unauthorized(new APIResponse { Message = "Invalid credentials" });

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("your_secret_key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, existingUser.Username)
                    // Add additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }
    }
}
