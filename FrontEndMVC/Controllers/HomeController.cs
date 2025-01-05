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
                // Teraz wywo�anie automatycznie do��czy nag��wek Authorization (o ile user jest zalogowany)
                var postsResponse = await _postApiClient.GetAsync("/Post");
                if (!postsResponse.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "B��d pobierania post�w. Kod: " + postsResponse.StatusCode;
                    return View(new List<PostViewModel>());
                }

                var posts = await postsResponse.Content.ReadFromJsonAsync<IEnumerable<PostViewModel>>();
                return View(posts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "Wyst�pi� b��d podczas pobierania post�w.";
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
