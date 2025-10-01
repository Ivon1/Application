using BackendCoworking.Models.DTOs;
using BackendCoworking.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class WorkspacesController(WorkspacesService _workspacesService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkspaceDTO>>> GetAllWorkspaces()
        {
            var workspacesDTOs = await _workspacesService.GetAllWorkspacesAsync();
            if (workspacesDTOs == null || !workspacesDTOs.Any())
            {
                return NotFound("No workspaces found.");
            }
            return Ok(workspacesDTOs);
        }

        [HttpGet("GetWorkspacesByCoworkingId/{id:int}")]
        public async Task<ActionResult<IEnumerable<WorkspaceDTO>>> GetWorkspacesByCoworkingId(int id)
        {
            var workspaces = await _workspacesService.GetWorkspacesByCoworkingIdAsync(id);
            if (workspaces == null || !workspaces.Any())
            {
                return NotFound($"No workspaces found for Coworking ID {id}.");
            }
            return Ok(workspaces);
        }
    }
}
