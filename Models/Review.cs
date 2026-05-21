using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorMatch.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReviewId { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required]
        public Guid TutorId { get; set; }

        [Required]
        public string StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Antes se llamaba ReviewCount

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}