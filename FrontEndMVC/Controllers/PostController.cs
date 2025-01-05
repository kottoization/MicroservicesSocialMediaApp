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
        private readonly ILogger<PostController> _logger;

        public PostController(IHttpClientFactory httpClientFactory, ILogger<PostController> logger)
        {
            _postApiClient = httpClientFactory.CreateClient("PostApi");
            _commentApiClient = httpClientFactory.CreateClient("CommentApi");
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postApiClient.GetFromJsonAsync<List<PostViewModel>>("/Post");
                return View(posts);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Unable to fetch posts.";
                return View(new List<PostViewModel>());
            }
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Wysyłamy do PostAPI. Pamiętaj, że PostAPI w [HttpPost] przyjmuje CreatePostDto,
            // a my wysyłamy PostViewModel. Zawiera on "Content". To wystarczy, jeśli klucze nazw
            // się pokrywają. 

            _logger.LogInformation("Attempting to create a new post.");

            var response = await _postApiClient.PostAsJsonAsync("/Post", new
            {
                Content = model.Content
            });

            _logger.LogInformation($"PostAPI responded with status code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Error creating post: {response.StatusCode} - {errorContent}");

                ModelState.AddModelError("", "Error creating post.");
                return View(model);
            }

            _logger.LogInformation("Post created successfully.");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Comments(Guid postId)
        {
            try
            {
                // Komentarze pobieramy z CommentAPI – natomiast w oryginale w CommentController 
                // nie było filtra "postId". Można to zrobić analogicznie, ale tu przyjmujemy,
                // że i tak pobiera wszystkie. Ewentualnie zmień w CommentController GET, by przyjmował postId.
                var response = await _commentApiClient.GetAsync($"/Comment");
                var comments = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<IEnumerable<CommentViewModel>>()
                    : new List<CommentViewModel>();

                comments = comments.Where(c => c.PostId == postId).ToList();

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

            var response = await _commentApiClient.PostAsJsonAsync("/Comment", new
            {
                PostId = model.PostId,
                Content = model.Content
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error adding comment.");
            }

            return RedirectToAction("Comments", new { postId = model.PostId });
        }
    }
}
