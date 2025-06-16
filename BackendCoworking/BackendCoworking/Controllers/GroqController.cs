using AutoMapper;
using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using BackendCoworking.Models.DTOs.GroqDTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BackendCoworking.Controllers
{
    [ApiController]
    [EnableCors("OpenCORSPolicy")]
    [Route("[controller]")]
    public class GroqController : ControllerBase
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.groq.com/openai/v1/chat/completions";
        private readonly CoworkingContextData _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public GroqController(IConfiguration configuration, 
            CoworkingContextData context, 
            IMapper mapper,
            IHttpClientFactory httpClientFactory)
        {
            _apiKey = configuration["GroqApiKey"];
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> AskGroq([FromBody] AskGroqRequest request)
        {
            // Validating the request model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Getting all bookings from the database
            var bookings = await _context.Bookings
                    .Include(b => b.Workspace)
                        .ThenInclude(w => w.WorkspaceAvailabilitys)
                            .ThenInclude(wa => wa.Availability)
                    .Include(b => b.Availability)
                    .Include(b => b.Workspace)
                        .ThenInclude(w => w.Coworking)
                    .ToListAsync();

            // Mapping bookings to BookingDTO
            var bookingsGroqRequest = _mapper.Map<List<BookingGroqRequest>>(bookings);

            // Generating the system prompt based on the bookings
            string systemPrompt = BuildSystemPrompt(bookingsGroqRequest);

            // Request body for Groq API
            var groqRequest = new GroqApiRequest
            {
                messages = new List<Message>
                {
                    new Message { role = "system", content = systemPrompt },
                    new Message { role = "user", content = request.Question }
                }
            };

            //Creating http request to Groq API
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(groqRequest),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync(_apiUrl, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                var aiResponse = root
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return Ok(new { response = aiResponse });
                //return Ok(systemPrompt);
            } 
            catch(Exception ex) { return BadRequest($"Error calling Groq API: {ex.Message}"); }
        }

        private string BuildSystemPrompt(List<BookingGroqRequest> bookings)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("You are a helpful coworking space assistant. " +
                "Your task is to answer questions about user's bookings based EXCLUSIVELY on the following data:");
            prompt.AppendLine("## Strict Rules:");
            prompt.AppendLine("1. NEVER invent or generate additional bookings");
            prompt.AppendLine("2. NEVER split a booking into multiple days - each booking is a SINGLE event");
            prompt.AppendLine("3. ALWAYS use the EXACT dates and times provided");
            prompt.AppendLine("4. NEVER add extra text like 'Here are your bookings' or 'Let me know...'");
            prompt.AppendLine("5. ALWAYS use SINGULAR form: '1 room', '1 desk'");
            prompt.AppendLine("6. Date format: 📅 MMMM dd, yyyy");
            prompt.AppendLine("7. Time format: HH:mm (24-hour)");
            prompt.AppendLine("8. Booking format: '📅 [Date] — [WorkspaceType] at [Location] ([StartTime] – [EndTime])'");
            prompt.AppendLine("4. If no bookings found for a date, respond: " +
                "'You don't have any bookings on [date]'");
            prompt.AppendLine();
            prompt.AppendLine("## Critical Instruction:");
            prompt.AppendLine("Start Time and End Time take from StartDateTime and from EndDateTime");
            prompt.AppendLine("Each booking below is a SINGLE event. Use the EXACT start date/time and end date/time as provided. DO NOT split into multiple days.");
            prompt.AppendLine();
            prompt.AppendLine("## Bookings Data:");

            foreach (var booking in bookings)
            {
                prompt.AppendLine($"  StartDate: {booking.StartDate.Date}");
                prompt.AppendLine($"  StartTime: {booking.StartDate.ToString("hh:mm")}");
                prompt.AppendLine($"  EndDate: {booking.EndDate.Date}");
                prompt.AppendLine($"  EndTime: {booking.EndDate.ToString("hh:mm")}");
                prompt.AppendLine($"  Workspace: {booking.WorkspaceName}");
                prompt.AppendLine($"  Details: {booking.CoworkingDescription}");
                prompt.AppendLine($"  Location: {booking.CoworkingName}");
                prompt.AppendLine($"  Address: {booking.CoworkingLocation}");
                prompt.AppendLine();
            }

            //prompt.AppendLine("## Response Examples:");
            //prompt.AppendLine("For the booking: July 13, 2025 12:00 to July 16, 2025 13:00");
            //prompt.AppendLine("Correct: '📅 July 13, 2025 — Meeting room at WorkClub Pechersk (12:00 – 13:00)'");
            //prompt.AppendLine("Incorrect: '📅 July 13, 2025 — Meeting room...' + '📅 July 14, 2025 — Meeting room...'");
            //prompt.AppendLine();
            //prompt.AppendLine("For the booking: February 04, 2026 09:00 to February 05, 2026 11:30");
            //prompt.AppendLine("Correct: '📅 February 04, 2026 — Private room at WorkClub Pechersk (09:00 – 11:30)'");
            //prompt.AppendLine("Incorrect any split across multiple days");

            return prompt.ToString();
        }
    }
}
