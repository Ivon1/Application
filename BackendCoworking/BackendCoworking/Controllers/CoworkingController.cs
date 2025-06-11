using AutoMapper;
using BackendCoworking.DatabaseSets;
using BackendCoworking.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class CoworkingController : ControllerBase
    {
        // Dependency injection for the database context and mapper
        private readonly CoworkingContextData _context;
        private readonly IMapper _mapper;

        public CoworkingController(CoworkingContextData context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoworkings()
        {
            var coworkings = await _context.Coworkings
                .Include(c => c.Photo)
                .ToListAsync();
            var coworkingDTOs = _mapper.Map<List<CoworkingDTO>>(coworkings);
            return Ok(coworkingDTOs);
        }
    }
}
