using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using M16API.Data;
using M16API.Models;
using M16API.Utils;
using Microsoft.EntityFrameworkCore;

namespace M16API.Services
{
    public class MissionService : IMissionService
    {
        private static string _googleMapsApiKey;
        private readonly ApplicationDbContext _context;
        private static Cache<string, Coordinates> _coordinatesCache;

        public MissionService(ApplicationDbContext context, IConfiguration configuration, Cache<string, Coordinates> cache)
        {
            _googleMapsApiKey = configuration["GoogleMapsApiKey"];
            _context = context;
            _coordinatesCache = cache;
        }

        public async Task<List<Mission>> GetMissions()
        {
            return await _context.Missions.ToListAsync();
        }

        public async Task<CountryIsolationDegree> GetCountriesByIsolation()
        {
            try
            {
                var isolatedAgents = await _context.Missions
                    .GroupBy(m => m.Agent)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key)
                    .ToListAsync();

                var result = await _context.Missions
                    .Where(m => isolatedAgents.Contains(m.Agent))
                    .GroupBy(m => m.Country)
                    .Select(g => new CountryIsolationDegree
                    {
                        Country = g.Key,
                        IsolationDegree = g.Count()
                    })
                    .OrderByDescending(g => g.IsolationDegree)
                    .FirstOrDefaultAsync();
                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<Mission> CreateMission(Mission mission)
        {
            var coordinates = await GetCoordinatesFromAddress(mission.Address);
            mission.Id = Guid.NewGuid();
            mission.Longitude = coordinates.Lon;
            mission.Latitude = coordinates.Lat;
            
            _coordinatesCache.Add(mission.Address, coordinates);
            _context.Missions.Add(mission);
            await _context.SaveChangesAsync();
            return mission;
        }

        public async Task<FindClosestResponse> GetClosestMission(string targetLocation)
        {
            Mission closestMission = null;
            double closestDistance = double.MaxValue;
            Coordinates targetCoordinates;
            if (DistanceUtils.IsCoordinates(targetLocation))
            {
                targetCoordinates = DistanceUtils.GetCoordinatesFromString(targetLocation);
            }
            else
            {
                targetCoordinates = await GetCoordinatesFromAddress(targetLocation);
            }
            
            if (targetCoordinates == null)
            {
                return null;
            }
            
            var missions = await _context.Missions.ToListAsync();
            foreach (var mission in missions)
            {
                var missionCoordinates = new Coordinates { Lat = mission.Latitude, Lon = mission.Longitude };
                var distance = DistanceUtils.GetDistance(targetCoordinates, missionCoordinates);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMission = mission;
                }
            }
        
            if (closestMission == null)
            {
                return null;
            }
        
            var response = new FindClosestResponse
            {
                ClosestMission = closestMission,
                Distance = closestDistance
            };
            return response;
        }
        
        private static async Task<Coordinates> GetCoordinatesFromAddress(string address)
        {
            try
            {
                if (_coordinatesCache.ContainsKey(address))
                {
                    return _coordinatesCache.GetValueOrDefault(address, null);
                }

                Coordinates coordinates;
                
                // without a valid api key: 
                if (_googleMapsApiKey == "<YOUR_GOOGLE_MAPS_API_KEY>")
                {
                    // simulate the coordinates:
                    coordinates = DistanceUtils.GenerateRandomCoordinates();
                }
                else
                {
                    var request = new AddressGeocodeRequest
                    {
                        Key = _googleMapsApiKey,
                        Address = address
                    };
                    var response = await GoogleMaps.Geocode.AddressGeocode.QueryAsync(request);
                    var result = response?.Results.FirstOrDefault()?.Geometry.Location;
                    coordinates = new Coordinates
                    {
                        Lat = result.Latitude,
                        Lon = result.Longitude
                    };
                }
                _coordinatesCache.Add(address, coordinates);
                return coordinates;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(new Coordinates { Lat = 0, Lon = 0 });
            }
        }
    }
}
