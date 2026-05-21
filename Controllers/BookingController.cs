using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _bookingService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(Guid id)
        {
            var offer = await _bookingService.getById(id);
            //Se refactoriza condicion por una operación ternaria o si corto
            return offer != null ? Ok(offer) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([FromBody] Booking newBooking)
        {

            var createdBooking = await _bookingService.Create(newBooking);
            return CreatedAtAction(nameof(getById), new { id = createdBooking.BookingId }, createdBooking);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Booking editedBooking)
        {

            return await _bookingService.Update(id, editedBooking) ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/change-accept")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Accept(Guid id)
        {
            return await _bookingService.Accept(id) ? Ok("Se ha aceptado la reserva") : NotFound();
        }

        [HttpPatch("{id}/change-decline")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Decline(Guid id)
        {
            return await _bookingService.Decline(id) ? Ok("Se ha rechazado la reserva") : NotFound();
        }
    }
}
