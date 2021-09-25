using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Product
{
    public class OfficialBrandProductsComponent : ViewComponent
    {
        private readonly IBrandService _brandService;

        public OfficialBrandProductsComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "")
        {
            int take = 20;

            var brands = await _brandService.GetOfficialBrandProductsForShowAsync(pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Product/OfficialBrandProducts.cshtml", brands);
        }
    }
}