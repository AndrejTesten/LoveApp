using System.ComponentModel.DataAnnotations;

namespace LoveApp.Models
{
    public class ActiveQuestion
    {
        [Key]
        public int Id { get; set; } // just primary key
        public int QuestionId { get; set; } // points to Questions table
    }
}
