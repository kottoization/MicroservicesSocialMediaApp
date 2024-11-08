using Microsoft.AspNetCore.Mvc;
using PostAPI.DTOs;
using PostAPI.Models;
using PostAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllAsync();
            var postsDto = posts.Select(p => MapToPostDto(p)).ToList();
            return Ok(postsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var postDto = MapToPostDto(post);
            return Ok(postDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = MapToPost(createPostDto);
            post.Id = Guid.NewGuid();
            post.UserId = GetCurrentUserId();
            post.CreatedAt = DateTime.UtcNow;

            await _postService.CreateAsync(post);

            var postDto = MapToPostDto(post);
            return CreatedAtAction(nameof(GetPost), new { id = postDto.Id }, postDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            if (existingPost.UserId != GetCurrentUserId())
                return Forbid();

            UpdatePostFromDto(existingPost, updatePostDto);
            await _postService.UpdateAsync(existingPost);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            if (existingPost.UserId != GetCurrentUserId())
                return Forbid();

            await _postService.DeleteAsync(id);
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdString);
        }

        private PostDto MapToPostDto(Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };
        }

        private Post MapToPost(CreatePostDto createPostDto)
        {
            return new Post
            {
                Content = createPostDto.Content
            };
        }

        private void UpdatePostFromDto(Post post, UpdatePostDto updatePostDto)
        {
            post.Content = updatePostDto.Content;
        }
    }
}
