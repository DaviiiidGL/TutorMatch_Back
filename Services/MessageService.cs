using Microsoft.EntityFrameworkCore;
using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Models;
using TutorMatch.Models.Enums;

namespace TutorMatch.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message> SendMessage(Guid bookingId, Message newMessage)
        {
            // 1. Buscamos la reserva
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) throw new Exception("Reserva no encontrada.");

            // 2. Validamos la regla de negocio: Solo reservas Aceptadas
            if (booking.Status != Status.Aceptada)
                throw new Exception("El chat solo está habilitado para sesiones Aceptadas.");

            // 3. Si es el primer mensaje, creamos la conversación
            if (booking.ConversationId == null)
            {
                var newConversation = new Conversation { ConversationId = Guid.NewGuid() };
                _context.Conversations.Add(newConversation);
                booking.ConversationId = newConversation.ConversationId;
            }

            // 4. Guardamos el mensaje
            newMessage.ConversationId = booking.ConversationId.Value;
            newMessage.SentAt = DateTime.UtcNow; // Fecha y hora actual

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            return newMessage;
        }

        public async Task<List<Message>> GetMessagesByBooking(Guid bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.ConversationId == null) return new List<Message>();

            // Traemos todos los mensajes de esa conversación ordenados por fecha
            return await _context.Messages
                .Where(m => m.ConversationId == booking.ConversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}