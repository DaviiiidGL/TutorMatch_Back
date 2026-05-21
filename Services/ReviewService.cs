using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Interfaces; // <-- No olvides agregar este using arriba
using TutorMatch.Models;
using TutorMatch.Models.Enums;

namespace TutorMatch.Services
{
    public class ReviewService : IReviewService // <-- Aquí le indicamos la herencia
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // El resto del método CreateReview se queda exactamente igual...
        public async Task<Review> CreateReview(Guid bookingId, Review newReview)
        {
            // ... (todo el código que pusimos antes) ...
            var booking = await _context.Bookings
                .Include(b => b.Offer)
                .ThenInclude(o => o.TutorProfile)
                .ThenInclude(t => t.Reviews)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null) throw new Exception("Reserva no encontrada.");
            if (booking.Status != Status.Completed) throw new Exception("Solo puedes calificar sesiones completadas.");
            if (booking.ReviewId != null) throw new Exception("Esta reserva ya tiene una reseña.");

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            booking.ReviewId = newReview.ReviewId;

            var tutor = booking.Offer.TutorProfile;
            tutor.Reviews.Add(newReview);

            double totalStars = tutor.Reviews.Sum(r => r.ReviewCount);
            tutor.AverageRating = totalStars / tutor.Reviews.Count;

            await _context.SaveChangesAsync();

            return newReview;
        }
    }
}