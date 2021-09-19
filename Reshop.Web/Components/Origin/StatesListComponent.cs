using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.Components.Origin
{
    public class StatesListComponent : ViewComponent
    {
        private readonly IOriginService _originService;

        public StatesListComponent(IOriginService originService)
        {
            _originService = originService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "")
        {
            int take = 15;

            var states = await _originService.GetStatesWithPaginationAsync(pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Origin/StatesList.cshtml", states);
        }
    }
}