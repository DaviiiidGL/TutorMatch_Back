using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorMatch.Models.Enums;

namespace TutorMatch.Models
{
    public class Booking
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BookingId { get; set; }

        [ForeignKey("StudentProfile")]
        public Guid UserId { get; set; }

        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        [Required]
        public TimeOnly StarTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public Status Status { get; set; }

        public double Price { get; set; }

        public Review? Review { get; set; }

        public Conversation Conversation { get; set; }
    }
}
