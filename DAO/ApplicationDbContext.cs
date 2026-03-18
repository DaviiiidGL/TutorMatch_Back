using TutorMatch_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TutorMatch_Back.DAO
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        //Constructor que recibe las opciones de conexión a la bd para tener contexto de esta
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //listado de clases -> a tablas en la base de datos
        //Los DB Set nos ayudan a mapear las clases como Entidades
        //Es decir que Entity Framework lee este archivo para tomar del modelo el esquema de las tablas

        public DbSet<Offers> Offer { get; set; }
        public DbSet<TutorProfiles> TutorProfile { get; set; }
        public DbSet<Availability> Availability { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<IdentityUser> Users { get; set; }

    }
}