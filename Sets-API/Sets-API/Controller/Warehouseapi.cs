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
                .FirstOrDefaultAsync(s => s.BarcodeEan == barcode);
            if (set == null)
            {
                set = await _context.Sets
                    .FirstOrDefaultAsync(s => s.BarcodeUpc == barcode);
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

            var existingSet = await _context.Sets.FindAsync(id);
            if (existingSet == null)
            {
                return NotFound();
            }
            // Alle Eigenschaften manuell aktualisieren
            existingSet.BoxId = set.BoxId;
            existingSet.Number = set.Number;
            existingSet.NumberVariant = set.NumberVariant;
            existingSet.Name = set.Name;
            existingSet.Year = set.Year;
            existingSet.Theme = set.Theme;
            existingSet.ThemeGroup = set.ThemeGroup;
            existingSet.Subtheme = set.Subtheme;
            existingSet.Category = set.Category;
            existingSet.Released = set.Released;
            existingSet.Pieces = set.Pieces;
            existingSet.Minifigs = set.Minifigs;
            existingSet.ImageId = set.ImageId;
            existingSet.ImageThumbnailUrl = set.ImageThumbnailUrl;
            existingSet.ImageImageUrl = set.ImageImageUrl;
            existingSet.LegocomId = set.LegocomId;
            existingSet.LegocomUsRetailPrice = set.LegocomUsRetailPrice;
            existingSet.LegocomUsDateFirstAvailable = set.LegocomUsDateFirstAvailable;
            existingSet.LegocomUsDateLastAvailable = set.LegocomUsDateLastAvailable;
            existingSet.LegocomUkRetailPrice = set.LegocomUkRetailPrice;
            existingSet.LegocomUkDateFirstAvailable = set.LegocomUkDateFirstAvailable;
            existingSet.LegocomUkDateLastAvailable = set.LegocomUkDateLastAvailable;
            existingSet.LegocomCaRetailPrice = set.LegocomCaRetailPrice;
            existingSet.LegocomCaDateFirstAvailable = set.LegocomCaDateFirstAvailable;
            existingSet.LegocomCaDateLastAvailable = set.LegocomCaDateLastAvailable;
            existingSet.LegocomDeRetailPrice = set.LegocomDeRetailPrice;
            existingSet.LegocomDeDateFirstAvailable = set.LegocomDeDateFirstAvailable;
            existingSet.LegocomDeDateLastAvailable = set.LegocomDeDateLastAvailable;
            existingSet.PackagingType = set.PackagingType;
            existingSet.Availability = set.Availability;
            existingSet.InstructionsCount = set.InstructionsCount;
            existingSet.AgeRangeId = set.AgeRangeId;
            existingSet.AgeRangeMin = set.AgeRangeMin;
            existingSet.AgeRangeMax = set.AgeRangeMax;
            existingSet.DimensionsHeight = set.DimensionsHeight;
            existingSet.DimensionsWidth = set.DimensionsWidth;
            existingSet.DimensionsDepth = set.DimensionsDepth;
            existingSet.DimensionsWeight = set.DimensionsWeight;
            existingSet.BarcodeEan = set.BarcodeEan;
            existingSet.BarcodeUpc = set.BarcodeUpc;
            existingSet.ExtendedDataBrickTags = set.ExtendedDataBrickTags;
            // Beziehung zu Box (optional, je nach Datenmodell)
            existingSet.Box = set.Box;
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
            var box = await _context.Boxes.Include(b => b.Sets).FirstOrDefaultAsync(b => b.Id == id);
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
            return CreatedAtAction(nameof(GetBoxById), new { id = box.Id }, box);
        }
        // Aktualisieren einer Box
        [HttpPut("boxes/{id}")]
        public
            async Task<IActionResult> UpdateBox(int id, [FromBody] Box box)
        {
            if (box == null || box.Id != id)
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
            existingBox.Qrcode = box.Qrcode;
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
