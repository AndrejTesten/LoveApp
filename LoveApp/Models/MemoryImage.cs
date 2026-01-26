namespace LoveApp.Models
{
    public class MemoryImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
        public int MemoryPinId { get; set; }
        public MemoryPin MemoryPin { get; set; } = null!;
    }
}
