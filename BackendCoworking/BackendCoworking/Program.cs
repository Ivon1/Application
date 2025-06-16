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
builder.Services.AddHttpClient();

// Configure Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder =>
      {
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

            // Add photos for workspaces and coworkings
            var photos = new List<Photos>();
            // Workspace photos
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

            // Coworking photos
            photos.Add(new Photos { Id = 12, ImageUrl = "https://i.ibb.co/PSGtC9P/Pechersk.jpg" }); // WorkClub Pechersk
            photos.Add(new Photos { Id = 13, ImageUrl = "https://i.ibb.co/8DyFP071/podil.jpg" }); // UrbanSpace Podil 
            photos.Add(new Photos { Id = 14, ImageUrl = "https://i.ibb.co/vvJgR0P9/lvivska.jpg" }); // Creative Hub Lvivska 
            photos.Add(new Photos { Id = 15, ImageUrl = "https://i.ibb.co/hJZjQvJB/olimpiska.jpg" }); // TechNest Olimpiiska 
            photos.Add(new Photos { Id = 16, ImageUrl = "https://i.ibb.co/vxZYXXRP/troya.jpg" }); // Hive Station Troieshchyna 

            context.Set<Photos>().AddRange(photos);

            // Add coworkings
            var coworkings = new List<Coworking>
        {
            new Coworking
            {
                Id = 1,
                Name = "WorkClub Pechersk",
                Description = "Modern coworking in the heart of Pechersk with quiet rooms and coffee on tap.",
                Location = "123 Yaroslaviv Val St, Kyiv",
                PhotoId = 12
            },
            new Coworking
            {
                Id = 2,
                Name = "UrbanSpace Podil",
                Description = "A creative riverside hub ideal for freelancers and small startups.",
                Location = "78 Naberezhno-Khreshchatytska St, Kyiv",
                PhotoId = 13
            },
            new Coworking
            {
                Id = 3,
                Name = "Creative Hub Lvivska",
                Description = "A compact, design-focused space with open desks and strong community vibes.",
                Location = "12 Lvivska Square, Kyiv",
                PhotoId = 14
            },
            new Coworking
            {
                Id = 4,
                Name = "TechNest Olimpiiska",
                Description = "A high-tech space near Olimpiiska metro, perfect for team sprints and solo focus.",
                Location = "45 Velyka Vasylkivska St, Kyiv",
                PhotoId = 15
            },
            new Coworking
            {
                Id = 5,
                Name = "Hive Station Troieshchyna",
                Description = "A quiet, affordable option in the city's northeast—great for remote workers.",
                Location = "102 Zakrevskogo St, Kyiv",
                PhotoId = 16
            }
        };
            context.Set<Coworking>().AddRange(coworkings);
            context.SaveChanges();

            // Create workspaces for each coworking
            var workspaces = new List<Workspaces>();

            // For each coworking, create the standard workspace types
            foreach (var coworking in coworkings)
            {
                // Create base workspace ID for this coworking
                int baseId = (coworking.Id - 1) * 3 + 1;

                // Add Open space
                workspaces.Add(new Workspaces
                {
                    Id = baseId,
                    WorksapceTypeName = "Open space",
                    Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                    CapacityId = 1,
                    CoworkingId = coworking.Id
                });

                // Add Private rooms
                workspaces.Add(new Workspaces
                {
                    Id = baseId + 1,
                    WorksapceTypeName = "Private rooms",
                    Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
                    CapacityId = 2,
                    CoworkingId = coworking.Id
                });

                // Add Meeting rooms
                workspaces.Add(new Workspaces
                {
                    Id = baseId + 2,
                    WorksapceTypeName = "Meeting rooms",
                    Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
                    CapacityId = 4,
                    CoworkingId = coworking.Id
                });
            }
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

            // Add availabilities
            var availabilities = new List<Availability>
        {
            // Generic availabilities
            new Availability { Id = 1, Name = "24 desks" },
            new Availability { Id = 2, Name = "7 rooms for 1 person" },
            new Availability { Id = 3, Name = "4 rooms for up to 2 people" },
            new Availability { Id = 4, Name = "3 rooms for up to 5 people" },
            new Availability { Id = 5, Name = "1 room for up to 10 people" },
            new Availability { Id = 6, Name = "4 rooms for up to 10 people" },
            new Availability { Id = 7, Name = "1 room for up to 20 people" },
            
            // Specific availabilities from the image
            new Availability { Id = 8, Name = "35 desks" },  // WorkClub Pechersk
            new Availability { Id = 9, Name = "4 private rooms" },
            new Availability { Id = 10, Name = "2 meeting rooms" },

            new Availability { Id = 11, Name = "20 desks" },  // UrbanSpace Podil
            new Availability { Id = 12, Name = "2 private rooms" },
            new Availability { Id = 13, Name = "1 meeting room" },

            new Availability { Id = 14, Name = "15 desks" },  // Creative Hub Lvivska
            new Availability { Id = 15, Name = "No private rooms" },

            new Availability { Id = 16, Name = "40 desks" },  // TechNest Olimpiiska
            new Availability { Id = 17, Name = "3 private rooms" },

            new Availability { Id = 18, Name = "25 desks" },  // Hive Station Troieshchyna
            new Availability { Id = 19, Name = "1 private room" }
        };
            context.Set<Availability>().AddRange(availabilities);

            // Save before creating relationships
            context.SaveChanges();

            // Add workspace photos for each workspace
            var workspacePhotos = new List<WorkspacePhotos>();

            // For each coworking
            for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
            {
                int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

                // Open space photos (1-4)
                for (int i = 1; i <= 4; i++)
                    workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId, PhotoId = i });

                // Private room photos (5-8)
                for (int i = 5; i <= 8; i++)
                    workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId + 1, PhotoId = i });

                // Meeting room photos (8-11)
                for (int i = 8; i <= 11; i++)
                    workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId + 2, PhotoId = i });
            }
            context.Set<WorkspacePhotos>().AddRange(workspacePhotos);

            // Add workspace amenities for each workspace
            var workspaceAmenities = new List<WorkspaceAmenitys>();

            // For each coworking
            for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
            {
                int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

                // Open space amenities (WiFi, Games, Coffee)
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 1 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 2 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 3 });

                // Private room amenities (WiFi, AirCondition, Coffee)
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 1 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 4 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 3 });

                // Meeting room amenities (WiFi, AirCondition, Coffee, Microphone)
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 1 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 4 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 3 });
                workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 5 });
            }
            context.Set<WorkspaceAmenitys>().AddRange(workspaceAmenities);

            // Add workspace availabilities
            var workspaceAvailabilities = new List<WorkspaceAvailabilitys>();

            // For each coworking
            for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
            {
                int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

                // Basic workspace availabilities for each workspace type

                // Open space availabilities
                //workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId, AvailabilityId = 1 }); // 24 desks

                // Private room availabilities
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 2 }); // 7 rooms for 1 person
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 3 }); // 4 rooms for up to 2 people
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 4 }); // 3 rooms for up to 5 people
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 5 }); // 1 room for up to 10 people

                // Meeting room availabilities
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 2, AvailabilityId = 6 }); // 4 rooms for up to 10 people
                workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 2, AvailabilityId = 7 }); // 1 room for up to 20 people
            }

            // Add specific availabilities for each coworking
            // WorkClub Pechersk
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 1, AvailabilityId = 8 }); // 35 desks
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 9 }); // 4 private rooms
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 10 }); // 2 meeting rooms

            // UrbanSpace Podil
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 4, AvailabilityId = 11 }); // 20 desks
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 5, AvailabilityId = 12 }); // 2 private rooms
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 6, AvailabilityId = 13 }); // 1 meeting room

            // Creative Hub Lvivska
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 7, AvailabilityId = 14 }); // 15 desks
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 8, AvailabilityId = 15 }); // No private rooms
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 9, AvailabilityId = 13 }); // 1 meeting room

            // TechNest Olimpiiska
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 10, AvailabilityId = 16 }); // 40 desks
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 11, AvailabilityId = 17 }); // 3 private rooms
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 12, AvailabilityId = 10 }); // 2 meeting rooms

            // Hive Station Troieshchyna
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 13, AvailabilityId = 18 }); // 25 desks
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 14, AvailabilityId = 19 }); // 1 private room
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 15, AvailabilityId = 13 }); // 1 meeting room

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

        // Add photos for workspaces and coworkings
        var photos = new List<Photos>();
        // Workspace photos
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

        // Coworking photos
        photos.Add(new Photos { Id = 12, ImageUrl = "https://i.ibb.co/PSGtC9P/Pechersk.jpg" }); // WorkClub Pechersk
        photos.Add(new Photos { Id = 13, ImageUrl = "https://i.ibb.co/8DyFP071/podil.jpg" }); // UrbanSpace Podil 
        photos.Add(new Photos { Id = 14, ImageUrl = "https://i.ibb.co/vvJgR0P9/lvivska.jpg" }); // Creative Hub Lvivska 
        photos.Add(new Photos { Id = 15, ImageUrl = "https://i.ibb.co/hJZjQvJB/olimpiska.jpg" }); // TechNest Olimpiiska 
        photos.Add(new Photos { Id = 16, ImageUrl = "https://i.ibb.co/vxZYXXRP/troya.jpg" }); // Hive Station Troieshchyna 

        await context.Set<Photos>().AddRangeAsync(photos, cancellationToken);

        // Add coworkings
        var coworkings = new List<Coworking>
        {
            new Coworking
            {
                Id = 1,
                Name = "WorkClub Pechersk",
                Description = "Modern coworking in the heart of Pechersk with quiet rooms and coffee on tap.",
                Location = "123 Yaroslaviv Val St, Kyiv",
                PhotoId = 12
            },
            new Coworking
            {
                Id = 2,
                Name = "UrbanSpace Podil",
                Description = "A creative riverside hub ideal for freelancers and small startups.",
                Location = "78 Naberezhno-Khreshchatytska St, Kyiv",
                PhotoId = 13
            },
            new Coworking
            {
                Id = 3,
                Name = "Creative Hub Lvivska",
                Description = "A compact, design-focused space with open desks and strong community vibes.",
                Location = "12 Lvivska Square, Kyiv",
                PhotoId = 14
            },
            new Coworking
            {
                Id = 4,
                Name = "TechNest Olimpiiska",
                Description = "A high-tech space near Olimpiiska metro, perfect for team sprints and solo focus.",
                Location = "45 Velyka Vasylkivska St, Kyiv",
                PhotoId = 15
            },
            new Coworking
            {
                Id = 5,
                Name = "Hive Station Troieshchyna",
                Description = "A quiet, affordable option in the city's northeast—great for remote workers.",
                Location = "102 Zakrevskogo St, Kyiv",
                PhotoId = 16
            }
        };
        await context.Set<Coworking>().AddRangeAsync(coworkings, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // Create workspaces for each coworking
        var workspaces = new List<Workspaces>();

        // For each coworking, create the standard workspace types
        foreach (var coworking in coworkings)
        {
            // Create base workspace ID for this coworking
            int baseId = (coworking.Id - 1) * 3 + 1;

            // Add Open space
            workspaces.Add(new Workspaces
            {
                Id = baseId,
                WorksapceTypeName = "Open space",
                Description = "A vibrant shared area perfect for freelancers or small teams who enjoy a collaborative atmosphere. Choose any available desk and get to work with flexibility and ease.",
                CapacityId = 1,
                CoworkingId = coworking.Id
            });

            // Add Private rooms
            workspaces.Add(new Workspaces
            {
                Id = baseId + 1,
                WorksapceTypeName = "Private rooms",
                Description = "Ideal for focused work, video calls, or small team huddles. These fully enclosed rooms offer privacy and come in a variety of sizes to fit your needs.",
                CapacityId = 2,
                CoworkingId = coworking.Id
            });

            // Add Meeting rooms
            workspaces.Add(new Workspaces
            {
                Id = baseId + 2,
                WorksapceTypeName = "Meeting rooms",
                Description = "Designed for productive meetings, workshops, or client presentations. Equipped with screens, whiteboards, and comfortable seating to keep your sessions running smoothly.",
                CapacityId = 4,
                CoworkingId = coworking.Id
            });
        }
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

        // Add availabilities
        var availabilities = new List<Availability>
        {
            // Generic availabilities
            new Availability { Id = 1, Name = "24 desks" },
            new Availability { Id = 2, Name = "7 rooms for 1 person" },
            new Availability { Id = 3, Name = "4 rooms for up to 2 people" },
            new Availability { Id = 4, Name = "3 rooms for up to 5 people" },
            new Availability { Id = 5, Name = "1 room for up to 10 people" },
            new Availability { Id = 6, Name = "4 rooms for up to 10 people" },
            new Availability { Id = 7, Name = "1 room for up to 20 people" },
            
            // Specific availabilities from the image
            new Availability { Id = 8, Name = "35 desks" },  // WorkClub Pechersk
            new Availability { Id = 9, Name = "4 private rooms" },
            new Availability { Id = 10, Name = "2 meeting rooms" },

            new Availability { Id = 11, Name = "20 desks" },  // UrbanSpace Podil
            new Availability { Id = 12, Name = "2 private rooms" },
            new Availability { Id = 13, Name = "1 meeting room" },

            new Availability { Id = 14, Name = "15 desks" },  // Creative Hub Lvivska
            new Availability { Id = 15, Name = "No private rooms" },

            new Availability { Id = 16, Name = "40 desks" },  // TechNest Olimpiiska
            new Availability { Id = 17, Name = "3 private rooms" },

            new Availability { Id = 18, Name = "25 desks" },  // Hive Station Troieshchyna
            new Availability { Id = 19, Name = "1 private room" }
        };
        await context.Set<Availability>().AddRangeAsync(availabilities, cancellationToken);

        // Save before creating relationships
        await context.SaveChangesAsync(cancellationToken);

        // Add workspace photos for each workspace
        var workspacePhotos = new List<WorkspacePhotos>();

        // For each coworking
        for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
        {
            int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

            // Open space photos (1-4)
            for (int i = 1; i <= 4; i++)
                workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId, PhotoId = i });

            // Private room photos (5-8)
            for (int i = 5; i <= 8; i++)
                workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId + 1, PhotoId = i });

            // Meeting room photos (8-11)
            for (int i = 8; i <= 11; i++)
                workspacePhotos.Add(new WorkspacePhotos { WorkspaceId = baseWorkspaceId + 2, PhotoId = i });
        }
        await context.Set<WorkspacePhotos>().AddRangeAsync(workspacePhotos, cancellationToken);

        // Add workspace amenities for each workspace
        var workspaceAmenities = new List<WorkspaceAmenitys>();

        // For each coworking
        for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
        {
            int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

            // Open space amenities (WiFi, Games, Coffee)
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 1 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 2 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId, AmenityId = 3 });

            // Private room amenities (WiFi, AirCondition, Coffee)
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 1 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 4 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 1, AmenityId = 3 });

            // Meeting room amenities (WiFi, AirCondition, Coffee, Microphone)
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 1 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 4 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 3 });
            workspaceAmenities.Add(new WorkspaceAmenitys { WorkspaceId = baseWorkspaceId + 2, AmenityId = 5 });
        }
        await context.Set<WorkspaceAmenitys>().AddRangeAsync(workspaceAmenities, cancellationToken);

        // Add workspace availabilities
        var workspaceAvailabilities = new List<WorkspaceAvailabilitys>();

        // For each coworking
        for (int coworkingId = 1; coworkingId <= 5; coworkingId++)
        {
            int baseWorkspaceId = (coworkingId - 1) * 3 + 1;

            // Basic workspace availabilities for each workspace type

            // Open space availabilities
            //workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId, AvailabilityId = 1 }); // 24 desks

            // Private room availabilities
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 2 }); // 7 rooms for 1 person
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 3 }); // 4 rooms for up to 2 people
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 4 }); // 3 rooms for up to 5 people
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 1, AvailabilityId = 5 }); // 1 room for up to 10 people

            // Meeting room availabilities
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 2, AvailabilityId = 6 }); // 4 rooms for up to 10 people
            workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = baseWorkspaceId + 2, AvailabilityId = 7 }); // 1 room for up to 20 people
        }

        // Add specific availabilities for each coworking
        // WorkClub Pechersk
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 1, AvailabilityId = 8 }); // 35 desks
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 2, AvailabilityId = 9 }); // 4 private rooms
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 3, AvailabilityId = 10 }); // 2 meeting rooms

        // UrbanSpace Podil
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 4, AvailabilityId = 11 }); // 20 desks
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 5, AvailabilityId = 12 }); // 2 private rooms
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 6, AvailabilityId = 13 }); // 1 meeting room

        // Creative Hub Lvivska
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 7, AvailabilityId = 14 }); // 15 desks
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 8, AvailabilityId = 15 }); // No private rooms
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 9, AvailabilityId = 13 }); // 1 meeting room

        // TechNest Olimpiiska
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 10, AvailabilityId = 16 }); // 40 desks
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 11, AvailabilityId = 17 }); // 3 private rooms
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 12, AvailabilityId = 10 }); // 2 meeting rooms

        // Hive Station Troieshchyna
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 13, AvailabilityId = 18 }); // 25 desks
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 14, AvailabilityId = 19 }); // 1 private room
        workspaceAvailabilities.Add(new WorkspaceAvailabilitys { WorkspaceId = 15, AvailabilityId = 13 }); // 1 meeting room

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