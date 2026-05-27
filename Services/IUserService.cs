using DotnetApi.Models;

namespace DotnetApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int Id);
        Task<User?> CreateAsync(User User);
        Task<User?> UpdateAsync(int Id, User User);
        Task<bool> DeleteAsync(int Id);
    }
}
