using System;

namespace TutorMatch.Models.DTOs
{
    public class CreateReviewDTO
    {
        public Guid BookingId { get; set; }
        public Guid TutorId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}