using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostAPI.DTOs;
using PostAPI.Services;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllAsync();
            var postsDto = posts.Select(post => new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = post.UserName,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            }).ToList();

            return Ok(postsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = post.UserName,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };

            return Ok(postDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (createPostDto == null)
            {
                _logger.LogWarning("CreatePostDto is null.");
                return BadRequest();
            }

            // Pobieramy ID użytkownika
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Pobieramy UserName (czyli nick)
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null or empty.");
                return Unauthorized();
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserName = userName,
                Content = createPostDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _postService.CreateAsync(post);
                _logger.LogInformation($"Post created successfully. PostId: {post.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post.");
                return StatusCode(500, "Internal server error");
            }


            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = post.UserName,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, postDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingPost.UserId != currentUserId)
                return Forbid();

            existingPost.Content = updatePostDto.Content;
            await _postService.UpdateAsync(existingPost);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingPost.UserId != currentUserId)
                return Forbid();

            await _postService.DeleteAsync(id);
            return NoContent();
        }
    }
}
