using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Models;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("booking/{bookingId}")]
        [Authorize(Roles = "Student")] // Solo los estudiantes pueden dejar reseñas
        public async Task<IActionResult> CreateReview(Guid bookingId, [FromBody] Review newReview)
        {
            try
            {
                var createdReview = await _reviewService.CreateReview(bookingId, newReview);
                return Ok(createdReview);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}