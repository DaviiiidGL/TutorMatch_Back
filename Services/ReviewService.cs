using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Models.DTOs;
using TutorMatch.Models.Enums;

namespace TutorMatch.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReview(CreateReviewDTO dto, string studentId, string studentName)
        {
            var booking = await _context.Bookings
                .Include(b => b.Offer)
                .ThenInclude(o => o.TutorProfile)
                .ThenInclude(t => t.Reviews)
                .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);

            if (booking == null) throw new Exception("Reserva no encontrada.");
            if (booking.Status != Status.Completed) throw new Exception("Solo puedes calificar sesiones completadas.");
            if (booking.ReviewId != null && booking.ReviewId != Guid.Empty) throw new Exception("Esta reserva ya tiene una reseña.");

            var newReview = new Review
            {
                ReviewId = Guid.NewGuid(),
                BookingId = dto.BookingId,
                TutorId = dto.TutorId,
                StudentId = studentId,
                StudentName = studentName,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            var tutor = booking.Offer.TutorProfile;

            booking.ReviewId = newReview.ReviewId;
            newReview.BookingId = booking.BookingId;
            newReview.TutorId = tutor.TutorId;
            newReview.StudentId = booking.StudentId;

            tutor.Reviews.Add(newReview);

            double totalStars = tutor.Reviews.Sum(r => r.Rating);
            tutor.AverageRating = totalStars / tutor.Reviews.Count;
            tutor.ReviewCount = tutor.Reviews.Count; 

            await _context.SaveChangesAsync();
            return newReview;
        }

        public async Task<Review?> GetReviewByBooking(Guid bookingId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.BookingId == bookingId);
        }

        public async Task<List<Review>> GetReviewsByTutor(Guid tutorId)
        {
            return await _context.Reviews.Where(r => r.TutorId == tutorId).ToListAsync();
        }
    }
}