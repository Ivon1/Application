using AutoMapper;
using BackendCoworking.BusinessLayer;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Repositories;

namespace BackendCoworking.Services
{
    public class CoworkingService(CoworkingRepository _coworkingRepository, IMapper _mapper)
    {
        public async Task<List<CoworkingDTO>> GetAllCoworkingsAsync()
        {
            var coworkings = await _coworkingRepository.GetAllCoworkingsAsync();

            var coworkingsDTOs = _mapper.Map<List<CoworkingDTO>>(coworkings);

            foreach (var coworkingDTO in coworkingsDTOs)
            {
                var coworking = coworkings.FirstOrDefault(x => x.Id == coworkingDTO.Id);
                if (coworking != null)
                {
                    coworkingDTO.AvailabilitySummary = CoworkingBussinesRules.GetAvailabilitySummary(coworking);
                }
            }

            return coworkingsDTOs;
        }
    }
}
