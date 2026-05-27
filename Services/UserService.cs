using DotnetApi.Data;
using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Services
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _dbContext;

        // constructor
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> CreateAsync(User User)
        {
            _dbContext.Users.Add(User);
            await _dbContext.SaveChangesAsync();
            return User;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // take 1000 users to prevent memory issues
            return await _dbContext.Users
                .AsNoTracking()
                .OrderBy(u => u.Id) // 1. Siempre ordenar antes de tomar
                .Take(1000)
                .ToListAsync();     // 2. Materializar la consulta
        }

        public async Task<User?> GetByIdAsync(int Id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var existingUser = await _dbContext.Users.FindAsync(Id);
            if (existingUser == null)
            {
                return false;
            }
            _dbContext.Users.Remove(existingUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> UpdateAsync(int Id, User User)
        {
            var existingUser = await _dbContext.Users.FindAsync(Id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Name = User.Name;
            existingUser.Email = User.Email;
            existingUser.Role = User.Role;
            await _dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}
