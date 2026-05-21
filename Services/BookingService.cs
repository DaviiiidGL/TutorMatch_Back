using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Models.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TutorMatch.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAll()
        {
            return await _context.Bookings.Where(e => e.isActive == true).ToListAsync();
        }

        public async Task<List<Booking>> listByTutor(Guid id)
        {
            return await _context.Bookings.Include(e => e.Offer).Where(e => e.isActive && e.Offer.TutorProfileId == id).ToListAsync();
        }

        public async Task<Booking> Create(Booking newBooking)
        {
            //Agregamos el registro a la lista
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
            return newBooking;
        }

        public async Task<bool> Update(Guid id, Booking editedBooking)
        {
            //validar la existencia de un ente supremo
            var bookingExiste = await getById(id);
            if (bookingExiste == null) return false;

            bookingExiste.StarTime= editedBooking.StarTime;
            bookingExiste.EndTime = editedBooking.EndTime;
            bookingExiste.Date = editedBooking.Date;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Accept(Guid id)
        {
            // Verificamos si existe o no el registro
            var existe = await getById(id);
            if (existe == null) return false;
            if (existe.isActive == false) return false;

            existe.Status = (Status)1;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Decline(Guid id)
        {
            // Verificamos si existe o no el registro
            var existe = await getById(id);
            if (existe == null) return false;
            if (existe.isActive == false) return false;

            existe.Status = (Status)2;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> isReserved(Guid id, DateOnly date, TimeOnly horaInicio, TimeOnly horaFinal)
        {

            return await _context.Bookings.Include(p=>p.Offer).Where(p => p.isActive && p.Offer.TutorProfileId == id && p.Date == date && (
        (p.StarTime <= horaInicio && p.EndTime > horaInicio) ||
        (p.StarTime < horaFinal && p.EndTime >= horaFinal) ||
        (p.StarTime >= horaInicio && p.EndTime <= horaFinal)
        )).AnyAsync();
        }

        public async Task<Booking> getById(Guid id) => await _context.Bookings.FindAsync(id);
    }
}
