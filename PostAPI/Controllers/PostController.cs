using Microsoft.AspNetCore.Mvc;
using PostAPI.DTOs;
using PostAPI.Models;
using PostAPI.Services;

namespace PostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        //TODO: MAPPER CO JEST XDDD


        public PostController(IPostService postService/*, IMapper mapper*/)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllAsync();
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(postsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var postDto = _mapper.Map<PostDto>(post);
            return Ok(postDto);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _mapper.Map<Post>(createPostDto);
            post.Id = Guid.NewGuid();
            post.UserId = GetCurrentUserId(); // Pobierz ID zalogowanego użytkownika
            post.CreatedAt = DateTime.UtcNow;

            await _postService.CreateAsync(post);

            var postDto = _mapper.Map<PostDto>(post);
            return CreatedAtAction(nameof(GetPost), new { id = postDto.Id }, postDto);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            // Sprawdź, czy użytkownik jest właścicielem posta
            if (existingPost.UserId != GetCurrentUserId())
                return Forbid();

            _mapper.Map(updatePostDto, existingPost);
            await _postService.UpdateAsync(existingPost);

            return NoContent();
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null)
                return NotFound();

            // Sprawdź, czy użytkownik jest właścicielem posta
            if (existingPost.UserId != GetCurrentUserId())
                return Forbid();

            await _postService.DeleteAsync(id);
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            // Pobierz ID zalogowanego użytkownika z tokena JWT
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdString);
        }
    }
}
