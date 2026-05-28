using DotnetApi.Models;
using DotnetApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var user = await _userService.GetByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { Id = createdUser?.Id }, createdUser);
        }

        [HttpPatch("{Id:int}")]
        public async Task<IActionResult> Update(int Id, [FromBody] User user)
        {
            var updatedUser = await _userService.UpdateAsync(Id, user);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id) 
        {
            var deleted = await _userService.DeleteAsync(Id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }
    }
}
