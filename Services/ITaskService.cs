using DotnetApi.Models;

namespace DotnetApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int Id);
        Task<TaskItem?> CreateAsync(TaskItem TaskItem);
        Task<TaskItem?> UpdateAsync(int Id, TaskItem TaskItem);
        Task<bool> DeleteAsync(int Id);
    }
}
