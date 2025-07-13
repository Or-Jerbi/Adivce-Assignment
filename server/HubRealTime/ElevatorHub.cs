using AdviceAssignement.DAL.Data;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.Services;
using Microsoft.AspNetCore.SignalR;

namespace AdviceAssignement.HubRealTime
{
    public class ElevatorHub : Hub
    {
        private readonly SimulationManager _simulationManager;

        public ElevatorHub(SimulationManager simulationManager)
        {
            _simulationManager = simulationManager;
        }

        public async Task StartSimulation(int buildingId)
        {
            Console.WriteLine($"Received start request for Building ID: {buildingId}");
            await _simulationManager.StartSimulation(buildingId);
            await Clients.Caller.SendAsync("SimulationStarted", buildingId);
        }

        public async Task StopSimulation(int buildingId)
        {
            Console.WriteLine($"Received stop request for Building ID: {buildingId}");
            _simulationManager.StopSimulation(buildingId);
            await Clients.Caller.SendAsync("SimulationStopped", buildingId);
        }
    }

}
