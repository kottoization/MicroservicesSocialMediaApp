using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using CommentAPI2.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Teraz wszystkie metody wymagają autoryzacji
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            // Możemy ewentualnie dodać param postId, ale w tym kodzie to jest prosty GET wszystkich komentarzy
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
                return BadRequest();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.Identity.Name;

            comment.Id = Guid.NewGuid();
            comment.UserId = userId;
            comment.UserName = userName;
            comment.CreatedAt = DateTime.UtcNow;

            await _commentService.CreateAsync(comment);
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] Comment updatedComment)
        {
            var existingComment = await _commentService.GetByIdAsync(id);
            if (existingComment == null)
                return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingComment.UserId != currentUserId)
                return Forbid();

            existingComment.Content = updatedComment.Content;
            await _commentService.UpdateAsync(existingComment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var existingComment = await _commentService.GetByIdAsync(id);
            if (existingComment == null)
                return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingComment.UserId != currentUserId)
                return Forbid();

            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
