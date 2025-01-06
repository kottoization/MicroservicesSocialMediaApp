using FrontEndMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEndMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _postApiClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _postApiClient = httpClientFactory.CreateClient("PostApi");
        }

        public async Task<IActionResult> Index()
        {
            var postsResponse = await _postApiClient.GetAsync("/Post");
            var posts = postsResponse.IsSuccessStatusCode
                ? await postsResponse.Content.ReadFromJsonAsync<IEnumerable<PostViewModel>>()
                : new List<PostViewModel>();

            return View(posts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}