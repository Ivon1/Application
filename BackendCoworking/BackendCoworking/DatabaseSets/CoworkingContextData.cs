using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.DatabaseSets
{
    public class CoworkingContextData : DbContext
    {
        public CoworkingContextData(DbContextOptions<CoworkingContextData> options)
            : base(options)
        {
        }

        public DbSet<Coworking> Coworkings { get; set; }
        public DbSet<Workspaces> Workspaces { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<WorkspaceAmenitys> WorkspaceAmenitys { get; set; }
        public DbSet<WorkspacePhotos> WorkspacePhotos { get; set; }
        public DbSet<WorkspaceAvailabilitys> WorkspaceAvailabilitys { get; set; }


    }
}
