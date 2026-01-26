namespace LoveApp.Models
{
    public class MemoryPin
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<MemoryImage> Images { get; set; } = new();
    }
}
