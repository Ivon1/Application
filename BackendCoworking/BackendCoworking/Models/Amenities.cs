using System.ComponentModel.DataAnnotations;

namespace BackendCoworking.Models
{
    public class Amenities
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
