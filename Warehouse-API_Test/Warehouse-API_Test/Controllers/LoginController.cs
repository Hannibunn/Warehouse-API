using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_API_Test.Migration;
namespace Warehouse_API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Createuser([FromBody] Users users)
        {
            return CreatedAtAction(nameof(Createuser), new {id =users.Id},users);  
        }

    }
}
