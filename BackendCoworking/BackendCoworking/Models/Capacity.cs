using System.ComponentModel.DataAnnotations;

namespace BackendCoworking.Models
{
    public class Capacity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, ErrorMessage = "Capacity type name cannot be longer than 100 characters.")]
        public string CapacityTypeName { get; set; } = string.Empty;
    }
}
