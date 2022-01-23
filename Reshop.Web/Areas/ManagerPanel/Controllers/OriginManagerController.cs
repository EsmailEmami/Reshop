using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Constants;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.Entities.User;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class OriginManagerController : Controller
    {
        #region constructor

        private readonly IOriginService _stateService;
        private readonly IProductService _productService;

        public OriginManagerController(IOriginService stateService, IProductService productService)
        {
            _stateService = stateService;
            _productService = productService;
        }

        #endregion


        [HttpGet]
        [Permission(PermissionsConstants.StatesPage)]
        public IActionResult ManageStates()
        {
            return View();
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult StatesList(int pageId, string filter)
        {
            return ViewComponent("StatesListComponent", new { pageId, filter });
        }

        [HttpGet]
        [Permission(PermissionsConstants.CitiesPage)]
        public IActionResult ManageCities()
        {
            return View();
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult CitiesList(int pageId, string filter, string[] states = null)
        {
            var selectedStates = states.ArrayToListInt();

            if (selectedStates == null)
                return NotFound();


            return ViewComponent("CitiesListComponent", new { pageId, filter, states = selectedStates });
        }



        #region add or edit state

        [HttpGet]
        [NoDirectAccess]
        [PermissionJs(PermissionsConstants.AddState, PermissionsConstants.EditState)]
        public async Task<IActionResult> AddOrEditState(int stateId = 0)
        {
            if (stateId == 0)
            {
                return View(new State());
            }
            else
            {
                var state = await _stateService.GetStateByIdAsync(stateId);
                if (state == null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

                return View(state);
            }
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs(PermissionsConstants.AddState, PermissionsConstants.EditState)]
        public async Task<IActionResult> AddOrEditState(State model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (model.StateId == 0)
            {
                var state = new State()
                {
                    StateName = model.StateName,
                };

                var result = await _stateService.AddStateAsync(state);

                if (result == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ثبت استان جدید با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
            else
            {
                var state = await _stateService.GetStateByIdAsync(model.StateId);
                if (state == null)
                {
                    ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }

                state.StateName = model.StateName;

                var result = await _stateService.EditStateAsync(state);
                if (result == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        [HttpPost]
        [Permission(PermissionsConstants.DeleteState)]
        public async Task<IActionResult> DeleteState(int stateId)
        {
            try
            {
                var result = await _stateService.RemoveStateAsync(stateId);
                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(ManageStates));


                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        #region add or edit city

        [HttpGet]
        [NoDirectAccess]
        [PermissionJs(PermissionsConstants.AddCity, PermissionsConstants.EditCity)]
        public async Task<IActionResult> AddOrEditCity(int cityId = 0)
        {
            ViewBag.States = _stateService.GetStates();

            if (cityId == 0)
            {
                return View(new City());
            }
            else
            {
                var city = await _stateService.GetCityByIdAsync(cityId);
                if (city == null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

                return View(city);
            }
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs(PermissionsConstants.AddCity, PermissionsConstants.EditCity)]
        public async Task<IActionResult> AddOrEditCity(City model)
        {
            ViewBag.States = _stateService.GetStates();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (model.CityId == 0)
            {
                var city = new City()
                {
                    CityName = model.CityName,
                    StateId = model.StateId
                };


                var result = await _stateService.AddCityAsync(city);

                if (result == ResultTypes.Successful)
                    return Json(new { isValid = true, returnUrl = "current" });

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ثبت شهر جدید با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
            else
            {
                var city = await _stateService.GetCityByIdAsync(model.CityId);
                if (city == null)
                {
                    ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش شهر با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }

                city.CityName = model.CityName;
                city.StateId = model.StateId;

                var result = await _stateService.EditCityAsync(city);

                if (result == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs(PermissionsConstants.DeleteCity)]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            try
            {
                var result = await _stateService.RemoveCityAsync(cityId);
                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(ManageCities));


                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}