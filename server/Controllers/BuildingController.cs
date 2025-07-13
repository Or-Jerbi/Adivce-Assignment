using System.Security.Claims;
using AdviceAssignement.DAL.Data;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdviceAssignement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingData _data;
        private readonly ElevatorData _elevatorData;

        public BuildingController(BuildingData data, ElevatorData elevatorData)
        {
            _data = data;
            _elevatorData = elevatorData;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBuildings()
        {
            try
            {
                var res = await _data.GetAllBuildings();
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllBuildings error: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching buildings.");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBuildingsByUserId([FromHeader] int userId)
        {
            try
            {
                var res= await _data.GetBuildingsByUserId(userId);  
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetBuildingsByUserId error: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching buildings.");
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBuildingById([FromHeader] int id)
        {
            try
            {
                var res = await _data.GetBuildingById(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetBuildingById error: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching building.");
            }
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserBuildings()
        {
            try 
            { 
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var buildings = await _data.GetBuildingsByUserId(userId);
                return Ok(buildings);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetUserBuildings error: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving buildings.");
            }

        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateBuilding(BuildingDto buildingDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            Building newBuilding = new Building()
            {
                Name = buildingDto.Name,
                UserId = int.Parse(userIdClaim.Value),
                NumberOfFloors = buildingDto.NumberOfFloors,
            };
            var res = await _data.CreateBuilding(newBuilding);
            Elevator newElevator = new Elevator()
            {
                BuildingId = res.Id,
                Direction = (int)Enums.ElevatorDirection.None,
                DoorStatus = (int)Enums.DoorStatus.Closed,
                Status = (int)Enums.ElevatorStatus.Idle,
                CurrentFloor = 0
            };

            try { var elevator = await _elevatorData.CreateNewElevator(newElevator); }
            catch (Exception er) { return Conflict(er.Message); }
            return Ok(res);
        }

    }
}
