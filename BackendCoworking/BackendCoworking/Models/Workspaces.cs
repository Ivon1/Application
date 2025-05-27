using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class Workspaces
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string WorksapceTypeName { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public int CapacityId { get; set; }
        [ForeignKey("CapacityId")]
        public virtual Capacity Capacity { get; set; } = null!;

        // Many-to-many relationship with amenities
        public virtual ICollection<WorkspaceAmenitys> WorkspaceAmenities { get; set; } = new List<WorkspaceAmenitys>();

        // Many-to-many relationship with images
        public virtual ICollection<WorkspacePhotos> WorkspacePhotos { get; set; } = new List<WorkspacePhotos>();

        // Many-to-many relationship with availability
        public virtual ICollection<WorkspaceAvailabilitys> WorkspaceAvailabilitys { get; set; } = new List<WorkspaceAvailabilitys>();
    }
}
