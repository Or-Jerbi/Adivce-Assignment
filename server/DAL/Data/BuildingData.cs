using System.Data;
using AdviceAssignement.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace AdviceAssignement.DAL.Data
{
    public class BuildingData
    {
        private readonly string _connectionString;

        public BuildingData(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        public async Task<List<Building>> GetAllBuildings()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM Buildings";
                var buildings = await connection.QueryAsync<Building>(sql);
                return buildings.ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetAllBuildings: {ex.Message}");
                throw;
            }
        }


        public async Task<Building?> GetBuildingById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM Buildings WHERE id = @id";
                var building = await connection.QuerySingleOrDefaultAsync<Building>(sql,new { id });
                return building;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetBuildingById with id {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Building>> GetBuildingsByUserId(int userId)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM Buildings WHERE UserID = @userId";
                var buildings = await connection.QueryAsync<Building>(sql,new { userId });
                return buildings.ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetBuildingsByUserId with id {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Building> CreateBuilding(Building building)
        {
            try
            {
                var connection = CreateConnection();
                var sql = "INSERT INTO Buildings (UserID,Name,NumberOfFloors) VALUES (@UserId,@Name,@NumberOfFloors) SELECT CAST(SCOPE_IDENTITY() as int);";  
                var newId = await connection.QuerySingleAsync<int>(sql, building);
                building.Id = newId;
                return building;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in CreateBuilding with building {building}: {ex.Message}");
                throw;
            }
        }
    }
}
