using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdviceAssignement.DAL.Data;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdviceAssignement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserData _data;

        public UserController(UserData data)
        {
            _data = data;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var res = await _data.GetAllUsers();
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllUsers error: {ex.Message}");
                return StatusCode(500, "Failed to fetch users.");
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var res = await _data.GetUserByEmail(email);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetUserByEmail error: {ex.Message}");
                return StatusCode(500, "Failed to fetch user.");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            try
            {
                var res = await _data.Login(userDto.Email, userDto.Password);
                if (res == null) return Unauthorized();

                var token = GenerateJwtToken(res);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Login error: {ex.Message}");
                return StatusCode(500, "Login failed.");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            try
            {
                User newUser = new User()
                {
                    Email = userDto.Email,
                    Password = userDto.Password
                };
                var res = await _data.CreateUser(newUser);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateUser error: {ex.Message}");
                return StatusCode(500, "Failed to create user.");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("X9F2!vL4r7T@qL9yZ6ePbA8#sDfWv1E3nCkT0rJx")); //unsecure
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AdviceAuthService",
                audience: "AdviceClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
