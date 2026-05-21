using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Models.Enums;
namespace TutorMatch.Services
{
    public class TutorProfileService : ITutorProfileService
    {
        private readonly ApplicationDbContext _context;

        public TutorProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TutorProfile>> GetAll()
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true).ToListAsync();
        }

        public async Task<List<TutorProfile>> listBySubject(Subject subject)
        {
        return await _context.TutorProfiles.Include(t => t.Offers).Where(e => e.isActive == true && e.Offers.Any(p => p.Subject.Equals(subject) == true)).ToListAsync();

        }
        public async Task<List<TutorProfile>> listByModality(bool isVirtual)
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true && e.IsVirtual==isVirtual).ToListAsync();
        }
        public async Task<List<TutorProfile>> listByPriceRange(double inicialPrice, double finalPrice)
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true && e.HourlyRate >= inicialPrice && e.HourlyRate <= finalPrice).ToListAsync();
        }

        public async Task<List<TutorProfile>> listByMinimumRating(double minRating)
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true && e.AverageRating>=minRating).ToListAsync();
        }

        public async Task<List<TutorProfile>> listByAvailability(DayOfWeek dayOfWeek, TimeOnly StarTime, TimeOnly EndTime)
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true && e.Availabilities.Any(p => p.DayOfWeek.Equals(dayOfWeek) == true && p.StarTime<= StarTime && p.EndTime >= EndTime)).ToListAsync();
        }

        public async Task<TutorProfile> Create(TutorProfile newTutor)
            {
                //Agregamos el registro a la lista
                _context.TutorProfiles.Add(newTutor);
                await _context.SaveChangesAsync();
                return newTutor;
            }

        public async Task<bool> Update(Guid id, TutorProfile editedTutor)
        {
            //validar la existencia de un ente supremo
            var tutorExiste = await getById(id);
            if (tutorExiste == null) return false;

            tutorExiste.Availabilities = editedTutor.Availabilities;
            tutorExiste.Bio = editedTutor.Bio;
            tutorExiste.IsVirtual = editedTutor.IsVirtual;
            tutorExiste.HourlyRate = editedTutor.HourlyRate;
            tutorExiste.CiudadPais = editedTutor.CiudadPais;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatus(Guid id)
        {
            // Verificamos si existe o no el registro
            var existe = await getById(id);
            if (existe == null) return false;

            existe.isActive = existe.isActive == true ? false : true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TutorProfile> getById(Guid id) => await _context.TutorProfiles.FindAsync(id);
    }
}