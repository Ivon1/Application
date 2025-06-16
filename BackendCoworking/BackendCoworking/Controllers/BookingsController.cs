using AutoMapper;
using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        // Dependency injection for the database context and mapper
        private readonly CoworkingContextData _context;
        private readonly IMapper _mapper;

        public BookingsController(CoworkingContextData context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET All Bookings
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _context.Bookings
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

                var bookingDtos = _mapper.Map<List<BookingDTO>>(bookings);

                return Ok(bookingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while retrieving bookings",
                    Error = ex.Message
                });
            }
        }

        // GET Booking by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
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
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (booking == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Booking with ID {id} not found"
                    });
                }
                var bookingDto = _mapper.Map<BookingDTO>(booking);
                return Ok(bookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while retrieving the booking",
                    Error = ex.Message
                });
            }
        }

        // POST Create Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            try
            {
                // Validate input
                if (bookingDto == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Booking data is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(bookingDto.Name))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Name is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(bookingDto.Email) || !bookingDto.Email.Contains('@'))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid email is required"
                    });
                }

                if (bookingDto.StartDate >= bookingDto.EndDate)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "End date must be after start date"
                    });
                }

                if (bookingDto.Workspace == null || bookingDto.Workspace.Id <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid workspace is required"
                    });
                }

                if (bookingDto.Availability == null || bookingDto.Availability.Id <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid availability is required"
                    });
                }

                // Check if workspace exists
                var workspaceId = bookingDto.Workspace.Id;
                var workspace = await _context.Workspaces
                    .Include(w => w.Capacity)
                    .FirstOrDefaultAsync(w => w.Id == workspaceId);

                if (workspace == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = $"Workspace with ID {workspaceId} not found"
                    });
                }

                // Check if availability exists
                var availabilityId = bookingDto.Availability.Id;
                var availability = await _context.Availabilities.FindAsync(availabilityId);

                if (availability == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = $"Availability with ID {availabilityId} not found"
                    });
                }

                // Check if availability belongs to workspace
                var validCombination = await _context.WorkspaceAvailabilitys
                    .AnyAsync(wa => wa.WorkspaceId == workspaceId && wa.AvailabilityId == availabilityId);

                if (!validCombination)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "The selected availability does not belong to the selected workspace"
                    });
                }

                // Check if booking dates are in future 
                if(bookingDto.StartDate < DateTime.UtcNow || bookingDto.EndDate < DateTime.UtcNow)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Booking dates must be in the future"
                    });
                }

                // Check if start date and end date are not more than 30 days apart
                if ((bookingDto.EndDate - bookingDto.StartDate).TotalDays > 30)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Booking duration cannot exceed 30 days"
                    });
                }


                // Convert local dates to UTC dates
                DateTime startDateUtc = DateTime.SpecifyKind(bookingDto.StartDate.ToUniversalTime(), DateTimeKind.Utc);
                DateTime endDateUtc = DateTime.SpecifyKind(bookingDto.EndDate.ToUniversalTime(), DateTimeKind.Utc);

                // Check for overlapping bookings
                var overlappingBooking = await _context.Bookings
                    .AnyAsync(b =>
                        b.WorkspaceId == workspaceId &&
                        b.AvailabilityId == availabilityId &&
                        ((b.StartDate <= startDateUtc && b.EndDate > startDateUtc) ||
                         (b.StartDate < endDateUtc && b.EndDate >= endDateUtc) ||
                         (b.StartDate >= startDateUtc && b.EndDate <= endDateUtc)));

                if (overlappingBooking)
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "The selected time slot is already booked"
                    });
                }

                // Create new booking entity with UTC dates
                var booking = new Bookings
                {
                    Name = bookingDto.Name,
                    Email = bookingDto.Email,
                    WorkspaceId = workspaceId,
                    AvailabilityId = availabilityId,
                    StartDate = startDateUtc,
                    EndDate = endDateUtc
                };

                // Save to database
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Format dates for the success message
                string startDateFormatted = bookingDto.StartDate.ToString("MMMM d, yyyy");
                string endDateFormatted = bookingDto.EndDate.ToString("MMMM d, yyyy");

                // Create success message
                string successMessage = $"Your {workspace.WorksapceTypeName.ToLower()} for {workspace.Capacity.CapacityTypeName} is booked from {startDateFormatted} to {endDateFormatted}. A confirmation has been sent to your email {bookingDto.Email}.";

                // Return success response with the formatted message
                return Ok(new
                {
                    Success = true,
                    Message = successMessage,
                    BookingId = booking.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while creating the booking",
                    Error = ex.Message
                });
            }
        }

        // DELETE Booking by ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                // Find the booking by ID
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Booking with ID {id} not found"
                    });
                }

                // Remove the booking from the database
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = $"Booking with ID {id} has been successfully deleted"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while deleting the booking",
                    Error = ex.Message
                });
            }
        }

        // PUT Update Booking
        [HttpPut]
        public async Task<IActionResult> UpdateBooking([FromBody] BookingDTO bookingDTO)
        {
            try
            {
                // Validate input
                if (bookingDTO == null || bookingDTO.Id <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid booking data with ID is required"
                    });
                }

                // Find the existing booking
                var existingBooking = await _context.Bookings.FindAsync(bookingDTO.Id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Booking with ID {bookingDTO.Id} not found"
                    });
                }

                if (string.IsNullOrWhiteSpace(bookingDTO.Name))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Name is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(bookingDTO.Email) || !bookingDTO.Email.Contains('@'))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid email is required"
                    });
                }

                if (bookingDTO.StartDate >= bookingDTO.EndDate)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "End date must be after start date"
                    });
                }

                if (bookingDTO.Workspace == null || bookingDTO.Workspace.Id <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid workspace is required"
                    });
                }

                if (bookingDTO.Availability == null || bookingDTO.Availability.Id <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Valid availability is required"
                    });
                }

                // Check if booking dates are in future 
                if (bookingDTO.StartDate < DateTime.UtcNow || bookingDTO.EndDate < DateTime.UtcNow)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Booking dates must be in the future"
                    });
                }

                // Check if start date and end date are not more than 30 days apart
                if ((bookingDTO.EndDate - bookingDTO.StartDate).TotalDays > 30)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Booking duration cannot exceed 30 days"
                    });
                }

                // Check if workspace exists
                var workspaceId = bookingDTO.Workspace.Id;
                var workspace = await _context.Workspaces
                    .Include(w => w.Capacity)
                    .FirstOrDefaultAsync(w => w.Id == workspaceId);

                if (workspace == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = $"Workspace with ID {workspaceId} not found"
                    });
                }

                // Check if availability exists
                var availabilityId = bookingDTO.Availability.Id;
                var availability = await _context.Availabilities.FindAsync(availabilityId);

                if (availability == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = $"Availability with ID {availabilityId} not found"
                    });
                }

                // Check if availability belongs to workspace
                var validCombination = await _context.WorkspaceAvailabilitys
                    .AnyAsync(wa => wa.WorkspaceId == workspaceId && wa.AvailabilityId == availabilityId);

                if (!validCombination)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "The selected availability does not belong to the selected workspace"
                    });
                }

                // Convert local dates to UTC dates
                DateTime startDateUtc = DateTime.SpecifyKind(bookingDTO.StartDate.ToUniversalTime(), DateTimeKind.Utc);
                DateTime endDateUtc = DateTime.SpecifyKind(bookingDTO.EndDate.ToUniversalTime(), DateTimeKind.Utc);

                // Check for overlapping bookings (excluding the current booking)
                var overlappingBooking = await _context.Bookings
                    .AnyAsync(b =>
                        b.Id != bookingDTO.Id && // Exclude the current booking
                        b.WorkspaceId == workspaceId &&
                        b.AvailabilityId == availabilityId &&
                        ((b.StartDate <= startDateUtc && b.EndDate > startDateUtc) ||
                         (b.StartDate < endDateUtc && b.EndDate >= endDateUtc) ||
                         (b.StartDate >= startDateUtc && b.EndDate <= endDateUtc)));

                if (overlappingBooking)
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "The selected time slot is already booked"
                    });
                }

                // Update booking properties
                existingBooking.Name = bookingDTO.Name;
                existingBooking.Email = bookingDTO.Email;
                existingBooking.WorkspaceId = workspaceId;
                existingBooking.AvailabilityId = availabilityId;
                existingBooking.StartDate = startDateUtc;
                existingBooking.EndDate = endDateUtc;

                // Save changes
                await _context.SaveChangesAsync();

                // Format dates for the success message
                string startDateFormatted = bookingDTO.StartDate.ToString("MMMM d, yyyy");
                string endDateFormatted = bookingDTO.EndDate.ToString("MMMM d, yyyy");

                string successMessage = $"Your {workspace.WorksapceTypeName.ToLower()} booking has been updated successfully. Your {workspace.Capacity.CapacityTypeName} is now booked from {startDateFormatted} to {endDateFormatted}. A confirmation has been sent to your email {bookingDTO.Email}.";

                // Return success response
                return Ok(new
                {
                    Success = true,
                    Message = successMessage,
                    BookingId = existingBooking.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while updating the booking",
                    Error = ex.Message
                });
            }
        }
    }
}
