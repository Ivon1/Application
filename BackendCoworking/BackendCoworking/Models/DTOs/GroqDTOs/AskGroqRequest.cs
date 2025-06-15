using System.ComponentModel.DataAnnotations;

namespace BackendCoworking.Models.DTOs.GroqDTOs
{
    public class AskGroqRequest
    {
        [Required(ErrorMessage = "The question field is required.")]
        public string Question { get; set; }
    }
}
