
using AdviceAssignement.DAL.Entities;
using Humanizer;
using AdviceAssignement.HubRealTime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AdviceAssignement.Services
{
    public class ElevatorSimulationService : BackgroundService
    {
        private readonly IHubContext<ElevatorHub> _hubContext;
        private List<int> _targetFloors = new List<int>();
        private readonly IServiceScopeFactory _scopeFactory;
        private Elevator _elevator;
        private int _timer = 0;
        private int beats;
        public ElevatorSimulationService(IServiceScopeFactory serviceScopeFactory,
            Elevator elevator, IHubContext<ElevatorHub> hubContext, int beats)
        {
            _scopeFactory = serviceScopeFactory;
            _elevator = elevator;
            _hubContext = hubContext;
            this.beats = beats;
        }

        public ElevatorCall HandleNewCall(ElevatorCall newCall, int direction)
        {
            int addedFloor = newCall.DestinaionFloor ?? newCall.RequestedFloor;

            if (_elevator.Direction == (int)Enums.ElevatorDirection.None)
            {
                if (_elevator.CurrentFloor < newCall.RequestedFloor)
                {
                    _elevator.Direction = (int)Enums.ElevatorDirection.Up;
                    _elevator.Status = (int)Enums.ElevatorStatus.MovingUp;
                }
                else if (_elevator.CurrentFloor > newCall.RequestedFloor)
                {
                    _elevator.Direction = (int)Enums.ElevatorDirection.Down;
                    _elevator.Direction = (int)Enums.ElevatorStatus.MovingDown;
                }
                _targetFloors.Add(addedFloor);
                newCall.IsHandled = true;
            } 

            else if (_elevator.Direction == direction)
            {
                if (_targetFloors.Contains(addedFloor))
                {
                    newCall.IsHandled = true;
                }
                else if (direction == (int)Enums.ElevatorDirection.Up && _targetFloors.Max() > addedFloor && addedFloor > _elevator.CurrentFloor || newCall.DestinaionFloor != null)
                {
                    _targetFloors.Add(addedFloor);
                    newCall.IsHandled = true;
                    _targetFloors.Sort();
                }
                else if (direction == (int)Enums.ElevatorDirection.Down && _targetFloors.Min() < addedFloor && addedFloor < _elevator.CurrentFloor || newCall.DestinaionFloor != null)
                {
                    _targetFloors.Add(addedFloor);
                    newCall.IsHandled = true;
                    _targetFloors.Sort((a, b) => b.CompareTo(a));
                }
            }
            return newCall;
        }

        public async Task AddNextCallToTarget(ElevatorsManagementDbContext dbContext)
        {
            if (_targetFloors.Count == 0)
            {
                var unhandeledCalls = dbContext.ElevatorCalls.Where(e => e.IsHandled == false).OrderBy(c => c.CallTime).ToList();
                ElevatorCall oldestCall = unhandeledCalls.FirstOrDefault();
                if (oldestCall != null)
                {
                    _targetFloors.Add(oldestCall.DestinaionFloor ?? oldestCall.RequestedFloor);
                    oldestCall.IsHandled = true;
                    dbContext.ElevatorCalls.Update(oldestCall);
                    await dbContext.SaveChangesAsync();
                    await SetElevatorAccordingToNewTarget(dbContext);
                    IEnumerable<ElevatorCall> targets = Enumerable.Empty<ElevatorCall>();
                    if (_elevator.Direction == (int)Enums.ElevatorDirection.Up)
                    {
                        targets = unhandeledCalls.Where(c => c.DestinaionFloor <= _targetFloors[0] && c.RequestedFloor >= _elevator.CurrentFloor);
                    }
                    else if (_elevator.Direction == (int)Enums.ElevatorDirection.Down)
                    {
                        targets = unhandeledCalls.Where(c => c.DestinaionFloor >= _targetFloors[0] && c.RequestedFloor <= _elevator.CurrentFloor);
                    }
                    foreach (var target in targets)
                    {
                        var call = HandleNewCall(target, _elevator.Direction);
                        if (call.IsHandled)
                        {
                            await dbContext.ElevatorCallAssignments.AddAsync(new ElevatorCallAssignment()
                            {
                                ElevatorId = _elevator.Id,
                                AssignmentTime = DateTime.UtcNow,
                            });
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    _elevator.Status = (int)Enums.ElevatorStatus.Idle;
                    _elevator.Direction = (int)Enums.ElevatorDirection.None;
                    dbContext.Elevators.Update(_elevator);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        public async Task ArrivedToTargetFloor(ElevatorsManagementDbContext dbContext)
        {
            if (_elevator.DoorStatus == (int)Enums.DoorStatus.Closed && _timer == 0)
            {
                _elevator.Status = (int)Enums.ElevatorStatus.OpeningDoors;
                _elevator.DoorStatus = (int)Enums.DoorStatus.Open;
                _timer = 1;
            }
            else if (_elevator.DoorStatus == (int)Enums.DoorStatus.Open && _timer == 0)
            {
                _targetFloors.Remove(_elevator.CurrentFloor);

                if (_targetFloors.Count > 0)
                {
                    await SetElevatorAccordingToNewTarget(dbContext);
                }
                else
                {
                    await AddNextCallToTarget(dbContext);
                }
                _elevator.Status = (int)Enums.ElevatorStatus.ClosingDoors;
                _elevator.DoorStatus = (int)Enums.DoorStatus.Closed;
                _timer = 1;
            }
        }

        public async Task SetElevatorAccordingToNewTarget(ElevatorsManagementDbContext dbContext)
        {
            if (_elevator == null) { return; }
            if (_targetFloors[0] > _elevator.CurrentFloor)
            {
                _elevator.Direction = (int)Enums.ElevatorDirection.Up;
                _elevator.Status = (int)Enums.ElevatorStatus.MovingUp;
            }
            else if (_targetFloors[0] < _elevator.CurrentFloor)
            {
                _elevator.Direction = (int)Enums.ElevatorDirection.Down;
                _elevator.Status = (int)Enums.ElevatorStatus.MovingDown;
            }
            dbContext.Elevators.Update(_elevator);
            await dbContext.SaveChangesAsync();
        }

        public void MoveElevator()
        {
            if (_timer > 0)
            {
                _timer--;
            }

            else if (_elevator.Direction == (int)Enums.ElevatorDirection.Up)
            {
                _elevator.CurrentFloor++;
            }
            else if (_elevator.Direction == (int)Enums.ElevatorDirection.Down || _elevator.Status == (int)Enums.ElevatorStatus.Idle && _elevator.CurrentFloor > 0)
            {
                _elevator.CurrentFloor--;
            }
        }

        public async Task UpdateElevatorAndNotifyClients(ElevatorsManagementDbContext dbContext)
        {
            try
            {
                dbContext.Elevators.Update(_elevator);
                await dbContext.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ElevatorUpdated", new
                {
                    CurrentFloor = _elevator.CurrentFloor,
                    Status = _elevator.Status,
                    Direction = _elevator.Direction
                });
            }
            catch (Exception ex)
            {
                throw new Exception("failed to update");
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ElevatorsManagementDbContext>();
                 
                    if (_targetFloors.Count == 0)
                    {
                        await AddNextCallToTarget(dbContext);
                    }

                    else if (_elevator.CurrentFloor == _targetFloors[0])
                    {
                        await ArrivedToTargetFloor(dbContext);
                    }
                    else 
                    { 
                        await SetElevatorAccordingToNewTarget(dbContext); 
                    }

                    MoveElevator();

                    await UpdateElevatorAndNotifyClients(dbContext);
                }
                await Task.Delay(TimeSpan.FromSeconds(beats), stoppingToken);
            }
        }
    }
}
