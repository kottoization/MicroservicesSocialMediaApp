using PostAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostAPI.Services
{
    public class PostService : IPostService
    {
        private readonly List<Post> _posts = new List<Post>();

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            // W rzeczywistości użyłbyś bazy danych. Tu dla prostoty używamy listy w pamięci.
            return await Task.FromResult(_posts);
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(post);
        }

        public async Task CreateAsync(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _posts.Add(post);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            var existingPost = _posts.FirstOrDefault(p => p.Id == post.Id);
            if (existingPost != null)
            {
                existingPost.Content = post.Content;
                // Zaktualizuj inne pola według potrzeb
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                _posts.Remove(post);
            }
            await Task.CompletedTask;
        }
    }
}
