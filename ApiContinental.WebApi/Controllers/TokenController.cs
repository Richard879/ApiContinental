using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiContinental.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        public TokenController(IConfiguration config) => _config = config;

        [HttpPost]
        public IActionResult GetToken([FromBody] CredentialDto cred)
        {
            if (cred.Username != "test" || cred.Password != "test") return Unauthorized();

            var jwtSecret = _config["JwtSecret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "JwtSecret no está configurado.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, cred.Username) },
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        public class CredentialDto
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
