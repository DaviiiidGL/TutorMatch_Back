using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorMatch.Models
{
    public class Message
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MessageId { get; set; }

        [Required]
        public Guid ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversation? Conversation { get; set; }

        [Required]
        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public IdentityUser? Sender { get; set; }

        [Required][MaxLength(100)]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
