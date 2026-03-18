using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAll();
        Task<Booking?> getById(Guid id);
        Task<List<Booking>> listByTutor(Guid id);
        Task<bool> isReserved(Guid id, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd);
        Task<Booking> Create(Booking Booking);
        Task<bool> Update(Guid id, Booking booking);
        Task<bool> Accept(Guid id);

        Task<bool> Decline(Guid id);
    }
}