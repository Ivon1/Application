using System.ComponentModel.DataAnnotations;

namespace BackendCoworking.Models
{
    public class Photos
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
