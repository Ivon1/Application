using AutoMapper;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;

namespace BackendCoworking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map AmenityDTO
            CreateMap<Amenities, AmenityDTO>();

            // Map WorkspaceDTO
            CreateMap<Workspaces, WorkspaceDTO>()
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity.CapacityTypeName))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.WorkspaceAmenities.Select(wa => wa.Amenity)))
                .ForMember(dest => dest.PhotoUrls, opt => opt.MapFrom(src => src.WorkspacePhotos.Select(wp => wp.Photo.ImageUrl)))
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.WorkspaceAvailabilitys.Select(wa => wa.Availability.Name)));
        }
    }
}