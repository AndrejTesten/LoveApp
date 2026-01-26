namespace LoveApp.Models
{
    public class MysteryMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
