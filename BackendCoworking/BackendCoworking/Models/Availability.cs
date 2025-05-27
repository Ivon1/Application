using System.ComponentModel.DataAnnotations;

namespace BackendCoworking.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Availability name could not be more than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Url]
        public string PhotoUrl { get; set; } = string.Empty;

        public ICollection<WorkspaceAvailabilitys> WorkspaceAvailabilitys { get; set; } = new List<WorkspaceAvailabilitys>();
    }
}
