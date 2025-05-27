using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class WorkspaceAvailabilitys
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int AvailabilityId { get; set; }


        [ForeignKey("WorkspaceId")]
        public virtual Workspaces Workspace { get; set; } = null!;

        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; } = null!;
    }
}
