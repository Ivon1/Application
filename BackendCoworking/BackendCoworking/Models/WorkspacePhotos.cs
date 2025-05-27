using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class WorkspacePhotos
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int PhotoId { get; set; }


        [ForeignKey("WorkspaceId")]
        public virtual Workspaces Workspace { get; set; } = null!;

        [ForeignKey("PhotoId")]
        public virtual Photos Photo { get; set; } = null!;

    }
}
