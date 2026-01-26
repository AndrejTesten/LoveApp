using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveApp.Models
{
    public class SentMysteryMessages
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MysteryMessageId { get; set; }

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Optional: navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("MysteryMessageId")]
        public virtual MysteryMessage MysteryMessage { get; set; }
    }
}
