using System;
using System.Threading.Tasks;
using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface IReviewService
    {
        Task<Review> CreateReview(Guid bookingId, Review newReview);
    }
}