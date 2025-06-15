namespace BackendCoworking.Models.DTOs.GroqDTOs
{
    public class BookingGroqRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CoworkingLocation { get; set; } = string.Empty;
        public string CoworkingName { get; set; } = string.Empty;
        public string CoworkingDescription { get; set; } = string.Empty;
        public string WorkspaceName { get; set; } = string.Empty;
        public string AvailabilityName { get; set; } = string.Empty;   
    }
}