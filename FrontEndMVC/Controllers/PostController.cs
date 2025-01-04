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
                var posts = await _postApiClient.GetFromJsonAsync<List<PostViewModel>>("/Post");
                return View(posts);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Unable to fetch posts.";
                return View(new List<PostViewModel>());
            }
        }

        public async Task<IActionResult> CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _postApiClient.PostAsJsonAsync("/Post", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error creating post.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Comments(Guid postId)
        {
            try
            {
                var response = await _commentApiClient.GetAsync($"/Comment?postId={postId}");
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

            var response = await _commentApiClient.PostAsJsonAsync("/Comment", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error adding comment.");
            }

            return RedirectToAction("Comments", new { postId = model.PostId });
        }
    }
}
