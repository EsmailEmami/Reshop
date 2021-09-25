using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Product
{
    public class BrandsListComponent : ViewComponent
    {
        private readonly IBrandService _brandService;

        public BrandsListComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "")
        {
            int take = 20;

            var brands = await _brandService.GetBrandsForShowAsync(pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Product/BrandsList.cshtml", brands);
        }
    }
}
