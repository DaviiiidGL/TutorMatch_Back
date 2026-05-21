using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorMatch.Models.Enums;

namespace TutorMatch.Models
{
    public class Booking
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BookingId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public IdentityUser? Student { get; set; }

        [Required]
        public Guid OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer? Offer { get; set; }

        [Required]
        public TimeOnly StarTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Pending;

        public double Price { get; set; }
        public Guid? ReviewId { get; set; }

        [ForeignKey("ReviewId")]
        public Review? Review { get; set; }
        public Guid? ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversation? Conversation { get; set; }

        [Required]
        public bool isActive { get; set; } = true;
    }
}
