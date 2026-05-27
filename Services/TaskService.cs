
using DotnetApi.Data;
using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;

        // constructor
        public TaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TaskItem?> CreateAsync(TaskItem TaskItem)
        {
            _dbContext.TaskItems.Add(TaskItem);
            await _dbContext.SaveChangesAsync();
            return TaskItem;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            // take 1000 users to prevent memory issues
            return await _dbContext.TaskItems
                .AsNoTracking()
                .OrderBy(u => u.Id) // 1. Siempre ordenar antes de tomar
                .Take(1000)
                .ToListAsync();     // 2. Materializar la consulta
        }

        public async Task<TaskItem?> GetByIdAsync(int Id)
        {
            return await _dbContext.TaskItems.FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var existingTaskItem = await _dbContext.TaskItems.FindAsync(Id);
            if (existingTaskItem == null)
            {
                return false;
            }
            _dbContext.TaskItems.Remove(existingTaskItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TaskItem?> UpdateAsync(int Id, TaskItem TaskItem)
        {
            var existingTaskItem = await _dbContext.TaskItems.FindAsync(Id);
            if (existingTaskItem == null)
            {
                return null;
            }
            existingTaskItem.Title = TaskItem.Title;
            existingTaskItem.IsCompleted = TaskItem.IsCompleted;
            await _dbContext.SaveChangesAsync();
            return existingTaskItem;
        }
    }
}
