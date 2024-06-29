using System.Text.RegularExpressions;
using M16API.Models;

namespace M16API.Utils
{
    public static class MissionUtils
    {
    
        public static double GetDistance(Coordinates origin, Coordinates destination)
        {
            return HaversineDistance(origin, destination);
        }
        
        public static bool IsCoordinates(string input)
        {
            var coordinateRegex = new Regex(@"^-?\d+(\.\d+)?,-?\d+(\.\d+)?$");
            return coordinateRegex.IsMatch(input);
        }
        
        public static Coordinates GetCoordinatesFromString(string coordinates)
        {
            var parts = coordinates.Split(',');
            if (parts.Length != 2 ||
                !double.TryParse(parts[0], out var lat) ||
                !double.TryParse(parts[1], out var lng))
            {
                return null;
            }
            return new Coordinates { Lat = lat, Lon = lng };
        }
        
        private static double HaversineDistance(Coordinates coord1, Coordinates coord2)
        {
            const double R = 6371; // Earth's radius in km

            double lat1 = DegreesToRadians(coord1.Lat);
            double lon1 = DegreesToRadians(coord1.Lon);
            double lat2 = DegreesToRadians(coord2.Lat);
            double lon2 = DegreesToRadians(coord2.Lon);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = R * c;

            return distance;
        }
        
        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }    
}
