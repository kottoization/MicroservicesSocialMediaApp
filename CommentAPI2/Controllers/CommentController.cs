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

            comment.Id = Guid.NewGuid();
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

            if (existingComment.UserId != updatedComment.UserId)
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

            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }

}
