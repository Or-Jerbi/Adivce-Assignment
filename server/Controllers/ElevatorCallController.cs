using AdviceAssignement.DAL.Data;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.DTOs;
using AdviceAssignement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdviceAssignement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorCallController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ElevatorCallData _data;
        private readonly ElevatorCallAssignmentData _callAssignmendata;
        private readonly SimulationManager _simulationManager;

        public ElevatorCallController(IConfiguration config, ElevatorCallData data, SimulationManager simulationManager, ElevatorCallAssignmentData callAssignmendata)
        {
            _config = config;
            _data = data;
            _simulationManager = simulationManager;
            _callAssignmendata = callAssignmendata;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllElevatorCalls([FromHeader] int buildingId) 
        {
            try
            {
                var res = await _data.GetAllElevatorCall(buildingId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllElevatorCalls error: {ex.Message}");
                return StatusCode(500, "Failed to fetch elevator calls");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetElevatorCallById([FromHeader] int elevatorCallId)
        {
            try
            {
                var res = await _data.GetElevatorCallById(elevatorCallId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetElevatorCallById error: {ex.Message}");
                return StatusCode(500, "Failed to fetch elevator call");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateElevatorCall([FromHeader] ElevatorCall updatedElevatorCall)
        {
            try
            {
                var res = await _data.UpdateElevatorCall(updatedElevatorCall);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"UpdateElevatorCall error: {ex.Message}");
                return StatusCode(500, "Failed to update elevator call");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateElevatorCall(ElevatorCallDto elevatorCallDto)
        {
            try
            {
                    ElevatorCall newElevatorCall = new ElevatorCall()
                {
                    BuildingId = elevatorCallDto.BuildingId,
                    RequestedFloor = elevatorCallDto.RequestedFloor,
                    DestinaionFloor = elevatorCallDto.DestinaionFloor,
                };

                var res = _simulationManager.HandleNewCall(newElevatorCall, elevatorCallDto.Direction);
                await _data.CreateElevatorCall(res);
                if (res.IsHandled)
                 {
                    await _callAssignmendata.CreateCallAssigment(res.BuildingId);
                }
                    return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateElevatorCall error: {ex.Message}");
                return StatusCode(500, "Failed to create elevator call");
            }
        }
    }
}
