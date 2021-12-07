using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Category;

namespace Reshop.Web.Components.Dropdown
{
    public class MobileMenuComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public MobileMenuComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.GetCategoriesMobileMenu();
            return View("/Views/Shared/Components/DropDownComponent/MobileMenu.cshtml", categories);
        }
    }
}
