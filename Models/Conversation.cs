using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorMatch.Models
{
    public class Conversation
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ConversationId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        public Guid TutorProfileId { get; set; }

        [ForeignKey("TutorProfileId")]
        public TutorProfile TutorProfile { get; set; }
        public List<Message> Messages { get; set; }
    }
}
