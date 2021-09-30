using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Product
{
    public class ColorsListComponent : ViewComponent
    {
        private readonly IColorService _colorService;

        public ColorsListComponent(IColorService colorService)
        {
            _colorService = colorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "")
        {
            int take = 20;

            var colors = await _colorService.GetColorsWithPaginationAsync(pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Product/ColorsList.cshtml", colors);
        }
    }
}