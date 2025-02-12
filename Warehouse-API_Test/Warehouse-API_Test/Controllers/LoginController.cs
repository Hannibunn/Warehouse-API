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
        public LoginController (ApplicatonDbContext context)
        {
            _context = context;
        }


        [HttpPost("Login")]

        public IActionResult Login([FromBody] Users request)
        {
            _context.Users.Add(request);
            _context.SaveChanges();

            var user = _context.Users.FirstOrDefault(u => u.email == request.email);

            if (user == null)
            {
                return Unauthorized("Ungültige Anmeldedaten!");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.HashedPassword, user.HashedPassword))
            {
                return Unauthorized("Ungültige Anmeldedaten!");
            }



            var token = _authService.GenerateJwtToken(user.email);

            return CreatedAtAction(nameof(Login), new {id =request.Id},request);  
        }

    }
}
