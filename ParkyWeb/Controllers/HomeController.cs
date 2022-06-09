using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _NPRepository;
        private readonly ITrailsRepository _TrRepository;
        private readonly IAccountRepository _AccountRepository;
        public HomeController(ILogger<HomeController> logger,
            INationalParkRepository nationalParkRepository,
            ITrailsRepository trailsRepository, IAccountRepository accountRepository)
        {
            _logger = logger;
            _NPRepository = nationalParkRepository;
            _TrRepository = trailsRepository;
            _AccountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                NationalParks = await _NPRepository.GetAllAsync(SD.NationalParksApiPath, HttpContext.Session.GetString("JwtToken")),
                Trails = await _TrRepository.GetAllAsync(SD.TrailsApiPath, HttpContext.Session.GetString("JwtToken"))
            };
            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            var User = new User();
            return View(User);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var UserObj = await _AccountRepository.LoginAsync(SD.UsersApiPath + "Authenticate/", user);
            if (UserObj.Token == null)
                return View();

            var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            Identity.AddClaim(new Claim(ClaimTypes.Name, UserObj.Username));
            Identity.AddClaim(new Claim(ClaimTypes.Role, UserObj.Role));
            var Principal = new ClaimsPrincipal(Identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
            HttpContext.Session.SetString("JwtToken", UserObj.Token);
            TempData["alert"] = "Welcome " + UserObj.Username;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            var Result = await _AccountRepository.RegisterAsync(SD.UsersApiPath + "Register/", user);
            if (Result == false)
            {
                return View();
            }
            TempData["alert"] = "Registration is success Welcome " + user.Username;
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JwtToken", "");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
