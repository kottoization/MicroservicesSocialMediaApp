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
    [Authorize] // Wymaga autoryzacji JWT dla całego kontrolera
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [AllowAnonymous] // Pozwalamy na pobieranie wszystkich komentarzy bez autoryzacji
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Pozwalamy na pobieranie pojedynczego komentarza bez autoryzacji
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

            // Pobieramy UserId z tokenu JWT
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            comment.Id = Guid.NewGuid();
            comment.UserId = userId; // Ustawiamy UserId na podstawie tokenu JWT
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

            // Pobieramy UserId z tokenu JWT
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (existingComment.UserId != userId)
                return Forbid(); // Użytkownik może edytować tylko swoje komentarze

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

            // Pobieramy UserId z tokenu JWT
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (existingComment.UserId != userId)
                return Forbid(); // Użytkownik może usuwać tylko swoje komentarze

            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
