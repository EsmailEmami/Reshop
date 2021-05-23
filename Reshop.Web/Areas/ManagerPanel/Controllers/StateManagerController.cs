using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class StateManagerController : Controller
    {
        #region constructor

        private readonly IStateService _stateService;

        public StateManagerController(IStateService stateService)
        {
            _stateService = stateService;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditState(int stateId = 0)
        {
            if (stateId == 0)
            {
                var model = new AddOrEditStateViewModel()
                {
                    Cities = _stateService.GetCities(),
                };

                return View(model);
            }
            else
            {
                var state = await _stateService.GetStateWithCitiesByIdAsync(stateId);
                if (state is null) return NotFound();

                var model = new AddOrEditStateViewModel()
                {
                    StateId = state.StateId,
                    StateName = state.StateName,
                    Cities = state.Cities as IAsyncEnumerable<City>
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditState(AddOrEditStateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.StateId == 0)
            {
                var state = new State()
                {
                    StateName = model.StateName,
                };

                foreach (var city in model.SelectedCities)
                {
                    state.Cities.Add(city);
                }

                var result = await _stateService.AddStateAsync(state);

                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ثبت استان جدید با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
            else
            {
                var state = await _stateService.GetStateWithCitiesByIdAsync(model.StateId);
                if (state is null) return NotFound();

                if (model.SelectedCities is not null)
                {
                    foreach (var city in state.Cities)
                    {
                        await _stateService.RemoveCityAsync(city);
                    }

                    foreach (var city in model.SelectedCities)
                    {
                        state.Cities.Add(city);
                    }
                }

                state.StateName = model.StateName;

                var result = await _stateService.EditStateAsync(state);

                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
        }
    }
}
