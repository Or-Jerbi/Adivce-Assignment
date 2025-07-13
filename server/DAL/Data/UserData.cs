using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdviceAssignement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AdviceAssignement.DAL.Data
{
    public class UserData
    {
        private readonly ElevatorsManagementDbContext _context;

        public UserData(ElevatorsManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetAllUsers error: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"GetUserByEmail error (email={email}): {ex.Message}");
                throw;
            }
        }

        public async Task<User?> CreateUser(User newUser)
        {
            try
            {
                var res = await _context.Users.AddAsync(newUser);
                if (res != null)
                {
                    await _context.SaveChangesAsync();
                    return newUser;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CreateUser error (email={newUser?.Email}): {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<User?> Login(string email, string password)
        {
            try
            {
                var res = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
                if (res != null)
                {
                    return res;
                }
                else
                {
                    throw new Exception("email or password incorrect");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Login error (email={email}): {ex.Message}");
                throw;
            }
        }

    }
}
