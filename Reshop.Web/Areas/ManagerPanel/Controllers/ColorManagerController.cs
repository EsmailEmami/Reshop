using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.Entities.Product;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class ColorManagerController : Controller
    {
        private readonly IColorService _colorService;

        public ColorManagerController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult ColorsList(int pageId = 1, string filter = "")
        {
            return ViewComponent("ColorsListComponent", new { pageId, filter });
        }

        #region add

        [HttpGet]
        [NoDirectAccess]
        public IActionResult AddColor()
        {
            return View(new Color());
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddColor(Color model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var color = new Color()
            {
                ColorName = model.ColorName,
                ColorCode = model.ColorCode
            };

            var addBrand = await _colorService.AddColorAsync(color);

            if (addBrand == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام افزودن برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region edit

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditColor(int colorId)
        {
            if (colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var color = await _colorService.GetColorByIdAsync(colorId);

            if (color == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(color);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditColor(Color model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var color = await _colorService.GetColorByIdAsync(model.ColorId);

            if (color == null)
            {
                ModelState.AddModelError("", "هنگام ویرایش نام اختصاصی کالا به مشکل خوردیم");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            color.ColorName = model.ColorName;
            color.ColorCode = model.ColorCode;
            
            var edit = await _colorService.EditColorAsync(color);

            if (edit == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام ویرایش نام اختصاصی کالا به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion
    }
}