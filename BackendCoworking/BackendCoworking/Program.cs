using AutoMapper;
using BackendCoworking.DatabaseSets;
using BackendCoworking.Mappings;
using BackendCoworking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

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
            new Amenities { Id = 1, Name = "Wi-Fi", ImageUrl = "https://i.ibb.co/WpPk1hT1/wifi.png" },
            new Amenities { Id = 2, Name = "Games", ImageUrl = "https://i.ibb.co/nMHCp248/device-gamepad-2.png" },
            new Amenities { Id = 3, Name = "Coffee", ImageUrl = "https://i.ibb.co/qMgP2y4d/coffee.png" },
            new Amenities { Id = 4, Name = "AirCondition", ImageUrl = "https://i.ibb.co/F4Jt7X6Z/air-conditioning.png" },
            new Amenities { Id = 5, Name = "Microphone", ImageUrl = "https://i.ibb.co/hFVCcGFD/microphone.png" }
        };
        context.Set<Amenities>().AddRange(amenities);

        // Add photos
        // Add photos
        var photos = new List<Photos>();
        photos.Add(new Photos { Id = 1, ImageUrl = "https://i.ibb.co/27RhrnyY/101931b7ae6f38119bf6b9fa9832610f6c6497ef.png" });
        photos.Add(new Photos { Id = 2, ImageUrl = "https://i.ibb.co/21CFf0tm/d5653107a69b20b64f00fb13a812060c8e0c1071.png" });
        photos.Add(new Photos { Id = 3, ImageUrl = "https://i.ibb.co/Mk38Y16j/e90bb3f8c112a868ce37c2a7181d36e2a8b39e51.jpg" });
        photos.Add(new Photos { Id = 4, ImageUrl = "https://i.ibb.co/GQ40MTvk/3b13dc57f5288df1675a9f4dc8fcaef0553a0f90.png" });
        photos.Add(new Photos { Id = 5, ImageUrl = "https://i.ibb.co/9kY419bM/4cb6e6b2ab60838a32a15fac8aca47acb834d1e7.png" });
        photos.Add(new Photos { Id = 6, ImageUrl = "https://i.ibb.co/N0z47v3/07e10141c7a2c2d51b36e43d4ca73d7c0753ed5b.png" });
        photos.Add(new Photos { Id = 7, ImageUrl = "https://i.ibb.co/KjZDT1pp/99daee0af82ef7f050a23b4f5df0e622a61dcc04.jpg" });
        photos.Add(new Photos { Id = 8, ImageUrl = "https://i.ibb.co/KjkHZXHj/b2e8c94bc3c1931bc3f2a7979dc48698ed605a5a.jpg" });
        photos.Add(new Photos { Id = 9, ImageUrl = "https://i.ibb.co/nMPJtbSY/afc3b33e8090e3f9bcad561d128a8719faad4c3d.png" });
        photos.Add(new Photos { Id = 10, ImageUrl = "https://i.ibb.co/DDxKJpjs/0ab5a14f0fdcf9012f51cd4b501c5ca12e98c72e.png" });
        photos.Add(new Photos { Id = 11, ImageUrl = "https://i.ibb.co/Kcp542h2/0fa6936c82bab3a7caab82b4c277f6a732667f5a.png" });
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

        // Meeting room photos (10-11)
        for (int i = 8; i <= 11; i++)
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
            new Amenities { Id = 1, Name = "Wi-Fi", ImageUrl = "https://i.ibb.co/WpPk1hT1/wifi.png" },
            new Amenities { Id = 2, Name = "Games", ImageUrl = "https://i.ibb.co/nMHCp248/device-gamepad-2.png" },
            new Amenities { Id = 3, Name = "Coffee", ImageUrl = "https://i.ibb.co/qMgP2y4d/coffee.png" },
            new Amenities { Id = 4, Name = "AirCondition", ImageUrl = "https://i.ibb.co/F4Jt7X6Z/air-conditioning.png" },
            new Amenities { Id = 5, Name = "Microphone", ImageUrl = "https://i.ibb.co/hFVCcGFD/microphone.png" }
        };
        await context.Set<Amenities>().AddRangeAsync(amenities, cancellationToken);

        // Add photos
        var photos = new List<Photos>();
        photos.Add(new Photos { Id = 1, ImageUrl = "https://i.ibb.co/27RhrnyY/101931b7ae6f38119bf6b9fa9832610f6c6497ef.png" });
        photos.Add(new Photos { Id = 2, ImageUrl = "https://i.ibb.co/21CFf0tm/d5653107a69b20b64f00fb13a812060c8e0c1071.png" });
        photos.Add(new Photos { Id = 3, ImageUrl = "https://i.ibb.co/Mk38Y16j/e90bb3f8c112a868ce37c2a7181d36e2a8b39e51.jpg" });
        photos.Add(new Photos { Id = 4, ImageUrl = "https://i.ibb.co/GQ40MTvk/3b13dc57f5288df1675a9f4dc8fcaef0553a0f90.png" });
        photos.Add(new Photos { Id = 5, ImageUrl = "https://i.ibb.co/9kY419bM/4cb6e6b2ab60838a32a15fac8aca47acb834d1e7.png" });
        photos.Add(new Photos { Id = 6, ImageUrl = "https://i.ibb.co/N0z47v3/07e10141c7a2c2d51b36e43d4ca73d7c0753ed5b.png" });
        photos.Add(new Photos { Id = 7, ImageUrl = "https://i.ibb.co/KjZDT1pp/99daee0af82ef7f050a23b4f5df0e622a61dcc04.jpg" });
        photos.Add(new Photos { Id = 8, ImageUrl = "https://i.ibb.co/KjkHZXHj/b2e8c94bc3c1931bc3f2a7979dc48698ed605a5a.jpg" });
        photos.Add(new Photos { Id = 9, ImageUrl = "https://i.ibb.co/nMPJtbSY/afc3b33e8090e3f9bcad561d128a8719faad4c3d.png" });
        photos.Add(new Photos { Id = 10, ImageUrl = "https://i.ibb.co/DDxKJpjs/0ab5a14f0fdcf9012f51cd4b501c5ca12e98c72e.png" });
        photos.Add(new Photos { Id = 11, ImageUrl = "https://i.ibb.co/Kcp542h2/0fa6936c82bab3a7caab82b4c277f6a732667f5a.png" });
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
        
        // Private room photos (5-9)
        for (int i = 5; i <= 8; i++)
            workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = 2, PhotoId = i });

        // Meeting room photos (10-11)
        for (int i = 8; i <= 11; i++)
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