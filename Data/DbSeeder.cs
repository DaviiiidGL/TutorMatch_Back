using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Models;
using TutorMatch.Models.Enums;

namespace TutorMatch.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Crear Roles si no existen
            string[] roles = { "Student", "Tutor", "Admin" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Si ya hay usuarios, significa que ya se sembró antes, entonces nos salimos
            if (await userManager.Users.AnyAsync()) return;

            // 2. Crear 10 Usuarios (Tutores)
            var tutoresId = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                var user = new IdentityUser { UserName = $"tutor{i}@test.com", Email = $"tutor{i}@test.com" };
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, "Tutor");
                tutoresId.Add(user.Id);

                // Crear su Perfil de Tutor (Usando TutorId y UserId correctos)
                context.TutorProfiles.Add(new TutorProfile
                {
                    TutorId = Guid.NewGuid(), // Corregido
                    UserId = user.Id,         // Corregido
                    Bio = $"Hola, soy el tutor {i} y me encanta enseñar. Tengo mucha paciencia y pedagogía.",
                    HourlyRate = 20000 + (i * 5000),
                    IsVirtual = (i % 2 == 0),
                    AverageRating = 0,
                    CiudadPais = "Medellín, Colombia", // Agregado para que no quede nulo
                    isActive = true
                });
            }
            await context.SaveChangesAsync();

            // 3. Crear 10 Usuarios (Estudiantes)
            var estudiantesId = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                var user = new IdentityUser { UserName = $"student{i}@test.com", Email = $"student{i}@test.com" };
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, "Student");
                estudiantesId.Add(user.Id);
            }

            // 4. Crear 30 Ofertas
            var tutorProfiles = await context.TutorProfiles.ToListAsync();
            var random = new Random();
            var subjects = Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();
            var levels = Enum.GetValues(typeof(Level)).Cast<Level>().ToList();

            foreach (var tutor in tutorProfiles)
            {
                for (int j = 0; j < 3; j++) // 3 ofertas por tutor = 30
                {
                    context.Offers.Add(new Offer
                    {
                        OfferId = Guid.NewGuid(),
                        TutorProfileId = tutor.TutorId, // Corregido
                        Subject = subjects[random.Next(subjects.Count)],
                        Level = levels[random.Next(levels.Count)],
                        DurationOptions = new List<int> { 60, 90, 120 }, // Agregado porque tu modelo lo requiere
                        Description = "Clases personalizadas adaptadas a tu ritmo.",
                        isActive = true
                    });
                }
            }
            await context.SaveChangesAsync();

            // 5. Crear 40 Reservas (Bookings), 30 Mensajes y 15 Reseñas
            // Agregamos .Include(o => o.TutorProfile) para que no sea nulo al actualizar el rating
            var offers = await context.Offers.Include(o => o.TutorProfile).ToListAsync();

            for (int k = 1; k <= 40; k++)
            {
                var offer = offers[random.Next(offers.Count)];

                var status = (k <= 20) ? Status.Accepted : Status.Pending;
                if (k <= 15) status = Status.Completed;

                var booking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    OfferId = offer.OfferId,
                    StudentId = estudiantesId[random.Next(estudiantesId.Count)],
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(k)),
                    StarTime = new TimeOnly(14, 0),
                    EndTime = new TimeOnly(16, 0),
                    Status = status,
                    Price = 35000,
                    isActive = true
                };

                // Crear 15 Reseñas para las reservas completadas
                if (status == Status.Completed)
                {
                    var review = new Review
                    {
                        ReviewId = Guid.NewGuid(),
                        BookingId = booking.BookingId,            // ¡Llenamos el nuevo campo!
                        TutorId = offer.TutorProfileId,           // Sacamos el ID directamente de la oferta
                        StudentId = booking.StudentId,            // ¡Llenamos el nuevo campo!
                        StudentName = "Estudiante de Prueba",     // ¡Llenamos el nuevo campo!
                        Comment = "Excelente tutor, aprendí muchísimo en la clase.",
                        Rating = random.Next(4, 6),
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Reviews.Add(review);
                    booking.ReviewId = review.ReviewId;

                    // Actualizamos las estadísticas del tutor en el seeder
                    if (offer.TutorProfile != null)
                    {
                        offer.TutorProfile.AverageRating = review.Rating;
                        offer.TutorProfile.ReviewCount = 1;
                    }
                }

                // Crear Conversaciones y 30 Mensajes para reservas Aceptadas/Completadas
                if ((status == Status.Accepted || status == Status.Completed) && k <= 30)
                {
                    var conversation = new Conversation { ConversationId = Guid.NewGuid() };
                    context.Conversations.Add(conversation);
                    booking.ConversationId = conversation.ConversationId;

                    context.Messages.Add(new Message
                    {
                        MessageId = Guid.NewGuid(),
                        ConversationId = conversation.ConversationId,
                        SenderId = booking.StudentId,
                        Content = "Hola, confirmo nuestra clase para mañana.",
                        SentAt = DateTime.UtcNow
                    });
                }

                context.Bookings.Add(booking);
            }

            await context.SaveChangesAsync();
        }
    }
}