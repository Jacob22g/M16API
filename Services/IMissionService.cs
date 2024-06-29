using M16API.Models;

namespace M16API.Services
{
    public interface IMissionService
    {
        Task<Mission> CreateMission(Mission mission);
        Task<List<Mission>> GetMissions();
        Task<CountryIsolationDegree> GetCountriesByIsolation();
        Task<FindClosestResponse> GetClosestMission(string targetLocation);
    }
}
