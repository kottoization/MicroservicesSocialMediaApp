using Microsoft.AspNetCore.Mvc;
using PeopleAPI.DTO;
//using PeopleAPI.DTOs;
using PeopleAPI.Services;

namespace PeopleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var user = await _peopleService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _peopleService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _peopleService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
        {
            var user = await _peopleService.UpdateUserAsync(id, userDto);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var success = await _peopleService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
