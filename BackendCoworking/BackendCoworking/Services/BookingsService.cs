using AutoMapper;
using BackendCoworking.BusinessLayer;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Models;
using BackendCoworking.Repositories;

namespace BackendCoworking.Services
{
    public class BookingsService(
        BookingsRepository _repository, 
        IMapper _mapper, 
        BookingBussinesRules _bookingBussinesRules)
    {
        public async Task<List<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = await _repository.GetAllBookingsAsync();
            return _mapper.Map<List<BookingDTO>>(bookings);
        }

        public async Task<BookingDTO> GetBookingById(int bookingId)
        {
            var booking = await _repository.GetBookingById(bookingId);
            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<bool> DeleteBookingById(int booking)
        {
            return await _repository.DeleteBookingById(booking);
        }

        public async Task<(bool, string?)> CreateBookingAsync(BookingDTO bookingDTO)
        {
            var booking = _mapper.Map<Bookings>(bookingDTO);
            var validation = await _bookingBussinesRules.ValidateBookingAsync(booking);
            
            if(validation.IsValid)
            {
                await _repository.AddBookingAsync(booking);
                return (validation.IsValid, _bookingBussinesRules.GenerateSuccessMessage
                    (booking, booking.Workspace, false));
            }
            else
            {
                return (validation.IsValid, validation.ErrorMessage);
            }
        }
    }
}
