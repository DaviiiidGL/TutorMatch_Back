namespace TutorMatch_Back.Services
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
            return await _context.Offers.Where(e => e.isActive == 1).ToListAsync();
        }

        public async Task<List<Offer>> listByTutor(Guid id)
        {
            return await _context.Offers.Where(e => e.isActive == 1 && e.TutorId==id).ToListAsync();
        }


        public async Task<Offer> Create(Offer newOffer)
        {
            //Agregamos el registro a la lista
            _context.Offerts.Add(newOffer);
            await _context.SaveChangesAsync();
            return newOffer;
        }

        public async Task<bool> Update(Guid id, Offer editedOffer)
        {
            //validar la existencia de un ente supremo
            var offerExiste = await getById(id);
            if (eventoExiste == null) return false;

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

            existe.isActive = existe.isActive == 1 ? 0 : 1;

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
