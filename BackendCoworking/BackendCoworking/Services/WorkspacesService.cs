using AutoMapper;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Repositories;

namespace BackendCoworking.Services
{
    public class WorkspacesService(WorkspacesRepository _workspacesRepository, IMapper _mapper)
    {
        public async Task<List<WorkspaceDTO>> GetAllWorkspacesAsync()
        {
            var workspaces = await _workspacesRepository.GetAllWorkspacesAsync();
            return _mapper.Map<IEnumerable<WorkspaceDTO>>(workspaces).ToList();
        }

        public async Task<List<WorkspaceDTO>> GetWorkspacesByCoworkingIdAsync(int coworkingId)
        {
            var workspaces = await _workspacesRepository.GetWorkspacesByCoworkingIdAsync(coworkingId);
            return _mapper.Map<IEnumerable<WorkspaceDTO>>(workspaces).ToList();
        }
    }
}
