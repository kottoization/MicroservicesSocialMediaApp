﻿using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostAPI.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(Guid id);
        Task CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task<string> GetUserNameById(string userId);
        Task DeleteAsync(Guid id);
    }
}
