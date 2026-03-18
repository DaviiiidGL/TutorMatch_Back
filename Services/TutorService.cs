using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.DAO;
using Microsoft.EntityFrameworkCore;
namespace TutorMatch.Services
{
    public class TutorService : ITutorService
    {


        private readonly ApplicationDbContext _context;

        public TutorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TutorProfile>> GetAll()
        {
            return await _context.TutorProfiles.Where(e => e.isActive == true).ToListAsync();
        }

        //public async Task<List<TutorProfile>> listBySubject(String subject)
        //{
         //   return await _context.TutorProfiles.Where(e => e.isActive == true && e. == subject).ToListAsync();
        //}

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

            tutorExiste.Availability = editedTutor.Availability;
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
