using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Category;

namespace Reshop.Web.Components.Dropdown
{
    public class DropDownComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public DropDownComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.GetCategoriesDropdown();
            return View("DropDown", categories);
        }
    }
}
