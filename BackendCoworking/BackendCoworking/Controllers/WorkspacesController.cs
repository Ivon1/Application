using AutoMapper;
using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class WorkspacesController : ControllerBase
    {
        private readonly CoworkingContextData _context;
        private readonly IMapper _mapper;

        public WorkspacesController(CoworkingContextData context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkspaceDTO>>> GetAllWorkspaces()
        { 
            var workspaces = await _context.Workspaces
                .Include(w => w.Capacity)
                .Include(w => w.WorkspaceAmenities)
                    .ThenInclude(wa => wa.Amenity)
                .Include(w => w.WorkspacePhotos)
                    .ThenInclude(wp => wp.Photo)
                .Include(w => w.WorkspaceAvailabilitys)
                    .ThenInclude(wa => wa.Availability)
                .ToListAsync();

            var workspaceDTOs = _mapper.Map<IEnumerable<WorkspaceDTO>>(workspaces);
            return Ok(workspaceDTOs);
        }
    }
}
