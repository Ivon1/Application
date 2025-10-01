using BackendCoworking.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class CoworkingController(CoworkingService _coworkingService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCoworkings()
        {
            var coworkings = await _coworkingService.GetAllCoworkingsAsync();
            if (coworkings == null || !coworkings.Any())
            {
                return NotFound("No coworkings found.");
            }
            return Ok(coworkings);
        }
    }
}
