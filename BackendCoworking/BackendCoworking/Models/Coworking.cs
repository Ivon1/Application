using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCoworking.Models
{
    public class Coworking
    {
        [Key]
        public int Id { get; set; }

        [Required, Length(3, 100, ErrorMessage = "Name must contain 3-100 symbols")]
        public string Name { get; set; } = string.Empty;

        [Required, Length(10, 500, ErrorMessage = "Description must contain 10-500 symbols")]
        public string Description { get; set; } = string.Empty;

        [Required, Length(5, 500, ErrorMessage = "Address must contain 5-500 symbols")]
        public string Location { get; set; } = string.Empty;

        [Required]
        public int PhotoId { get; set; }

        [ForeignKey("PhotoId")]
        public Photos? Photo { get; set; }

        public ICollection<Workspaces> Workspaces { get; set; } = new List<Workspaces>();
    }
}