using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutorMatch.Models;
using TutorMatch.Models.DTOs;

namespace TutorMatch.Interfaces
{
    public interface IReviewService
    {
        Task<Review> CreateReview(CreateReviewDTO dto, string studentId, string studentName);
        Task<Review?> GetReviewByBooking(Guid bookingId);
        Task<List<Review>> GetReviewsByTutor(Guid tutorId);
    }
}