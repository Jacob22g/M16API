// using GoogleApi;
// using GoogleApi.Entities.Common;
// using GoogleApi.Entities.Maps.DistanceMatrix.Request;
// using GoogleApi.Entities.Maps.Geocoding.Address.Request;
// using GoogleApi.Entities.Maps.Geocoding.Coordinate.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public IActionResult GetMissions()
        {
            var missions = _missionService.GetMissions();
            return Ok(missions);
        }

        // [HttpGet("countries-by-isolation")]
        // public IActionResult GetCountriesByIsolation()
        // {
        //     // TODO: Missions will be fetched from the DB

        //     var agentMissionCount = Missions
        //         .GroupBy(m => m.Agent)
        //         .ToDictionary(g => g.Key, g => g.Count());

        //     var isolatedAgents = agentMissionCount
        //         .Where(kv => kv.Value == 1)
        //         .Select(kv => kv.Key)
        //         .ToHashSet();

        //     var countryIsolationDegree = Missions
        //         .Where(m => isolatedAgents.Contains(m.Agent))
        //         .GroupBy(m => m.Country)
        //         .OrderByDescending(g => g.Count())
        //         .Select(g => new { Country = g.Key, IsolationDegree = g.Count() })
        //         .FirstOrDefault();

        //     return Ok(countryIsolationDegree);
        // }

        // [HttpPost("find-closest")]
        // public async Task<IActionResult> FindClosestMission([FromBody] FindClosestRequest request)
        // {
        //     Location targetLocation;
        //     if (IsCoordinates(request.TargetLocation))
        //     {
        //         targetLocation = await GetGeocodeFromCoordinatesAsync(request.TargetLocation);
        //     }
        //     else
        //     {
        //         targetLocation = await GetGeocodeFromAddressAsync(request.TargetLocation);
        //     }

        //     if (targetLocation == null)
        //     {
        //         return BadRequest("Invalid target location");
        //     }

        //     Mission closestMission = null;
        //     double closestDistance = double.MaxValue;

        //     // TODO: Missions will be fetched from the DB
        //     foreach (var mission in Missions)
        //     {
        //         var missionLocation = await GetGeocodeFromAddressAsync(mission.Address);
        //         if (missionLocation != null)
        //         {
        //             var distance = await GetDistanceAsync(targetLocation, missionLocation);
        //             if (distance < closestDistance)
        //             {
        //                 closestDistance = distance;
        //                 closestMission = mission;
        //             }
        //         }
        //     }

        //     if (closestMission == null)
        //     {
        //         return NotFound("No missions found");
        //     }

        //     var response = new FindClosestResponse
        //     {
        //         ClosestMission = closestMission,
        //         Distance = closestDistance
        //     };

        //     return Ok(response);
        // }

        // private bool IsCoordinates(string input)
        // {
        //     var coordinateRegex = new Regex(@"^-?\d+(\.\d+)?,-?\d+(\.\d+)?$");
        //     return coordinateRegex.IsMatch(input);
        // }

        // private async Task<Location> GetGeocodeFromAddressAsync(string address)
        // {
        //     var request = new AddressGeocodeRequest
        //     {
        //         Key = _googleMapsApiKey,
        //         Address = address
        //     };
        //     var response = await GoogleMaps.Geocode.Address.QueryAsync(request);
        //     return response.Results.FirstOrDefault()?.Geometry.Location;
        // }

        // private async Task<Location> GetGeocodeFromCoordinatesAsync(string coordinates)
        // {
        //     var parts = coordinates.Split(',');
        //     if (parts.Length != 2 ||
        //         !double.TryParse(parts[0], out var lat) ||
        //         !double.TryParse(parts[1], out var lng))
        //     {
        //         return null;
        //     }

        //     var request = new CoordinateGeocodeRequest
        //     {
        //         Key = _googleMapsApiKey,
        //         Location = new Location(lat, lng)
        //     };
        //     var response = await GoogleMaps.Geocode.Coordinate.QueryAsync(request);
        //     return response.Results.FirstOrDefault()?.Geometry.Location;
        // }

        // private async Task<double> GetDistanceAsync(Location origin, Location destination)
        // {
        //     var request = new DistanceMatrixRequest
        //     {
        //         Key = _googleMapsApiKey,
        //         Origins = new[] { origin },
        //         Destinations = new[] { destination }
        //     };
        //     var response = await GoogleMaps.DistanceMatrix.QueryAsync(request);
        //     var element = response.Rows.FirstOrDefault()?.Elements.FirstOrDefault();
        //     return element?.Distance.Value ?? double.MaxValue;
        // }
    }
}
