using PeopleAPI.DTO;
using PeopleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAPI.Services
{
    public interface IPeopleService
    {
        Task<User> CreateUserAsync(UserDto userDto);
        Task<User> GetUserByIdAsync(Guid id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(Guid id, UserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }

    public class PeopleService : IPeopleService
    {
        private readonly List<User> _users = new();

        public async Task<User> CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                CreatedAt = DateTime.UtcNow
            };
            _users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return await Task.FromResult(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<User> UpdateUserAsync(Guid id, UserDto userDto)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            return await Task.FromResult(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            _users.Remove(user);
            return await Task.FromResult(true);
        }
    }
}
