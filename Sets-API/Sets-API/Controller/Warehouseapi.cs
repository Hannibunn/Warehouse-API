using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sets_API.APIKEYS;
using Sets_API.Data;
using Sets_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Sets_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Warehouseapi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApiKey _apiKeyService;
        private readonly AuthService _authService;
        public Warehouseapi(ApplicationDbContext context, AuthService authService, ApiKey apiKeyService)
        {
            _context = context;
            _authService = authService;
            _apiKeyService = apiKeyService;
        }

        //  Sets Controller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Set>>> GetSets()
        {
            var sets = await _context.Sets/*.Take(1000)*/.
                ToListAsync();
            return Ok(sets);
        }

        // Abrufen von Sets anhand der ID
        [HttpGet("sets/{id}")]
        public async Task<ActionResult<Set>> GetSetById(int id)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound();
            }
            return Ok(set);
        }

        //  Abrufen von Sets anhand des Barcodes
        [HttpGet("sets/barcode/{barcode}")]
        public async Task<ActionResult<Set>> GetSetByBarcode(string barcode)
        {
            var set = await _context.Sets
                .Include(s => s.Barcode)
                .FirstOrDefaultAsync(s => s.Barcode != null && s.Barcode.EAN == barcode);

            if (set == null)
            {
                set = await _context.Sets
                    .Include(s => s.Barcode)
                    .FirstOrDefaultAsync(s => s.Barcode != null && s.Barcode.UPC == barcode);
            }

            if (set == null)
            {
                return NotFound(new { message = "Kein Set mit diesem Barcode gefunden." });
            }

            return Ok(set);
        }

        // Neuer Sets Controller
        [HttpPost("sets")]
        public async Task<ActionResult<Set>> CreateSet([FromBody] Set set)
        {
            if (set == null)
            {
                return BadRequest("Set-Daten sind erforderlich.");
            }
            _context.Sets.Add(set);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSetById), new { id = set.Id }, set);
        }

        // Aktualisieren eines Sets
        [HttpPut("sets/{id}")]
        public async Task<IActionResult> UpdateSet(int id, [FromBody] Set set)
        {
            if (set == null || set.Id != id)
            {
                return BadRequest("Ungültige Set-Daten.");
            }

            var existingSet = await _context.Sets
                .Include(s => s.Barcode)
                .Include(s => s.LEGOCom)
                .Include(s => s.Image)
                .Include(s => s.AgeRange)
                .Include(s => s.Dimensions)
                .Include(s => s.ExtendedData)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSet == null)
            {
                return NotFound();
            }

            // Primitive Werte
            existingSet.setID = set.setID;
            existingSet.boxID = set.boxID;
            existingSet.number = set.number;
            existingSet.numberVariant = set.numberVariant;
            existingSet.name = set.name;
            existingSet.year = set.year;
            existingSet.theme = set.theme;
            existingSet.themeGroup = set.themeGroup;
            existingSet.subTheme = set.subTheme;
            existingSet.category = set.category;
            existingSet.released = set.released;
            existingSet.pieces = set.pieces;
            existingSet.minifigs = set.minifigs;
            existingSet.packagingType = set.packagingType;
            existingSet.availability = set.availability;
            existingSet.instructionsCount = set.instructionsCount;

            // Objekt: Barcode
            existingSet.Barcode = set.Barcode;

            // Objekt: LEGOCom
            existingSet.LEGOCom = set.LEGOCom;

            // Objekt: Image
            existingSet.Image = set.Image;

            // Objekt: AgeRange
            existingSet.AgeRange = set.AgeRange;

            // Objekt: Dimensions
            existingSet.Dimensions = set.Dimensions;

            // Objekt: ExtendedData
            existingSet.ExtendedData = set.ExtendedData;

            // Beziehung zu Box (optional, falls relevant)
            existingSet.box = set.box;

            _context.Sets.Update(existingSet);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Löschen eines Sets
        [HttpDelete("sets/{id}")]
        public async Task<IActionResult> DeleteSet(int id)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound();
            }
            _context.Sets.Remove(set);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Box Controller
        [HttpGet("boxes")]
        public async Task<ActionResult<IEnumerable<Box>>> GetBoxes()
        {
            var boxes = await _context.Boxes.Include(b => b.Sets).ToListAsync();
            return Ok(boxes);
        }
        // Abrufen von Boxen anhand der ID
        [HttpGet("boxes/{id}")]
        public async Task<ActionResult<Box>> GetBoxById(int id)
        {
            var box = await _context.Boxes.Include(b => b.Sets).FirstOrDefaultAsync(b => b.ID == id);
            if (box == null)
            {
                return NotFound();
            }
            return Ok(box);
        }
        // Abrufen von Boxen anhand des QR-Codes
        [HttpPost("boxes")]
        public async Task<ActionResult<Box>> CreateBox([FromBody] Box box)
        {
            if (box == null)
            {
                return BadRequest("Box-Daten sind erforderlich.");
            }
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoxById), new { id = box.ID }, box);
        }
        // Aktualisieren einer Box
        [HttpPut("boxes/{id}")]
        public
            async Task<IActionResult> UpdateBox(int id, [FromBody] Box box)
        {
            if (box == null || box.ID != id)
            {
                return BadRequest("Ungültige Box-Daten.");
            }
            var existingBox = await _context.Boxes.FindAsync(id);
            if (existingBox == null)
            {
                return NotFound();
            }
            existingBox.Name = box.Name;
            existingBox.Description = box.Description;
            existingBox.QRcode = box.QRcode;
            _context.Boxes.Update(existingBox);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Löschen einer Box
        [HttpDelete("boxes/{id}")]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box == null)
            {
                return NotFound();
            }
            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        // Generate API Key Controller
        [HttpPost("generate")]
        public IActionResult GenerateApiKey()
        {
            var newKey = _apiKeyService.CreateApiKey();
            return Ok(new { apiKey = newKey });
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

        // Get All API Keys Controller
        [HttpGet("apikeys")]
        public IActionResult GetAllApiKeys()
        {
            var apiKeys = _apiKeyService.GetAllApiKeys();
            return Ok(apiKeys);
        }
    

  

    }
}
