using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostAPI.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            // not optimal but ok for small DB
            var posts = await _dbContext.Posts
                .Select(post => new Post
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                })
                .ToListAsync();

            var comments = await _dbContext.Comments.ToListAsync();

            foreach (var post in posts)
            {
                post.Comments = comments
                    .Where(comment => comment.PostId == post.Id)
                    .ToList(); 
            }

            return posts;
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            var post = await _dbContext.Posts
                .Where(p => p.Id == id)
                .Select(p => new Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                })
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return null;
            }

            post.Comments = await _dbContext.Comments
                .Where(comment => comment.PostId == id)
                .ToListAsync();

            return post;
        }

        public async Task CreateAsync(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            var existingPost = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existingPost != null)
            {
                existingPost.Content = post.Content;
                // Update other properties if needed
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserName ?? "Unknown User";
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
