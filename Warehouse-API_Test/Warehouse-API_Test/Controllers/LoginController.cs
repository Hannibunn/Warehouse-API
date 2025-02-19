using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_API_Test.Models;
using Warehouse_API_Test.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Warehouse_API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicatonDbContext _context;
      

        public LoginController(ApplicatonDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Users request)
        {
            if (request == null)
            {
                return BadRequest("Anfrage ist leer!");
            }

            if (string.IsNullOrEmpty(request.email))
            {
                return BadRequest("E-Mail ist erforderlich!");
            }

            var user = _context.Users.FirstOrDefault(u => u.email == request.email);
            if (user == null)
            {
                return Unauthorized("Ungültige Anmeldedaten!");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.HashedPassword, user.HashedPassword))
            {
                return Unauthorized("Ungültige Anmeldedaten!");
            }

            if (_authService == null)
            {
                return StatusCode(500, "AuthService nicht initialisiert!");
            }

            var token = _authService.GenerateJwtToken(user.email);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users request)
        {
            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.HashedPassword))
            {
                return BadRequest("E-Mail und Passwort sind erforderlich!");
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (existingUser != null)
            {
                return Conflict("Ein Benutzer mit dieser E-Mail existiert bereits.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.HashedPassword);
            var newUser = new Users
            {
                email = request.email,
                HashedPassword = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Benutzer erfolgreich registriert!");
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }




    }
}
