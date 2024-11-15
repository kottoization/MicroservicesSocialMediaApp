using Microsoft.AspNetCore.Mvc;
using PeopleAPI.DTO;
using PeopleAPI.Models;
using PeopleAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // POST: api/People
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserDto userDto)
        {
            var user = await _peopleService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // GET: api/People/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _peopleService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _peopleService.GetAllUsersAsync();
            return Ok(users);
        }

        // PUT: api/People/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(Guid id, UserDto userDto)
        {
            var updatedUser = await _peopleService.UpdateUserAsync(id, userDto);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        // DELETE: api/People/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _peopleService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
