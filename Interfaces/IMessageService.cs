using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface IMessageService
    {
        Task<Message> SendMessage(Guid bookingId, Message newMessage);
        Task<List<Message>> GetMessagesByBooking(Guid bookingId);
    }
}
