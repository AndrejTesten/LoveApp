namespace LoveApp.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public List<TripPoint> Route { get; set; } = new();
    }

    public class TripPoint
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int PointOrder { get; set; }
    }

}
