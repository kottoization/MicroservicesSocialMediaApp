using FrontEndMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Json;

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
            try
            {
                // Teraz wywo³anie automatycznie do³¹czy nag³ówek Authorization (o ile user jest zalogowany)
                var postsResponse = await _postApiClient.GetAsync("/Post");
                if (!postsResponse.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "B³¹d pobierania postów. Kod: " + postsResponse.StatusCode;
                    return View(new List<PostViewModel>());
                }

                var posts = await postsResponse.Content.ReadFromJsonAsync<IEnumerable<PostViewModel>>();
                return View(posts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "Wyst¹pi³ b³¹d podczas pobierania postów.";
                return View(new List<PostViewModel>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
