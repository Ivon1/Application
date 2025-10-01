namespace BackendCoworking.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public WorkspaceDTO? Workspace { get; set; }
        public AvailabilityDTO? Availability { get; set; }

        public override string ToString()
        {
            return $"BookingDTO [Id={Id}, Name={Name}, Email={Email}, StartDate={StartDate}, EndDate={EndDate}, Workspace={Workspace.Id}, Availability={Availability.Id}]";
        }
    }
}
