using DotnetApi.Models;
using DotnetApi.Models.Requests;
using DotnetApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // constructor
        public TaskController(ITaskService taskervice)
        {
            _taskService = taskervice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var task = await _taskService.GetAllAsync();
            return Ok(task);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var task = await _taskService.GetByIdAsync(Id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItemUpsertRequest request)
        {
            var task = new TaskItem
            {
                Id = request.Id ?? 0,
                Title = request.Title,
                IsCompleted = request.IsCompleted ?? false,
                UserId = request.UserId
            };

            var createdTask = await _taskService.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { Id = createdTask?.Id }, createdTask);
        }

        [HttpPatch("{Id:int}")]
        public async Task<IActionResult> Update(int Id, [FromBody] TaskItemUpsertRequest request)
        {
            var task = new TaskItem
            {
                Id = request.Id ?? Id,
                Title = request.Title,
                IsCompleted = request.IsCompleted ?? false,
                UserId = request.UserId
            };

            var updatedTask = await _taskService.UpdateAsync(Id, task);
            if (updatedTask == null)
            {
                return NotFound();
            }
            return Ok(updatedTask);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var deleted = await _taskService.DeleteAsync(Id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }
    }
}
