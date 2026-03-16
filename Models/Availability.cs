using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorMatch.Models
{
    public class Availability
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TutorId { get; set; }

        [ForeignKey("TutorProfile")]
        public Guid TutorProfileId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly StarTime {  get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public bool IsRecurring { get; set; }

    }
}
