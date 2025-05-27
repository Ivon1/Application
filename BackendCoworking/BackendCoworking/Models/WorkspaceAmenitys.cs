using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class WorkspaceAmenitys
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int AmenityId { get; set; }


        [ForeignKey("WorkspaceId")]
        public Workspaces Workspace { get; set; } = null!;

        [ForeignKey("AmenityId")]
        public Amenities Amenity { get; set; } = null!;
    }
}
