using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Interfaces;
using TutorMatch.Models;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere estar logueado para usar el chat
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("booking/{bookingId}")]
        public async Task<IActionResult> SendMessage(Guid bookingId, [FromBody] Message newMessage)
        {
            try
            {
                var message = await _messageService.SendMessage(bookingId, newMessage);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetMessages(Guid bookingId)
        {
            var messages = await _messageService.GetMessagesByBooking(bookingId);
            return Ok(messages);
        }
    }
}