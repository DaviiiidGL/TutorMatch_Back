using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TutorMatch.Models;

namespace TutorMatch.DAO
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        //Constructor que recibe las opciones de conexión a la bd para tener contexto de esta
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //listado de clases -> a tablas en la base de datos
        //Los DB Set nos ayudan a mapear las clases como Entidades
        //Es decir que Entity Framework lee este archivo para tomar del modelo el esquema de las tablas

        public DbSet<TutorProfile> TutorProfiles { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANTE: Siempre llamar a base.OnModelCreating cuando usas Identity
            base.OnModelCreating(builder);

            // Evitar cascada múltiple en Conversations -> TutorProfile
            builder.Entity<Conversation>()
                .HasOne(c => c.TutorProfile) // Asume que tienes una propiedad public TutorProfile TutorProfile {get;set;}
                .WithMany() // O .WithMany(t => t.Conversations) si la colección existe en TutorProfile
                .HasForeignKey(c => c.TutorProfileId)
                .OnDelete(DeleteBehavior.Restrict); // ESTA ES LA MAGIA: Desactiva la cascada

            // Evitar cascada múltiple en Conversations -> User (opcional, pero recomendado aquí)
            builder.Entity<Conversation>()
                .HasOne(c => c.User) // Asume que tienes public IdentityUser User {get;set;}
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Offer) // Asume public Offer Offer {get;set;} en Booking
                .WithMany()
                .HasForeignKey(b => b.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Restringir el borrado desde el Estudiante (User)
            builder.Entity<Booking>()
                .HasOne(b => b.Student) // Asume public IdentityUser Student {get;set;} en Booking
                .WithMany()
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Restringir el borrado desde la Conversación
            builder.Entity<Booking>()
                .HasOne(b => b.Conversation) // Asume public Conversation Conversation {get;set;} en Booking
                .WithMany()
                .HasForeignKey(b => b.ConversationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(c => c.Sender) // Asume que tienes una propiedad public TutorProfile TutorProfile {get;set;}
                .WithMany() // O .WithMany(t => t.Conversations) si la colección existe en TutorProfile
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
