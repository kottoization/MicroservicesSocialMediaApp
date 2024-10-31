using Microsoft.AspNetCore.Mvc;

namespace PostAPI.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
