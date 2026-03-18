using System;

using TutorMatch_Back.Models;

namespace TutorMatch_Back.Interfaces
{
    public interface IOfferService
    {
        Task<List<Offer>> GetAll();
        Task<List<Offer>> listByTutor(Guid id);


        Task<Offer> Create(Offer offer);

        Task<bool> Update(Guid id, Offer offer);

        Task<bool> ChangeStatud(Guid id);
    }
}