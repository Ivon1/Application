using AutoMapper;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Models.DTOs.GroqDTOs;

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

            // Map from BookingsDTO to Bookings
            CreateMap<BookingDTO, Bookings>();

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
                .ForMember(dest => dest.PhotoUrl, src => src.MapFrom(x => x.Photo.ImageUrl));

            // Map from Booking to BookingGroqRequest
            CreateMap<Bookings, BookingGroqRequest>()
                .ForMember(dest => dest.CoworkingName, src => src.MapFrom(x => x.Workspace.Coworking.Name))
                .ForMember(dest => dest.WorkspaceName, src => src.MapFrom(x => x.Workspace.WorksapceTypeName))
                .ForMember(dest => dest.AvailabilityName, src => src.MapFrom(x => x.Availability.Name))
                .ForMember(dest => dest.StartDate, src => src.MapFrom(x => x.StartDate))
                .ForMember(dest => dest.EndDate, src => src.MapFrom(x => x.EndDate))
                .ForMember(dest => dest.CoworkingDescription, src => src.MapFrom(x => x.Workspace.Coworking.Description))
                .ForMember(dest => dest.CoworkingLocation, src => src.MapFrom(x => x.Workspace.Coworking.Location));

            //Map from BookingDTO to Bookings
            CreateMap<BookingDTO, Bookings>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.StartDate, src => src.MapFrom(x => x.StartDate))
                .ForMember(dest => dest.EndDate, src => src.MapFrom(x => x.EndDate))
                .ForMember(dest => dest.WorkspaceId, src => src.MapFrom(x => x.Workspace.Id))
                .ForMember(dest => dest.AvailabilityId, src => src.MapFrom(x => x.Availability.Id));
        }
    }
}