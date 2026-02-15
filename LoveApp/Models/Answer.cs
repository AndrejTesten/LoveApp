using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveApp.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // could be your auth user id

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
