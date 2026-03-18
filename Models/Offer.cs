using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorMatch.Models.Enums;

namespace TutorMatch.Models
{
    public class Offer
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OfferId { get; set; }

        [ForeignKey("TutorProfile")]
        public Guid TutorId { get; set; }

        [Required]
        public Subject Subject { get; set; }

        [Required]
        public Level Level { get; set; }

        [Required]
        public List<int> DurationOptions { get; set; }

        [Required]
        public string Description { get; set; }

        public bool isActive { get; set; }
    }
}
