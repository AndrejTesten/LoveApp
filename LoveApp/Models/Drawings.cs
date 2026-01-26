using System;
using System.ComponentModel.DataAnnotations;

namespace LoveApp.Models
{
    public class Drawing
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }   // Optional: associate with a user

        [Required]
        public string DrawingData { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
