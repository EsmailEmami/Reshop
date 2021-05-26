using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CityManagerController : Controller
    {
        #region constructor

        private readonly IStateService _stateService;

        public CityManagerController(IStateService stateService)
        {
            _stateService = stateService;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            return View(_stateService.GetCities());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCity(int cityId = 0)
        {
            if (cityId == 0)
            {

                return View(new City() { CityId = 0 });
            }
            else
            {
                var city = await _stateService.GetCityByIdAsync(cityId);
                if (city is null) return NotFound();

                return View(city);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCity(City model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CityId == 0)
            {
                var city = new City()
                {
                    CityName = model.CityName,
                };


                var result = await _stateService.AddCityAsync(city);

                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ثبت استان جدید با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
            else
            {
                var city = await _stateService.GetCityByIdAsync(model.CityId);
                if (city is null) return NotFound();


                city.CityName = model.CityName;

                var result = await _stateService.EditCityAsync(city);

                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
        }


        public async Task<IActionResult> DeleteCity(int cityId)
        {
            try
            {
                var result = await _stateService.RemoveCityAsync(cityId);
                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));


                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
