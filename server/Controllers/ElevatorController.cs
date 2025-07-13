using AdviceAssignement.DAL.Data;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdviceAssignement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly ElevatorData _data;

        public ElevatorController(ElevatorData data)
        {
            _data = data;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllElevators()
        {
            try
            {
                var res = await _data.GetAllElevators();
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllElevators error: {ex.Message}");
                return StatusCode(500, "Failed to fetch elevators.");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetElevatorCall([FromHeader] int buildingId)
        {
            try
            {
                var res = await _data.GetElevatorsByBuilding(buildingId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetElevatorCall error: {ex.Message}");
                return StatusCode(500, "Failed to fetch elevator.");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateElevator([FromHeader] Elevator updatedElevator)
        {
            try
            {
                var res = await _data.UpdateElevator(updatedElevator);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"UpdateElevator error: {ex.Message}");
                return StatusCode(500, "Failed to update elevator.");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateElevator(int buildingId)
        {
            try
            {
                Elevator newElevator = new Elevator()
                {
                    BuildingId = buildingId,
                    Direction = (int)Enums.ElevatorDirection.None,
                    DoorStatus = (int)Enums.DoorStatus.Closed,
                    Status =  (int)Enums.ElevatorStatus.Idle,
                    CurrentFloor = 0
                };
                var res = await _data.CreateNewElevator(newElevator);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateElevator error: {ex.Message}");
                return StatusCode(500, "Failed to create elevator.");
            }
        }
    }
}
