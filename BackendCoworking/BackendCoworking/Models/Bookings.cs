using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class Bookings
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "")]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress(ErrorMessage = "Wrong email address !")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int WorkspaceId { get; set; }

        [Required]
        public int AvailabilityId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        [ForeignKey("WorkspaceId")]
        public virtual Workspaces? Workspace { get; set; }

        [ForeignKey("AvailabilityId")]
        public virtual Availability? Availability { get; set; }

    }
}
