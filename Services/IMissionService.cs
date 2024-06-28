
using M16API.Data;
using M16API.Models;
using Microsoft.EntityFrameworkCore;

namespace M16API.Services
{
    public interface IMissionService
    {
        Task<Mission> CreateMission(Mission mission);
        Task<List<Mission>> GetMissions();
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
            var missions = await _context.Missions.ToListAsync();
            return missions;
        }

        public async Task<Mission> CreateMission(Mission mission)
        {
            if (mission.Coordinates == null)
            {
                mission.Coordinates = await GetCoordinatesFromAddress(mission.Address);
            }
            _context.Missions.Add(mission); // Add the newMission to the context
            await _context.SaveChangesAsync(); // Save changes asynchronously
            return mission; // Return the newly created mission
        }
        
        private async Task<Coordinates> GetCoordinatesFromAddress(string address)
        {
            // Implement logic to fetch coordinates from address (e.g., using Google Maps API)
            // Example mock implementation:
            return new Coordinates(25.1112, 23.1234);
        }
    }
}
