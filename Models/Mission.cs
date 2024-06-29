namespace M16API.Models
{
    public class Mission
    {
        public Guid Id { get; set; }
        public string Agent { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
    }
    
    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
    
    public class CountryIsolationDegree
    {
        public string Country { get; set; }
        public int IsolationDegree { get; set; }
    }
}
