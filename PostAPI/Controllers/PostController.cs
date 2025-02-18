﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostAPI.DTOs;
using PostAPI.Services;
using SharedModels.Models;
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
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllAsync();
            var postsDto = new List<PostDto>();

            foreach (var post in posts)
            {
                var userName = await _postService.GetUserNameById(post.UserId); // Pobranie nazwy użytkownika
                postsDto.Add(new PostDto
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    UserName = userName,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    CommentsCount = post.Comments.Count // Liczba komentarzy
                });
            }

            return Ok(postsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var userName = await _postService.GetUserNameById(post.UserId); // Pobranie nazwy użytkownika

            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = userName,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments?.Count ?? 0 // Liczba komentarzy
            };

            return Ok(postDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            Console.WriteLine($"\nAuthorization Header: {Request.Headers["Authorization"]}\n");
            if (createPostDto == null)
                return BadRequest();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Unauthorized: User ID is missing or invalid.");
                return Unauthorized("User ID is missing or invalid.");
            }

            Console.WriteLine($"User ID: {userId}");

            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Content = createPostDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _postService.CreateAsync(post);

            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
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

            if (existingPost.UserId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
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

            if (existingPost.UserId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            await _postService.DeleteAsync(id);
            return NoContent();
        }
    }
}
