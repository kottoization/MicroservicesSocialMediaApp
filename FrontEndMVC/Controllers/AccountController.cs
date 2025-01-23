using FrontEndMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SharedModels.Models.Dto;
using System.Net.Http.Headers;


namespace FrontEndMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _identityApiClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _identityApiClient = httpClientFactory.CreateClient("IdentityApi");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _identityApiClient.PostAsJsonAsync("/User/login", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var user = await response.Content.ReadFromJsonAsync<UserDto>();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid response from server.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWT", user.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            //TODO: sprawdzic jak przekazywac przez front to JWT, byc moze ponizsza linijke trzeba bedzie dodac i skonfigurowac
            //_postApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _identityApiClient.PostAsJsonAsync("/User/register", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
