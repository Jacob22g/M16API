using System.Text.RegularExpressions;
using M16API.Models;

namespace M16API.Utils
{
    public static class DistanceUtils
    {
        private static readonly Regex coordinateRegex = new Regex(@"^-?\d+(\.\d+)?,-?\d+(\.\d+)?$");
    
        public static double GetDistance(Coordinates origin, Coordinates destination)
        {
            return HaversineDistance(origin, destination);
        }
        
        // public static (Coordinates coordinates) ParseCoordinates(string input)
        // {
        //     // Define the regex pattern
        //     string pattern = @"^(?<lat>-?\d+(\.\d+)?),\s*(?<lng>-?\d+(\.\d+)?)$";
        //     // Create a regex object
        //     Regex regex = new Regex(pattern);
        //
        //     // Match the input string against the pattern
        //     Match match = regex.Match(input);
        //
        //     // Check if the match was successful
        //     if (match.Success)
        //     {
        //         // Extract the latitude and longitude from the match groups
        //         double lat = double.Parse(match.Groups["lat"].Value);
        //         double lng = double.Parse(match.Groups["lng"].Value);
        //
        //         // Return the coordinates as a tuple
        //         return new Coordinates(lat, lng);
        //     }
        //     else
        //     {
        //         // Return null if the input string does not match the pattern
        //         return null;
        //     }
        // }
        
        public static bool IsCoordinates(string input)
        {
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

        /**
         * Latitude : max/min 90.0000000 to -90.0000000
         * Longitude : max/min 180.0000000 to -180.0000000
         */
        public static Coordinates GenerateRandomCoordinates()
        {
            var random = new Random();
            double latitude = random.NextDouble() * 180 - 90;
            double longitude = random.NextDouble() * 360 - 180;
            return new Coordinates
            {
                Lat = latitude,
                Lon = longitude
            };
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
