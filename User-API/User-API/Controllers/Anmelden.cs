using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.Models;
using User_API.Services;

namespace User_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Anmelden : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicatonDbContext _context;
        public Anmelden(ApplicatonDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // Login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Users request)
        {
            if (request == null)
                return BadRequest("Anfrage ist leer!");

            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("E-Mail ist erforderlich!");

            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Ungültige Anmeldedaten!");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized("Ungültige Anmeldedaten!");

            var token = _authService.GenerateJwtToken(user.Email);

            return Ok(new { Token = token });
        }

        // Registrierung
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("E-Mail und Passwort sind erforderlich!");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return Conflict("Ein Benutzer mit dieser E-Mail existiert bereits.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Users
            {
                Email = request.Email,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Benutzer erfolgreich registriert!");
        }
    }
}
