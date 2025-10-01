using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.Repositories
{
    public class WorkspacesRepository(CoworkingContextData _context)
    {
        public async Task<List<Workspaces>> GetAllWorkspacesAsync()
        {
            return await _context.Workspaces
                .Include(w => w.Capacity)
                .Include(w => w.WorkspaceAmenities)
                    .ThenInclude(wa => wa.Amenity)
                .Include(w => w.WorkspacePhotos)
                    .ThenInclude(wp => wp.Photo)
                .Include(w => w.WorkspaceAvailabilitys)
                    .ThenInclude(wa => wa.Availability)
                .ToListAsync();
        }

        public async Task<List<Workspaces>> GetWorkspacesByCoworkingIdAsync(int coworkingId)
        {
            return await _context.Workspaces
                .Where(w => w.CoworkingId == coworkingId)
                .Include(w => w.Capacity)
                .Include(w => w.WorkspaceAmenities)
                    .ThenInclude(wa => wa.Amenity)
                .Include(w => w.WorkspacePhotos)
                    .ThenInclude(wp => wp.Photo)
                .Include(w => w.WorkspaceAvailabilitys)
                    .ThenInclude(wa => wa.Availability)
                .ToListAsync();
        }
    }
}
