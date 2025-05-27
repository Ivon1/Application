using BackendCoworking.DatabaseSets;
using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackendCoworking.Mappings;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
string CORSOpenPolicy = "OpenCORSPolicy";

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => {
          builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
      });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Coworking Space API",
        Version = "v1",
        Description = "API for managing coworking spaces and bookings"
    });
});

builder.Services.AddDbContext<CoworkingContextData>(
    options => options.UseNpgsql(connectionString)
    .UseSeeding((context, _) =>
    {
        // Check if data already exists
        if (context.Set<Workspaces>().Any())
            return;

        // Add capacities
        var capacities = new List<Capacity>
        {
            new Capacity { Id = 1, CapacityTypeName = "1 person" },
            new Capacity { Id = 2, CapacityTypeName = "2 people" },
            new Capacity { Id = 3, CapacityTypeName = "5 people" },
            new Capacity { Id = 4, CapacityTypeName = "10 people" },
            new Capacity { Id = 5, CapacityTypeName = "20 people" }
        };
        context.Set<Capacity>().AddRange(capacities);

        // Add workspaces
        var workspaces = new List<Workspaces>
        {
            new Workspaces
            {
                Id = 1,
                WorksapceTypeName = "Open space",
                Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                CapacityId = 1 // Default to 1 person capacity (per desk)
            },
            new Workspaces
            {
                Id = 2,
                WorksapceTypeName = "Private rooms",
                Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
                CapacityId = 2 // Default to 2 people capacity
            },
            new Workspaces
            {
                Id = 3,
                WorksapceTypeName = "Meeting rooms",
                Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
                CapacityId = 4 // Default to 10 people capacity
            }
        };
        context.Set<Workspaces>().AddRange(workspaces);

        // Add amenities
        var amenities = new List<Amenities>
        {
            new Amenities { Id = 1, Name = "Wi-Fi", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 2, Name = "Games", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 3, Name = "Coffee", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 4, Name = "Projector", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 5, Name = "Microphone", ImageUrl = "https://placehold.co/600x400/EEE/31343C" }
        };
        context.Set<Amenities>().AddRange(amenities);

        // Add photos
        var photos = new List<Photos>();
        for (int i = 1; i <= 11; i++)
        {
            photos.Add(new Photos { Id = i, ImageUrl = "https://placehold.co/600x400/EEE/31343C" });
        }
        context.Set<Photos>().AddRange(photos);

        // Add availabilities
        var availabilities = new List<Availability>
        {
            new Availability { Id = 1, Name = "24 desks" },
            new Availability { Id = 2, Name = "7 rooms for 1 person" },
            new Availability { Id = 3, Name = "4 rooms for up to 2 people" },
            new Availability { Id = 4, Name = "3 rooms for up to 5 people" },
            new Availability { Id = 5, Name = "1 room for up to 10 people" },
            new Availability { Id = 6, Name = "4 rooms for up to 10 people" },
            new Availability { Id = 7, Name = "1 room for up to 20 people" }
        };
        context.Set<Availability>().AddRange(availabilities);

        // Save before creating relationships
        context.SaveChanges();

        // Add workspace photos
        var workspacePhotos = new List<WorkspacePhotos>();
        
        // Open space photos (1-4)
        for (int i = 1; i <= 4; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 1, PhotoId = i });
        
        // Private room photos (5-8)
        for (int i = 5; i <= 8; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 2, PhotoId = i });
        
        // Meeting room photos (9-11)
        for (int i = 9; i <= 11; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 3, PhotoId = i });
            
        context.Set<WorkspacePhotos>().AddRange(workspacePhotos);

        // Add workspace amenities
        var workspaceAmenities = new List<WorkspaceAmenitys>
        {
            // Open space amenities (WiFi, Games, Coffee)
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 2 },
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 3 },
            
            // Private room amenities (WiFi, Projector, Coffee)
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 4 },
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 3 },
            
            // Meeting room amenities (WiFi, Projector, Coffee, Microphone)
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 4 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 3 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 5 }
        };
        context.Set<WorkspaceAmenitys>().AddRange(workspaceAmenities);

        // Add workspace availabilities
        var workspaceAvailabilities = new List<WorkspaceAvailabilitys>
        {
            // Open space availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 1, AvailabilityId = 1 }, // 24 desks
            
            // Private room availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 2 }, // 7 rooms for 1 person
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 3 }, // 4 rooms for up to 2 people
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 4 }, // 3 rooms for up to 5 people
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 5 }, // 1 room for up to 10 people
            
            // Meeting room availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 6 }, // 4 rooms for up to 10 people
            new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 7 }  // 1 room for up to 20 people
        };
        context.Set<WorkspaceAvailabilitys>().AddRange(workspaceAvailabilities);

        context.SaveChanges();
    })
    .UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        // Check if data already exists
        if (await context.Set<Workspaces>().AnyAsync(cancellationToken))
            return;

        // Add capacities
        var capacities = new List<Capacity>
        {
            new Capacity { Id = 1, CapacityTypeName = "1 person" },
            new Capacity { Id = 2, CapacityTypeName = "2 people" },
            new Capacity { Id = 3, CapacityTypeName = "5 people" },
            new Capacity { Id = 4, CapacityTypeName = "10 people" },
            new Capacity { Id = 5, CapacityTypeName = "20 people" }
        };
        await context.Set<Capacity>().AddRangeAsync(capacities, cancellationToken);

        // Add workspaces
        var workspaces = new List<Workspaces>
        {
            new Workspaces
            {
                Id = 1,
                WorksapceTypeName = "Open space",
                Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                CapacityId = 1 // Default to 1 person capacity (per desk)
            },
            new Workspaces
            {
                Id = 2,
                WorksapceTypeName = "Private rooms",
                Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
                CapacityId = 2 // Default to 2 people capacity
            },
            new Workspaces
            {
                Id = 3,
                WorksapceTypeName = "Meeting rooms",
                Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
                CapacityId = 4 // Default to 10 people capacity
            }
        };
        await context.Set<Workspaces>().AddRangeAsync(workspaces, cancellationToken);

        // Add amenities
        var amenities = new List<Amenities>
        {
            new Amenities { Id = 1, Name = "Wi-Fi", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 2, Name = "Games", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 3, Name = "Coffee", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 4, Name = "Projector", ImageUrl = "https://placehold.co/600x400/EEE/31343C" },
            new Amenities { Id = 5, Name = "Microphone", ImageUrl = "https://placehold.co/600x400/EEE/31343C" }
        };
        await context.Set<Amenities>().AddRangeAsync(amenities, cancellationToken);

        // Add photos
        var photos = new List<Photos>();
        for (int i = 1; i <= 11; i++)
        {
            photos.Add(new Photos { Id = i, ImageUrl = "https://placehold.co/600x400/EEE/31343C" });
        }
        await context.Set<Photos>().AddRangeAsync(photos, cancellationToken);

        // Add availabilities
        var availabilities = new List<Availability>
        {
            new Availability { Id = 1, Name = "24 desks" },
            new Availability { Id = 2, Name = "7 rooms for 1 person" },
            new Availability { Id = 3, Name = "4 rooms for up to 2 people" },
            new Availability { Id = 4, Name = "3 rooms for up to 5 people" },
            new Availability { Id = 5, Name = "1 room for up to 10 people" },
            new Availability { Id = 6, Name = "4 rooms for up to 10 people" },
            new Availability { Id = 7, Name = "1 room for up to 20 people" }
        };
        await context.Set<Availability>().AddRangeAsync(availabilities, cancellationToken);

        // Save before creating relationships
        await context.SaveChangesAsync(cancellationToken);

        // Add workspace photos
        var workspacePhotos = new List<WorkspacePhotos>();
        
        // Open space photos (1-4)
        for (int i = 1; i <= 4; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 1, PhotoId = i });
        
        // Private room photos (5-8)
        for (int i = 5; i <= 8; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 2, PhotoId = i });
        
        // Meeting room photos (9-11)
        for (int i = 9; i <= 11; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 3, PhotoId = i });
            
        await context.Set<WorkspacePhotos>().AddRangeAsync(workspacePhotos, cancellationToken);

        // Add workspace amenities
        var workspaceAmenities = new List<WorkspaceAmenitys>
        {
            // Open space amenities (WiFi, Games, Coffee)
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 2 },
            new WorkspaceAmenitys { WorkspaceId = 1, AmenityId = 3 },
            
            // Private room amenities (WiFi, Projector, Coffee)
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 4 },
            new WorkspaceAmenitys { WorkspaceId = 2, AmenityId = 3 },
            
            // Meeting room amenities (WiFi, Projector, Coffee, Microphone)
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 1 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 4 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 3 },
            new WorkspaceAmenitys { WorkspaceId = 3, AmenityId = 5 }
        };
        await context.Set<WorkspaceAmenitys>().AddRangeAsync(workspaceAmenities, cancellationToken);

        // Add workspace availabilities
        var workspaceAvailabilities = new List<WorkspaceAvailabilitys>
        {
            // Open space availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 1, AvailabilityId = 1 }, // 24 desks
            
            // Private room availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 2 }, // 7 rooms for 1 person
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 3 }, // 4 rooms for up to 2 people
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 4 }, // 3 rooms for up to 5 people
            new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 5 }, // 1 room for up to 10 people
            
            // Meeting room availabilities
            new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 6 }, // 4 rooms for up to 10 people
            new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 7 }  // 1 room for up to 20 people
        };
        await context.Set<WorkspaceAvailabilitys>().AddRangeAsync(workspaceAvailabilities, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    })
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CoworkingContextData>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coworking Space API v1"));
}

app.UseHttpsRedirection();
app.UseCors(CORSOpenPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();