using AdviceAssignement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using AdviceAssignement.Services;

namespace AdviceAssignement.DAL.Data
{
    public class ElevatorCallData
    {
        private readonly ElevatorsManagementDbContext _context;

        public ElevatorCallData(ElevatorsManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<ElevatorCall>> GetAllElevatorCall(int buildingId)
        {
            List<ElevatorCall> calls = new List<ElevatorCall>();
            try
            {
                calls = await _context.ElevatorCalls.Where( e => e.BuildingId == buildingId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllElevatorCall error: {ex.Message}");
                throw;
            }
            return calls;
        }

        public async Task<ElevatorCall?> GetElevatorCallById(int id)
        {
            try
            {
                return await _context.ElevatorCalls.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetElevatorCallById error (id={id}): {ex.Message}");
                throw;
            }
        }

        public async Task<ElevatorCall?> UpdateElevatorCall(ElevatorCall updatedElevatorCall)
        {
            try
            {
                var res = _context.ElevatorCalls.Attach(updatedElevatorCall);
                
                if (res != null)
                {
                    await _context.SaveChangesAsync();
                    return updatedElevatorCall;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"UpdateElevatorCall error (id={updatedElevatorCall?.Id}): {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<ElevatorCall?> CreateElevatorCall(ElevatorCall newEleatorCall)
        {
            try
            {
                var res = await _context.ElevatorCalls.AddAsync(newEleatorCall);
                if (res != null)
                {
                    await _context.SaveChangesAsync();
                    return newEleatorCall;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateElevatorCall error: {ex.Message}");
                throw;
            }
            return null;
        }
    }
}
