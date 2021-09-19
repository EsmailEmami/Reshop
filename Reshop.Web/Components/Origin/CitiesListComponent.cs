using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Origin
{
    public class CitiesListComponent : ViewComponent
    {
        private readonly IOriginService _originService;

        public CitiesListComponent(IOriginService originService)
        {
            _originService = originService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "", List<int> states = null)
        {
            int take = 15;

            var cities = await _originService.GetCitiesWithPaginationAsync(pageId, take, filter, states);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;
            ViewBag.SelectedStates = states;

            ViewBag.States = _originService.GetStates();

            return View("/Views/Shared/Components/Origin/CitiesList.cshtml", cities);
        }
    }
}
