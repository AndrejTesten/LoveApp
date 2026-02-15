using System.ComponentModel.DataAnnotations;

namespace LoveApp.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
