using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
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
            return View(_stateService.GetStates());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditState(int stateId = 0)
        {
            if (stateId == 0)
            {
                var model = new AddOrEditStateViewModel()
                {
                    Cities = _stateService.GetCities() as IEnumerable<City>,
                };

                return View(model);
            }
            else
            {
                var state = await _stateService.GetStateDataForEditAsync(stateId);
                if (state is null) return NotFound();

                return View(state);
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

                var result = await _stateService.AddStateAsync(state);

                if (result == ResultTypes.Successful)
                {
                    if (model.SelectedCities is not null)
                    {
                        foreach (var cityId in model.SelectedCities)
                        {
                            var stateCity = new StateCity()
                            {
                                StateId = state.StateId,
                                CityId = cityId
                            };

                            await _stateService.AddStateCityAsync(stateCity);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ثبت استان جدید با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
            else
            {
                var state = await _stateService.GetStateByIdAsync(model.StateId);
                if (state is null) return NotFound();

                state.StateName = model.StateName;

                await _stateService.RemoveCitiesOfStateAsync(state.StateId);

                if (model.SelectedCities is not null)
                {
                    foreach (var cityId in model.SelectedCities)
                    {
                        var stateCity = new StateCity()
                        {
                            StateId = state.StateId,
                            CityId = cityId
                        };

                        await _stateService.AddStateCityAsync(stateCity);
                    }
                }

              

                var result = await _stateService.EditStateAsync(state);

                if (result == ResultTypes.Successful)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش استان با مشکلی غیر منتظره مواجه شدیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteState(int stateId)
        {
            try
            {
                var result = await _stateService.RemoveStateAsync(stateId);
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
