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
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        /* [HttpGet]
          public async Task<IActionResult> GetPosts()
          {
              var posts = await _postService.GetAllAsync();
              var postsDto = posts.Select(p => MapToPostDto(p)).ToList();
              return Ok(postsDto);
          }*/
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _postService.GetAllAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching posts.", Details = ex.Message });
            }
        }

       // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            try
            {
                var post = await _postService.GetByIdAsync(id);
                if (post == null)
                    return NotFound(new { Message = "Post not found." });

                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the post.", Details = ex.Message });
            }
        }


        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var postDto = MapToPostDto(post);
            return Ok(postDto);
        }*/


        // tmp do usunięcia lub zmiany
       // [Authorize]
        [HttpPost("manual")]
        public async Task<IActionResult> CreatePostManual(string content, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required");
            try
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Content = content,
                    CreatedAt = DateTime.UtcNow
                };

                await _postService.CreateAsync(post);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the post.", Details = ex.Message });

            }
        }
       // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] Post updatedPost)
        {
            try
            {
                var existingPost = await _postService.GetByIdAsync(id);
                if (existingPost == null)
                    return NotFound();

                if (existingPost.UserId != updatedPost.UserId)
                    return Forbid();

                existingPost.Content = updatedPost.Content;
                await _postService.UpdateAsync(existingPost);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the post.", Details = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var existingPost = await _postService.GetByIdAsync(id);
                if (existingPost == null)
                    return NotFound();

                await _postService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the post.", Details = ex.Message });
            }
        }

        /*
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
        }*/
        /*
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
        }*/
    }
}

