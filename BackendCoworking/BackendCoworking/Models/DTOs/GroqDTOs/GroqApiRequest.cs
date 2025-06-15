namespace BackendCoworking.Models.DTOs.GroqDTOs
{
    public class GroqApiRequest
    {
        public string model { get; set; } = "llama3-8b-8192";
        public List<Message> messages { get; set; }
        public double temperature { get; set; } = 0.7;
    }
}
