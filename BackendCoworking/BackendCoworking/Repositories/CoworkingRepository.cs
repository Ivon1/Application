using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.Repositories
{
    public class CoworkingRepository(CoworkingContextData _context)
    {
        public async Task<List<Coworking>> GetAllCoworkingsAsync()
        {
            return await _context.Coworkings
                .Include(c => c.Photo)
                .Include(c => c.Workspaces)
                    .ThenInclude(w => w.Capacity)
                .Include(c => c.Workspaces)
                    .ThenInclude(c => c.WorkspaceAvailabilitys)
                        .ThenInclude(wa => wa.Availability)
                .ToListAsync();
        }
    }
}
