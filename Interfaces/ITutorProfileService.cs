using System;

using TutorMatch.Models;
using TutorMatch.Models.Enums;

namespace TutorMatch.Interfaces
{
    public interface ITutorProfileService
    {
        Task<List<TutorProfile>> GetAll();
        Task<List<TutorProfile>> listBySubject(Subject subject);
        Task<List<TutorProfile>> listByModality(bool isVirtual);
        Task<List<TutorProfile>> listByPriceRange(double inicialPrice, double finalPrice);
        Task<List<TutorProfile>> listByAvailability(DayOfWeek dayOfWeek, TimeOnly StarTime, TimeOnly EndTime);
        Task<List<TutorProfile>> listByMinimumRating(double minRating);

        Task<TutorProfile?> getById(Guid id);

        Task<TutorProfile> Create(TutorProfile tutor);

        Task<bool> Update(Guid id, TutorProfile tutorProfile);
        Task<bool> ChangeStatus(Guid id);
    }
}