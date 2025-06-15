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

            // Creating http request to Groq API
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
            } 
            catch(Exception ex) { return BadRequest($"Error calling Groq API: {ex.Message}"); }
        }

        private string BuildSystemPrompt(List<BookingGroqRequest> bookings)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("You are a helpful coworking space assistant. " +
                "Your task is to answer questions about user's bookings based on the following data:");
            prompt.AppendLine("## Strict Rules:");
            prompt.AppendLine("1. NEVER add extra text like 'Here are your bookings' or 'Let me know...'");
            prompt.AppendLine("Rules for formatting responses:");
            prompt.AppendLine("1. Always use SINGULAR form for bookings (e.g., '1 room', '1 desk')");
            prompt.AppendLine("2. Date format: 📅 MMMM dd, yyyy (e.g., 📅 May 18, 2025)");
            prompt.AppendLine("3. Time format: HH:mm (e.g., 10:00 – 12:00)");
            prompt.AppendLine("4. If no bookings found for a date, respond: " +
                "'You don't have any bookings on [date]'");
            prompt.AppendLine("5. When listing bookings, use format: '📅 [Date] — [WorkspaceType] at [Location] ([StartTime] – [EndTime])'");
            prompt.AppendLine("6. If multiple bookings: list each on separate line without additional text");
            prompt.AppendLine();
            prompt.AppendLine("Current bookings:");

            foreach (var booking in bookings)
            {
                prompt.AppendLine($"  Start DateTime: {booking.StartDate:MMMM dd, yyyy}");
                prompt.AppendLine($"  End DateTime: {booking.EndDate:MMMM dd, yyyy}");
                prompt.AppendLine($"  WorkspaceName: {booking.WorkspaceName}");
                prompt.AppendLine($"  CoworkingName: {booking.CoworkingName}");
                prompt.AppendLine($"  CoworkingDescription: {booking.CoworkingDescription}");
                prompt.AppendLine($"  CoworkingLocation: {booking.CoworkingLocation}");
                prompt.AppendLine($"  Availability: {booking.AvailabilityName}");
                prompt.AppendLine();
            }



            return prompt.ToString();
        }
    }
}
