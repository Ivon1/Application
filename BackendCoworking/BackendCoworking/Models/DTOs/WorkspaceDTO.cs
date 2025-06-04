namespace BackendCoworking.Models.DTOs
{
    public class WorkspaceDTO
    {
        public int Id { get; set; }
        public string WorksapceTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Capacity { get; set; } = string.Empty;
        public List<AmenityDTO> Amenities { get; set; } = new List<AmenityDTO>();
        public List<string> PhotoUrls { get; set; } = new List<string>();
        public List<AvailabilityDTO> Availabilities { get; set; } = new List<AvailabilityDTO>();
    }
}
