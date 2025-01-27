using FrontEndMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FrontEndMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly HttpClient _postApiClient;
        private readonly HttpClient _commentApiClient;

        public PostController(IHttpClientFactory httpClientFactory)
        {
            _postApiClient = httpClientFactory.CreateClient("PostApi");
            _commentApiClient = httpClientFactory.CreateClient("CommentApi");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postApiClient.GetFromJsonAsync<List<PostViewModel>>("/api/Post");
            
                return View(posts);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Unable to fetch posts.";
                return View(new List<PostViewModel>());
            }
        }

        public async Task<IActionResult> CreatePostForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {

            Console.WriteLine($"\nRequest URL: {_postApiClient.BaseAddress}Post\n");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
                return View("CreatePostForm", model);
            }

            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    ModelState.AddModelError("", "User is not authenticated.");
            //    return View("CreatePostForm", model);
            //}

            //var post = new PostViewModel
            //{
            //    Id = Guid.NewGuid(),
            //    UserId = userId,
            //    Content = model.Content,
            //    CreatedAt = DateTime.UtcNow,
            //    Comments = new List<CommentViewModel>() // Domyślna pusta lista
            //};


            var response = await _postApiClient.PostAsJsonAsync("/api/Post", new { Content = model.Content });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error creating post: {error}");
                return View("CreatePostForm", model);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Comments(Guid postId)
        {
            try
            {
                var response = await _commentApiClient.GetAsync($"/api/Comment?postId={postId}");
                var comments = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<IEnumerable<CommentViewModel>>()
                    : new List<CommentViewModel>();

                ViewBag.PostId = postId;
                return View(comments);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Unable to fetch comments.";
                return View(new List<CommentViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Comments", new { postId = model.PostId });

            var response = await _commentApiClient.PostAsJsonAsync("/api/Comment", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error adding comment.");
            }

            return RedirectToAction("Comments", new { postId = model.PostId });
        }
    }
}
