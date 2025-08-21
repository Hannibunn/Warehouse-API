using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_API.Models;
using User_API.Data;
using User_API.APIKEYS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BCrypt.Net;
using System.Security.Claims;
using User_API.Services;



namespace User_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicatonDbContext _context;
        private readonly ApiKey? _apiKeyService;
        private readonly IBoxService _boxService;
      

        public LoginController(ApplicatonDbContext context, AuthService authService, ApiKey? apiKeyService = null)
        {
            _context = context;
            _authService = authService;
            _apiKeyService = apiKeyService;
        }


        [HttpPost("Login")]
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

            // JWT erstellen
            var token = _authService.GenerateJwtToken(user.Email);

            // API-Key erstellen (wenn dein ApiKey-Service vorhanden ist)
            string? apiKey = null;
            if (_apiKeyService != null)
            {
                apiKey = _apiKeyService.CreateApiKey();
            }

            return Ok(new
            {
                Token = token,
                ApiKey = apiKey
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("E-Mail und Passwort sind erforderlich!");
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return Conflict("Ein Benutzer mit dieser E-Mail existiert bereits.");
            }
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
        // Alle User abrufen
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost("generate")]
        public IActionResult GenerateApiKey()
        {
            var newKey = _apiKeyService.CreateApiKey();
            return Ok(new { apiKey = newKey });
        }

        // Alle API-Keys abrufen
        [HttpGet("apikeys")]
        public IActionResult GetAllApiKeys()
        {
            var apiKeys = _apiKeyService.GetAllApiKeys();
            return Ok(apiKeys);
        }
        // Admin API-Key abrufen

        [HttpGet("protected-endpoint")]
        public IActionResult GetProtectedData([FromHeader(Name = "X-Api-Key")] string apiKey)
        {

            var adminKey = _apiKeyService.GetAdminKey();

            if (apiKey != adminKey)
            {
                return Unauthorized(new { message = "Ungültiger API-Key" });
            }
            return Ok(new { message = "Erfolgreicher Zugriff auf geschützte Daten" });
        }
        // Authentifizierung Controller mit email und API-Key
        [HttpPost("authenticate")]
        public IActionResult GetApiKeys()
        {
            var apiKey = Request.Headers["ApiKey"].ToString();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API key is required.");
            }
            if (!_apiKeyService.ValidateApiKey(apiKey, out string newKey))
            {
                return Unauthorized("Invalid API key.");
            }
            var email = Request.Headers["Email"].ToString();
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required for authentication.");
            }
            var token = _authService.GenerateJwtToken(email);
            return Ok(new { token, newApiKey = newKey });

        }
 
        // Ueberprüfung API Key Controller
        [HttpPost("validate")]
        public IActionResult ValidateApiKey([FromBody] string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("API key is required.");
            }
            var isValid = _apiKeyService.ValidateApiKey(apiKey, out string newKey);
            if (!isValid)
            {
                return Unauthorized("Invalid API key.");
            }
            if (newKey != null)
            {
                return Ok(new { message = "API key validated successfully.", newApiKey = newKey });
            }
            return Ok("API key validated successfully.");
        }

        //  Alle Boxen eines Users holen
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBoxesByUser(int userId)
        {
            var boxes = await _context.Boxes
                .Where(b => b.UserId == userId)
                .Include(b => b.Sets)
                .ToListAsync();
            return Ok(boxes);
        }

        // Neue Box erstellen
        [HttpPost]
        public async Task<IActionResult> CreateBox([FromBody] Box box)
        {
            if (box == null)
                return BadRequest();

            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoxById), new { id = box.ID }, box);
        }

        //  Box nach Id holen
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoxById(int id)
        {
            var box = await _context.Boxes
                .Include(b => b.Sets)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (box == null) return NotFound();
            return Ok(box);
        }

        //  Box aktualisieren
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBox(int id, [FromBody] Box updatedBox)
        {
            if (id != updatedBox.ID)
                return BadRequest();

            var box = await _context.Boxes.FindAsync(id);
            if (box == null) return NotFound();

            box.Name = updatedBox.Name;
            box.Description = updatedBox.Description;
            box.QRcode = updatedBox.QRcode;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //  Box löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box == null) return NotFound();

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //  Sets einer Box abrufen
        [HttpGet("{boxId}/sets")]
        public async Task<IActionResult> GetSetsInBox(int boxId)
        {
            var box = await _context.Boxes
                .Include(b => b.Sets)
                .FirstOrDefaultAsync(b => b.ID == boxId);

            if (box == null) return NotFound();

            return Ok(box.Sets);
        }

        //   Set zur Box hinzufügen
        [HttpPost("{boxId}/sets")]
        public async Task<IActionResult> AddSetToBox(int boxId, [FromBody] Set set)
        {
            var box = await _context.Boxes
                .Include(b => b.Sets)
                .FirstOrDefaultAsync(b => b.ID == boxId);

            if (box == null) return NotFound();

            set.boxID = boxId;
            set.box = box;
            _context.Sets.Add(set);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSetsInBox), new { boxId = boxId }, set);
        }

          //Set aus Box entfernen
        [HttpDelete("{boxId}/sets/{setId}")]
        public async Task<IActionResult> RemoveSetFromBox(int boxId, int setId)
        {
            var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setId && s.boxID == boxId);
            if (set == null) return NotFound();

            _context.Sets.Remove(set);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //  Box nach Name für einen User abrufen
        [HttpGet("user/{userId}/byname/{boxName}")]
        public async Task<IActionResult> GetBoxByNameForUser(int userId, string boxName)
        {
            var box = await _context.Boxes
                .Include(b => b.Sets)
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Name == boxName);

            if (box == null)
                return NotFound("Box nicht gefunden oder gehört nicht dem User.");

            return Ok(box);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBoxes()
        {
            int? userId = _authService.GetUserIdFromToken(HttpContext);
            if (userId == null)
                return Unauthorized();

            var boxes = await _boxService.GetBoxesByUserIdAsync(userId.Value);
            return Ok(boxes);
        }
    }
}
