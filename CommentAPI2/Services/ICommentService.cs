using SharedModels.Models;

namespace CommentAPI2.Services
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
