using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorMatch.Models
{
    public class TutorProfile
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TutorId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser UserProfile { get; set; }

        public List<Availability> Availability { get; set; }

        [Required][MinLength(10)][MaxLength(10)]
        public string Bio { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Offer> Offers { get; set; }

        [Required]
        public bool IsVirtual { get; set; }

        public double HourlyRate { get; set; }

        public double AverageRating { get; set; }

        public string CiudadPais { get; set; }

        public bool isActive { get; set; }
    }
}
