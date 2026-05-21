using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TutorMatch.Interfaces;
using TutorMatch.Models.DTOs;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/reviews")] // <-- ¡Plural para coincidir con el front!
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] CreateReviewDTO dto)
        {
            try
            {
                // Obtenemos el ID y Nombre del estudiante logueado desde su Token JWT
                var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var studentName = User.Identity?.Name ?? "Estudiante";

                var createdReview = await _reviewService.CreateReview(dto, studentId, studentName);
                return Ok(createdReview);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("booking/{bookingId}")]
        [AllowAnonymous] // Para que cualquiera pueda ver las reseñas
        public async Task<IActionResult> GetReviewForBooking(Guid bookingId)
        {
            var review = await _reviewService.GetReviewByBooking(bookingId);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet("tutor/{tutorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsForTutor(Guid tutorId)
        {
            var reviews = await _reviewService.GetReviewsByTutor(tutorId);
            return Ok(reviews);
        }
    }
}