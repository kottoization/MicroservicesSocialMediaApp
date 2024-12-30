using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommentAPI.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(Guid id);
        Task CreateAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Guid id);
    }
}
