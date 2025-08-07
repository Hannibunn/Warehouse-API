using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User_API.Services;

namespace Usertesting
{
    internal class BoxController : ControllerBase
    {
        private readonly IBoxService _boxService;
        private readonly IAuthService _authService;

        public BoxController(IBoxService boxService, IAuthService authService)
        {
            _boxService = boxService;
            _authService = authService;
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
