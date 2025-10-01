using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackendCoworking.BusinessLayer
{
    public class BookingBussinesRules(CoworkingContextData _context)
    {
        public async Task<(bool IsValid, string? ErrorMessage)> ValidateBookingAsync(Bookings booking, int? existingBookingId = null)
        {
            // Check basic fields
            var basicValidation = ValidateBasicFields(booking);
            if (!basicValidation.IsValid)
                return basicValidation;

            // Check dates
            var dateValidation = ValidateDates(booking);
            if (!dateValidation.IsValid)
                return dateValidation;

            // Check workspace and availability
            var resourceValidation = await ValidateResourcesAsync(booking);
            if (!resourceValidation.IsValid)
                return resourceValidation;

            // Check overlapping bookings
            var overlapValidation = await ValidateNoOverlappingBookingsAsync(booking, existingBookingId);
            if (!overlapValidation.IsValid)
                return overlapValidation;

            return (true, null);
        }

        public (bool IsValid, string? ErrorMessage) ValidateBasicFields(Bookings booking)
        {
            if (booking == null)
                return (false, "Booking data is required");

            if (string.IsNullOrWhiteSpace(booking.Name))
                return (false, "Name is required");

            if (string.IsNullOrWhiteSpace(booking.Email) || !booking.Email.Contains('@'))
                return (false, "Valid email is required");

            if (booking.Workspace == null || booking.Workspace.Id <= 0)
                return (false, "Valid workspace is required");

            if (booking.Availability == null || booking.Availability.Id <= 0)
                return (false, "Valid availability is required");

            return (true, null);
        }

        public (bool IsValid, string? ErrorMessage) ValidateDates(Bookings booking)
        {
            if (booking.StartDate >= booking.EndDate)
                return (false, "End date must be after start date");

            if (booking.StartDate < DateTime.UtcNow || booking.EndDate < DateTime.UtcNow)
                return (false, "Booking dates must be in the future");

            if ((booking.EndDate - booking.StartDate).TotalDays > 30)
                return (false, "Booking duration cannot exceed 30 days");

            return (true, null);
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateResourcesAsync(Bookings booking)
        {
            var workspaceId = booking.Workspace.Id;
            var workspace = await _context.Workspaces
                .Include(w => w.Capacity)
                .FirstOrDefaultAsync(w => w.Id == workspaceId);

            if (workspace == null)
                return (false, $"Workspace with ID {workspaceId} not found");

            var availabilityId = booking.Availability.Id;
            var availability = await _context.Availabilities.FindAsync(availabilityId);

            if (availability == null)
                return (false, $"Availability with ID {availabilityId} not found");

            var validCombination = await _context.WorkspaceAvailabilitys
                .AnyAsync(wa => wa.WorkspaceId == workspaceId && wa.AvailabilityId == availabilityId);

            if (!validCombination)
                return (false, "The selected availability does not belong to the selected workspace");

            return (true, null);
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateNoOverlappingBookingsAsync(Bookings booking, int? existingBookingId = null)
        {
            DateTime startDateUtc = DateTime.SpecifyKind(booking.StartDate.ToUniversalTime(), DateTimeKind.Utc);
            DateTime endDateUtc = DateTime.SpecifyKind(booking.EndDate.ToUniversalTime(), DateTimeKind.Utc);

            var workspaceId = booking.Workspace.Id;
            var availabilityId = booking.Availability.Id;

            var query = _context.Bookings
                .Where(b =>
                    b.WorkspaceId == workspaceId &&
                    b.AvailabilityId == availabilityId &&
                    ((b.StartDate <= startDateUtc && b.EndDate > startDateUtc) ||
                     (b.StartDate < endDateUtc && b.EndDate >= endDateUtc) ||
                     (b.StartDate >= startDateUtc && b.EndDate <= endDateUtc)));

            if (existingBookingId.HasValue)
                query = query.Where(b => b.Id != existingBookingId.Value);

            var overlappingBooking = await query.AnyAsync();

            if (overlappingBooking)
                return (false, "The selected time slot is already booked");

            return (true, null);
        }

        public string GenerateSuccessMessage(Bookings booking, Workspaces workspace, bool isUpdate = false)
        {
            string startDateFormatted = booking.StartDate.ToString("MMMM d, yyyy");
            string endDateFormatted = booking.EndDate.ToString("MMMM d, yyyy");

            if (isUpdate)
            {
                return $"Your {workspace.WorksapceTypeName.ToLower()} booking has been updated successfully. " +
                       $"Your {workspace.Capacity.CapacityTypeName} is now booked from {startDateFormatted} to {endDateFormatted}. " +
                       $"A confirmation has been sent to your email {booking.Email}.";
            }
            else
            {
                return $"Your {workspace.WorksapceTypeName.ToLower()} for {workspace.Capacity.CapacityTypeName} " +
                       $"is booked from {startDateFormatted} to {endDateFormatted}. " +
                       $"A confirmation has been sent to your email {booking.Email}.";
            }
        }
    }
}
