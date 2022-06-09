using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.IO;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _NPRepository;
        public NationalParksController(INationalParkRepository NPRepository)
        {
            _NPRepository = NPRepository;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? Id)
        {
            var NationalPark = new NationalPark();
            if (Id == null)
            {
                return View(NationalPark);
            }
            NationalPark = await _NPRepository.GetAsync(SD.NationalParksApiPath, Id.GetValueOrDefault(), HttpContext.Session.GetString("JwtToken"));
            if (NationalPark == null)
                return NotFound();
            return View(NationalPark);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark NationalPark)
        {
            if (ModelState.IsValid)
            {
                var Files = HttpContext.Request.Form.Files;
                if (Files.Count > 0)
                {
                    byte[] P1 = null;
                    using (var Fs1 = Files[0].OpenReadStream())
                    {
                        using (var Ms1 = new MemoryStream())
                        {
                            Fs1.CopyTo(Ms1);
                            P1 = Ms1.ToArray();
                        }
                    }
                    NationalPark.Picture = P1;
                }
                else
                {
                    var NationalParkDb = await _NPRepository.GetAsync(SD.NationalParksApiPath, NationalPark.Id, HttpContext.Session.GetString("JwtToken"));
                    NationalPark.Picture = NationalParkDb.Picture;
                }
                if (NationalPark.Id == 0)
                {
                    await _NPRepository.CreateAsync(SD.NationalParksApiPath, NationalPark, HttpContext.Session.GetString("JwtToken"));
                }
                else
                {
                    await _NPRepository.UpdateAsync(SD.NationalParksApiPath + NationalPark.Id, NationalPark, HttpContext.Session.GetString("JwtToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(NationalPark);
        }
        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { Data = await _NPRepository.GetAllAsync(SD.NationalParksApiPath, HttpContext.Session.GetString("JwtToken")) });
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var Status = await _NPRepository.DeleteAsync(SD.NationalParksApiPath, Id, HttpContext.Session.GetString("JwtToken"));
            if (Status)
            {
                return Json(new { success = true, message = "Deleted Successful" });
            }
            return Json(new { success = false, message = "Deleted Not Successful" });
        }
    }
}
