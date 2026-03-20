using System;

using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface IOfferService
    {
        Task<List<Offer>> GetAll();
        Task<Offer?> getById(Guid id);
        Task<List<Offer>> listByTutor(Guid id);
        Task<Offer> Create(Offer offer);
        Task<bool> Update(Guid id, Offer offer);
        Task<bool> ChangeStatus(Guid id);
    }
}