using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;

namespace TutorMatch.Services
{
    public class OfferService : IOfferService
    {


        private readonly ApplicationDbContext _context;

        public OfferService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Offer>> GetAll()
        {
            return await _context.Offers.Where(e => e.isActive == true).ToListAsync();
        }

        public async Task<List<Offer>> listByTutor(Guid id)
        {
            return await _context.Offers.Where(e => e.isActive && e.TutorProfileId == id).ToListAsync();
        }


        public async Task<Offer> Create(Offer newOffer)
        {
            int tutorExist = _context.TutorProfiles.Where(e => e.TutorId == newOffer.TutorProfileId).Count();
            if (tutorExist == 0) throw new Exception("Event not found");

            //Agregamos el registro a la lista
            _context.Offers.Add(newOffer);
            await _context.SaveChangesAsync();
            return newOffer;
        }
        
        public async Task<bool> Update(Guid id, Offer editedOffer)
        {
            //validar la existencia de un ente supremo
            var offerExiste = await getById(id);
            if (offerExiste == null) return false;

            offerExiste.DurationOptions = editedOffer.DurationOptions;
            offerExiste.Description = editedOffer.Description;
            offerExiste.Subject = editedOffer.Subject;

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

        public async Task<Offer> getById(Guid id) => await _context.Offers.FindAsync(id);
    }
}