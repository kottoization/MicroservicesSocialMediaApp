using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly List<Comment> _comments = new List<Comment>();

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await Task.FromResult(_comments);
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            return await Task.FromResult(comment);
        }

        public async Task CreateAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _comments.Add(comment);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            var existingComment = _comments.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment != null)
            {
                existingComment.Content = comment.Content;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _comments.Remove(comment);
            }
            await Task.CompletedTask;
        }
    }
}
