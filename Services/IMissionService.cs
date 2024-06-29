using M16API.Data;
using M16API.Models;
using M16API.Utils;
using Microsoft.EntityFrameworkCore;

namespace M16API.Services
{
    public interface IMissionService
    {
        Task<Mission> CreateMission(Mission mission);
        Task<List<Mission>> GetMissions();
        Task<CountryIsolationDegree> GetCountriesByIsolation();
        Task<FindClosestResponse> GetClosestMission(string targetLocation);

    }

    public class MissionService : IMissionService
    {
        private readonly ApplicationDbContext _context;

        public MissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mission>> GetMissions()
        {
            try
            {
                var missions = await _context.Missions.ToListAsync();
                return missions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            try
            {
                var coordinates = await GetCoordinatesFromAddress(mission.Address);
                var newMission = new Mission
                {
                    Id = Guid.NewGuid(),
                    Agent = mission.Agent,
                    Country = mission.Country,
                    Address = mission.Address,
                    Latitude = coordinates.Lat,
                    Longitude = coordinates.Lon,
                    Date = DateTime.UtcNow
                };
            
                _context.Missions.Add(newMission);
                await _context.SaveChangesAsync();
                return newMission;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FindClosestResponse> GetClosestMission(string targetLocation)
        {
            try
            {
                Mission closestMission = null;
                double closestDistance = double.MaxValue;
                Coordinates targetCoordinates = MissionUtils.IsCoordinates(targetLocation) ?
                    MissionUtils.GetCoordinatesFromString(targetLocation) :
                    await GetCoordinatesFromAddress(targetLocation);
                
                if (targetCoordinates == null)
                {
                    // return BadRequest("Invalid target location");
                    // return "Invalid target location";
                    return null;
                }
            
                // todo: change this:
                var missions = await _context.Missions.ToListAsync();
                foreach (var mission in missions)
                {
                    var missionCoordinates = new Coordinates { Lat = mission.Latitude, Lon = mission.Longitude };
                    var distance = MissionUtils.GetDistance(targetCoordinates, missionCoordinates);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestMission = mission;
                    }
                }
            
                if (closestMission == null)
                {
                    // return NotFound("No missions found");
                    // return "No missions found";
                    return null;
                }
            
                var response = new FindClosestResponse
                {
                    ClosestMission = closestMission,
                    Distance = closestDistance
                };
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static async Task<Coordinates> GetCoordinatesFromAddress(string address)
        {
            // var request = new AddressGeocodeRequest
            // {
            //     Key = _googleMapsApiKey,
            //     Address = address
            // };
            // var response = await GoogleMaps.Geocode.Address.QueryAsync(request);
            // return response.Results.FirstOrDefault()?.Geometry.Location;
            // Implement logic to fetch coordinates from address (e.g., using Google Maps API)
            return await Task.FromResult(new Coordinates { Lat = 24.1112, Lon = 21.12342 });
        }
    }
}
