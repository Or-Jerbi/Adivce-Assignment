using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AdviceAssignement.DAL.Entities;
using AdviceAssignement.HubRealTime;
using System.Threading;
using System.Threading.Tasks;
using Mono.TextTemplating;
using AdviceAssignement.DAL.Data;
using AdviceAssignement.DTOs;

namespace AdviceAssignement.Services
{
    public class SimulationManager : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<ElevatorHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<int, ElevatorSimulationService> _activeSimulations;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public SimulationManager(
            IServiceScopeFactory scopeFactory,
            IHubContext<ElevatorHub> hubContext,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _configuration = configuration;
            _activeSimulations = new ConcurrentDictionary<int, ElevatorSimulationService>();
        }

        public async Task StartSimulation(int buildingId)
        {
            await _lock.WaitAsync();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    if (!_activeSimulations.ContainsKey(buildingId))
                    {
                        var elevatorData = scope.ServiceProvider.GetRequiredService<ElevatorData>();

                        var elevator = await elevatorData.GetElevatorsByBuilding(buildingId);
                        if (elevator == null)
                        {
                            Console.WriteLine($"Elevator not found for Building ID {buildingId}. Cannot start simulation.");
                            return;
                        }

                        var simulation = new ElevatorSimulationService(
                            _scopeFactory,
                            elevator,
                            _hubContext,
                            int.Parse(_configuration.GetSection("Beats")["Time"])
                        );
                        _ = simulation.StartAsync(CancellationToken.None);
                        _activeSimulations.TryAdd(elevator.BuildingId, simulation);
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task StopSimulation(int buildingId)
        {
            await _lock.WaitAsync();
            try
            {
                if (_activeSimulations.TryRemove(buildingId, out var simulation))
                {
                    try
                    {
                        await simulation.StopAsync(CancellationToken.None);
                        Console.WriteLine($"Simulation for Building ID {buildingId} has been explicitly stopped.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error stopping simulation for Building ID {buildingId}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"No active simulation found for Building ID {buildingId} to stop.");
                }
            }
            finally
            { 
                _lock.Release(); 
            }
        }

        public ElevatorCall HandleNewCall(ElevatorCall newCall, int direction)
        {
            if (_activeSimulations.TryGetValue(newCall.BuildingId, out var simulation))
            {
                return simulation.HandleNewCall(newCall, direction);
            }
            else
            {
                throw new InvalidOperationException($"Simulation for elevator with building ID {newCall.BuildingId} is not active.");
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var simulation in _activeSimulations.Values)
            {
                simulation.StopAsync(cancellationToken).Wait(); 
            }
            _activeSimulations.Clear();
            return Task.CompletedTask;
        }
    }
}