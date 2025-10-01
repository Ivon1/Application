using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.Repositories
{
    public class BookingsRepository(CoworkingContextData _context)
    {
        public async Task<List<Bookings>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Workspace)
                        .ThenInclude(b => b.Capacity)
                    .Include(w => w.Workspace)
                        .ThenInclude(w => w.WorkspaceAmenities)
                            .ThenInclude(wa => wa.Amenity)
                    .Include(w => w.Workspace)
                        .ThenInclude(w => w.WorkspacePhotos)
                            .ThenInclude(wp => wp.Photo)
                    .Include(b => b.Workspace)
                        .ThenInclude(w => w.WorkspaceAvailabilitys)
                            .ThenInclude(wa => wa.Availability)
                    .Include(b => b.Availability)
                .ToListAsync();
        }

        public async Task<Bookings?> GetBookingById(int bookingId)
        {
            var booking = await _context.Bookings
                    .Include(b => b.Workspace)
                        .ThenInclude(b => b.Capacity)
                    .Include(w => w.Workspace)
                        .ThenInclude(w => w.WorkspaceAmenities)
                            .ThenInclude(wa => wa.Amenity)
                    .Include(w => w.Workspace)
                        .ThenInclude(w => w.WorkspacePhotos)
                            .ThenInclude(wp => wp.Photo)
                    .Include(b => b.Workspace)
                        .ThenInclude(w => w.WorkspaceAvailabilitys)
                            .ThenInclude(wa => wa.Availability)
                    .Include(b => b.Availability)
                    .FirstOrDefaultAsync(b => b.Id == bookingId);

            return booking;
        }

        public async Task<bool> DeleteBookingById(int bookingId)
        {
            bool deleteResult = false;
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    deleteResult = true;
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error happend while " +
                $"trying to remove booking with id -> {bookingId}" +
                $"Error message: {ex.Message}"); 
            }

            return deleteResult;
        }

        public async Task AddBookingAsync(Bookings newBooking)
        {
            try
            {
                await _context.Bookings.AddAsync(newBooking);
                _context.SaveChanges();
            } 
            catch(Exception ex)
            {
                Console.WriteLine($"Error happend while " +
                    $"trying to add booking with id -> {newBooking.Id}" +
                    $"Error message: {ex.Message}");
            }
        }

        public async Task UpdateBookingAsync(Bookings updatedBooking)
        {
            try
            {
                _context.Bookings.Update(updatedBooking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error happend while " +
                    $"trying to update booking with id -> {updatedBooking.Id}" +
                    $"Error message: {ex.Message}");
            }
        }
    }
}
