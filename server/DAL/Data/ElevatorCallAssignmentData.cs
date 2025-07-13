using AdviceAssignement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdviceAssignement.DAL.Data
{
    public class ElevatorCallAssignmentData
    {
        private readonly ElevatorsManagementDbContext _dbContext;
        public ElevatorCallAssignmentData(ElevatorsManagementDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<ElevatorCallAssignment?> CreateCallAssigment(int buildingId)
        {
            try
            {
                var elevator = await _dbContext.Elevators.FirstOrDefaultAsync(e => e.BuildingId == buildingId);
                if (elevator != null)
                {
                    ElevatorCallAssignment callAssignment = new ElevatorCallAssignment()
                    {
                        ElevatorId = elevator.Id,
                        AssignmentTime = DateTime.UtcNow
                    };
                    var res = await _dbContext.ElevatorCallAssignments.AddAsync(callAssignment);
                    if (res != null)
                    {
                        await _dbContext.SaveChangesAsync();
                        return callAssignment;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to create new elevator call assigment: {ex.ToString()}");
            }
            return null;
        }
    }
}
