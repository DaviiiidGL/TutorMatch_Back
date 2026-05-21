using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Models.Enums;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // <-- ¡Plural!
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ApplicationDbContext _context; // Lo inyectamos para consultas rápidas

        public BookingController(IBookingService bookingService, ApplicationDbContext context)
        {
            _bookingService = bookingService;
            _context = context;
        }

        // --- ENDPOINTS DEL ESTUDIANTE ---

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyBookings([FromQuery] string? status)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _context.Bookings.Include(b => b.Offer).Where(b => b.StudentId == studentId);

            if (!string.IsNullOrEmpty(status))
            {
                // Convierte "accepted" a Status.Accepted
                if (Enum.TryParse<Status>(status, true, out var parsedStatus))
                {
                    query = query.Where(b => b.Status == parsedStatus);
                }
            }

            return Ok(await query.ToListAsync());
        }

        // --- ENDPOINTS DEL TUTOR ---

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Busca las reservas donde el perfil del tutor pertenece a este usuario
            var bookings = await _context.Bookings
                .Include(b => b.Offer).ThenInclude(o => o.TutorProfile)
                .Where(b => b.Offer.TutorProfile.UserId == userId && b.Status == Status.Pending)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("accepted")]
        public async Task<IActionResult> GetTutorAcceptedBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookings = await _context.Bookings
                .Include(b => b.Offer).ThenInclude(o => o.TutorProfile)
                .Where(b => b.Offer.TutorProfile.UserId == userId && b.Status == Status.Accepted)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpPatch("{id}/accept")]
        public async Task<IActionResult> AcceptBooking(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = Status.Accepted;
            await _context.SaveChangesAsync();
            return Ok(booking);
        }

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectBooking(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = Status.Rejected;
            await _context.SaveChangesAsync();
            return Ok(booking);
        }

        // --- ENDPOINT GENERAL PARA CREAR (El que ya tenías pero adaptado) ---
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking newBooking)
        {
            try
            {
                newBooking.StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? newBooking.StudentId;
                var created = await _bookingService.Create(newBooking);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}