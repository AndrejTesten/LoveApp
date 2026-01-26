namespace LoveApp.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string? Note { get; set; } // nullable
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
