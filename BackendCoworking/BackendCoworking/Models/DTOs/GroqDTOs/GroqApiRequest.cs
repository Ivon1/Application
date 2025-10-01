namespace BackendCoworking.Models.DTOs.GroqDTOs
{
    public class GroqApiRequest
    {
        public string model { get; set; } = "llama-3.1-8b-instant";
        public List<Message> messages { get; set; }
        public double temperature { get; set; } = 0.7;
    }
}
