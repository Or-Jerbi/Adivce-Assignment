using AdviceAssignement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdviceAssignement.DAL.Data
{
    public class ElevatorData
    {
        private readonly ElevatorsManagementDbContext _context;

        public ElevatorData(ElevatorsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Elevator>> GetAllElevators()
        {
            try
            {
                return await _context.Elevators.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllElevators error: {ex.Message}");
                throw;
            }
        }

        public async Task<Elevator?> GetElevatorsByBuilding(int buildingId)
        {
            try
            {
                return await _context.Elevators.FirstOrDefaultAsync(e => e.BuildingId == buildingId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetElevatorsByBuilding error (buildingId={buildingId}): {ex.Message}");
                throw;
            }
        }

        public async Task<Elevator?> CreateNewElevator(Elevator elevator)
        {
            try
            {
                var res = await _context.Elevators.AddAsync(elevator);
                if (res != null)
                {
                    await _context.SaveChangesAsync();
                    return elevator;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateNewElevator error: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<Elevator?> UpdateElevator(Elevator updatedElevator)
        {
            try
            {
                var res = _context.Elevators.Attach(updatedElevator);
                if (res != null)
                {
                    await _context.SaveChangesAsync();
                    return updatedElevator;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"UpdateElevator error (id={updatedElevator?.Id}): {ex.Message}");
                throw;
            }
            return null;
        }
    }
}
