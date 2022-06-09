using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly ITrailsRepository _TrRepository;
        private readonly INationalParkRepository _NPRepoitory;
        public TrailsController(INationalParkRepository NPRepoitory, ITrailsRepository TrRepository)
        {
            _TrRepository = TrRepository;
            _NPRepoitory = NPRepoitory;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }
        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _TrRepository.GetAllAsync(SD.TrailsApiPath, HttpContext.Session.GetString("JwtToken")) });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? Id)
        {
            var NPList = await _NPRepoitory.GetAllAsync(SD.NationalParksApiPath, HttpContext.Session.GetString("JwtToken"));
            var TrViewModel = new TrailsViewModel()
            {
                NationalParks = NPList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };
            if (Id == null)
            {
                return View(TrViewModel);
            }
            TrViewModel.Trail = await _TrRepository.GetAsync(SD.TrailsApiPath, Id.GetValueOrDefault(), HttpContext.Session.GetString("JwtToken"));
            if (TrViewModel == null)
            {
                return NotFound();
            }
            else
            {
                //TrViewModel.Trail = await _TrRepository.GetAsync(SD.TrailsApiPath, Id.GetValueOrDefault());
                return View(TrViewModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Trail.Id == 0)
                {
                    await _TrRepository.CreateAsync(SD.TrailsApiPath, viewModel.Trail, HttpContext.Session.GetString("JwtToken"));
                }
                else
                {
                    await _TrRepository.UpdateAsync(SD.TrailsApiPath + viewModel.Trail.Id, viewModel.Trail, HttpContext.Session.GetString("JwtToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var NPList = await _NPRepoitory.GetAllAsync(SD.NationalParksApiPath, HttpContext.Session.GetString("JwtToken"));
                var TrViewModel = new TrailsViewModel()
                {
                    NationalParks = NPList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = viewModel.Trail
                };
                return View(TrViewModel);
            }

        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var Status = await _TrRepository.DeleteAsync(SD.TrailsApiPath, Id, HttpContext.Session.GetString("JwtToken"));
            if (Status == true)
            {
                return Json(new { Success = true, message = "Deleted Successful" });
            }
            else
                return Json(new { Success = false, message = "Deleted Unsuccessful" });
        }
    }
}
