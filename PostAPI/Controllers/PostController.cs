using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostAPI.DTOs;
using SharedModels.Models;
using PostAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _postService.GetAllAsync();
                var postsDto = posts.Select(post => new PostDto
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt
                }).ToList();
                return Ok(postsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching posts.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            try
            {
                var post = await _postService.GetByIdAsync(id);
                if (post == null)
                    return NotFound(new { Message = "Post not found." });

                var postDto = new PostDto
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt
                };

                return Ok(postDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the post.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (createPostDto == null)
                return BadRequest();

            try
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                    Content = createPostDto.Content,
                    CreatedAt = DateTime.UtcNow
                };

                await _postService.CreateAsync(post);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, new PostDto
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the post.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            try
            {
                var existingPost = await _postService.GetByIdAsync(id);
                if (existingPost == null)
                    return NotFound();

                if (existingPost.UserId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                    return Forbid();

                existingPost.Content = updatePostDto.Content;
                await _postService.UpdateAsync(existingPost);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the post.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var existingPost = await _postService.GetByIdAsync(id);
                if (existingPost == null)
                    return NotFound();

                if (existingPost.UserId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                    return Forbid();

                await _postService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the post.", Details = ex.Message });
            }
        }
    }
}
