using PeopleAPI.Data;
using PeopleAPI.DTO;
using PeopleAPI.Models;
//using Microsoft.EntityFrameworkCore;
using PeopleAPI.DTO;
using System.Data.Entity;

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
        private readonly PeopleDbContext _context;

        public PeopleService(PeopleDbContext context)
        {
            _context = context;
        }

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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> UpdateUserAsync(Guid id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
