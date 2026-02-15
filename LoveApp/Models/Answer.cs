using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveApp.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; } // just store the Question ID

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
