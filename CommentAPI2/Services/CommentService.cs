using SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CommentAPI2.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            
            return await _dbContext.Comments.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            var existingComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
            if (existingComment != null)
            {
                existingComment.Content = comment.Content;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
  
        }
    }

}
