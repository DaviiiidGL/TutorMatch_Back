using System;

using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface ITutorService
    {
        Task<List<TutorProfile>> GetAll();
       //* Task<List<TutorProfile>> listBySubject(String subject);
        //Task<List<TutorProfile>> listByModality(bool isVirtual);
       // Task<List<TutorProfile>> listByPriceRange(double inicialPrice, double finalPrice);
        //Task<List<TutorProfile>> listByAvailability(DayOfWeek dayOfWeek, TimeOnly StarTime, TimeOnly EndTime);
        //Task<List<TutorProfile>> listByMinimumRating(double minRating);
        
        Task<TutorProfile?> getById(Guid id);

        Task<TutorProfile> Create(TutorProfile tutor);

        Task<bool> Update(Guid id, TutorProfile tutorProfile);
        Task<bool> ChangeStatus(Guid id);
    }
}
