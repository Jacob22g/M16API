// using GoogleApi;
// using GoogleApi.Entities.Common;
// using GoogleApi.Entities.Maps.DistanceMatrix.Request;
// using GoogleApi.Entities.Maps.Geocoding.Address.Request;
// using GoogleApi.Entities.Maps.Geocoding.Coordinate.Request;
using Microsoft.AspNetCore.Mvc;
using M16API.Models;
using M16API.Services;

namespace M16API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionsController : ControllerBase
    {
        private readonly IMissionService _missionService;

        public MissionsController(IMissionService missionService)
        {
            _missionService = missionService;
        }

        [HttpPost]
        public async Task<IActionResult> PostMission([FromBody] Mission mission)
        {
            var createdMission = await _missionService.CreateMission(mission);
            return CreatedAtAction(nameof(PostMission), new { id = createdMission.Agent }, createdMission);
        }

        [HttpGet]
        public async Task<IActionResult> GetMissions()
        {
            var missions = await _missionService.GetMissions();
            return Ok(missions);
        }

        [HttpGet("countries-by-isolation")]
        public async Task<IActionResult> GetCountriesByIsolation()
        {
            var mostIsolatedCountry = await _missionService.GetCountriesByIsolation();
            return Ok(mostIsolatedCountry);
        }

        [HttpPost("find-closest")]
        public async Task<IActionResult> FindClosestMission([FromBody] FindClosestRequest request)
        {
            var mostIsolatedCountry = await _missionService.GetClosestMission(request.TargetLocation);
            return Ok(mostIsolatedCountry);
        }

    }
}
