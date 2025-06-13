using AutoMapper;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;

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

            //CreateMap<BookingDTO, Bookings>()
            //    .ForMember(dest => dest.Workspace, opt => opt.Ignore())
            //    .ForMember(dest => dest.Availability, opt => opt.Ignore())
            //    .ForMember(dest => dest.WorkspaceId, opt => opt.MapFrom(src => src.Workspace != null ? src.Workspace.Id : 0))
            //    .ForMember(dest => dest.AvailabilityId, opt => opt.MapFrom(src => src.Availability != null ? src.Availability.Id : 0));

            // Map from Coworking to CoworkingDTO
            CreateMap<Coworking, CoworkingDTO>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.Location, src => src.MapFrom(x => x.Location))
                .ForMember(dest => dest.PhotoUrl, src => src.MapFrom(x => x.Photo.ImageUrl));
        }
    }
}