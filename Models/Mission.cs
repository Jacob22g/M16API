namespace M16API.Models
{
    public class Mission
    {
        public string Agent { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public Coordinates? Coordinates { get; set; }
        public DateTime Date { get; set; }
        public Mission(string agent, string country, string address, Coordinates? coordinates, DateTime date)
        {
            Agent = agent;
            Country = country;
            Address = address;
            Coordinates = coordinates;
            Date = date;
        }
    }

    public class Coordinates
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public Coordinates(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }
    }
}
