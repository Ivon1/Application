namespace BackendCoworking.Models.DTOs
{
    public class CoworkingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string AvailabilitySummary { get; set; } = string.Empty;
    }
}
