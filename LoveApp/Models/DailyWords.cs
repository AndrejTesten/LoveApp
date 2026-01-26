namespace LoveApp.Models
{
    public class DailyWords
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public int SenderId { get; set; }  // 1=Andrej, 2=Laila
        public int ReceiverId { get; set; }
        public bool Learned { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class DailyWordDto
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool Learned { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }



}
