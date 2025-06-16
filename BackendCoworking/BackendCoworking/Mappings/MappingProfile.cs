using AutoMapper;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Models.DTOs.GroqDTOs;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace BackendCoworking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from Availability to AvailabilityDTO
            CreateMap<Availability, AvailabilityDTO>();

            // Map from Amenities to AmenityDTO
            CreateMap<Amenities, AmenityDTO>();

            // Map from Coworking to CoworkingDTO
            CreateMap<Coworking, CoworkingDTO>();

            // Map from Booking to BookingGroqRequest
            CreateMap<Bookings, BookingGroqRequest>();

            // Map from Workspaces to WorkspaceDTO
            CreateMap<Workspaces, WorkspaceDTO>()
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity.CapacityTypeName))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.WorkspaceAmenities.Select(wa => wa.Amenity)))
                .ForMember(dest => dest.PhotoUrls, opt => opt.MapFrom(src => src.WorkspacePhotos.Select(wp => wp.Photo.ImageUrl)))
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.WorkspaceAvailabilitys.Select(wa => wa.Availability)))
                .ForMember(dest => dest.CoworkingId, opt => opt.MapFrom(src => src.CoworkingId));

            // Booking mapping
            CreateMap<Bookings, BookingDTO>()
                .ForMember(dest => dest.Workspace, opt => opt.MapFrom(src => src.Workspace))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availability));

            // Map from Coworking to CoworkingDTO
            CreateMap<Coworking, CoworkingDTO>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.Location, src => src.MapFrom(x => x.Location))
                .ForMember(dest => dest.PhotoUrl, src => src.MapFrom(x => x.Photo.ImageUrl))
                .ForMember(dest => dest.AvailabilitySummary, src => src.MapFrom(x => GetAvailabilitySummary(x)));

            // Map from Booking to BookingGroqRequest
            CreateMap<Bookings, BookingGroqRequest>()
                .ForMember(dest => dest.CoworkingName, src => src.MapFrom(x => x.Workspace.Coworking.Name))
                .ForMember(dest => dest.WorkspaceName, src => src.MapFrom(x => x.Workspace.WorksapceTypeName))
                .ForMember(dest => dest.AvailabilityName, src => src.MapFrom(x => x.Availability.Name))
                .ForMember(dest => dest.StartDate, src => src.MapFrom(x => x.StartDate))
                .ForMember(dest => dest.EndDate, src => src.MapFrom(x => x.EndDate))
                .ForMember(dest => dest.CoworkingDescription, src => src.MapFrom(x => x.Workspace.Coworking.Description))
                .ForMember(dest => dest.CoworkingLocation, src => src.MapFrom(x => x.Workspace.Coworking.Location));
        }

        private string GetAvailabilitySummary(Coworking coworking)
        {
            int privateRooms = 0;
            int desks = 0;
            int meetingRooms = 0;
            string summary = "";

            var workspaces = coworking.Workspaces;

            foreach(Workspaces workspace in workspaces)
            {
                workspace.WorkspaceAvailabilitys.ToList().ForEach(wa =>
                {
                    string availabilityName = wa.Availability.Name.ToLower();
                    Match match = Regex.Match(availabilityName, @"(\d+)");
                    
                    if (match.Success)
                    {
                        int count = int.Parse(match.Groups[1].Value);
                        
                        if (availabilityName.Contains("meeting rooms") || availabilityName.Contains("meeting room"))
                        {
                            meetingRooms += count;
                        }
                        else if (availabilityName.Contains("private rooms") || availabilityName.Contains("private room"))
                        {
                            privateRooms += count;
                        }
                        else if (availabilityName.Contains("desks") || availabilityName.Contains("desk"))
                        {
                            desks += count;
                        }
                        else if (availabilityName.Contains("rooms") || availabilityName.Contains("room"))
                        {
                            if (!availabilityName.Contains("no"))
                            {
                                privateRooms += count;
                            }
                        }
                    }
                });
            }
            var summaryParts = new List<string>();
            
            if (desks > 0)
            {
                summaryParts.Add($"🪑 {desks} desks");
            }
            
            if (privateRooms > 0)
            {
                summaryParts.Add($"🔒 {privateRooms} private rooms");
            }
            
            if (meetingRooms > 0)
            {
                summaryParts.Add($"👥 {meetingRooms} meeting rooms");
            }
            
            summary = string.Join(" · ", summaryParts);
            return summary;
        }
    }
}