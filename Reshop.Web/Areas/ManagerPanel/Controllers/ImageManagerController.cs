using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Image;
using Reshop.Domain.DTOs;
using Reshop.Domain.Entities.Image;
using System.IO;
using System.Threading.Tasks;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.Image;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class ImageManagerController : Controller
    {
        #region constructor

        private readonly IImageService _imageService;

        public ImageManagerController(IImageService imageService)
        {
            _imageService = imageService;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var images = await _imageService.GetImagesForShowAsync();

            return View(images);
        }

        #region add image

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddImage()
        {
            return View(new AddOrEditImageViewModel()
            {
                Places = await _imageService.GetImagesPlaceAsync(),
            });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddImage(AddOrEditImageViewModel model)
        {
            model.Places = await _imageService.GetImagesPlaceAsync();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "هنگام افزودن تصویر به مشکل خوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var image = new Image()
            {
                ImageUrl = model.ImageUrl,
                ImagePlaceId = model.SelectedPlace
            };

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "banner");
            string imageName = await ImageConvertor.CreateNewImage(model.Image, path, 3000);


            image.ImageName = imageName;


            var addImage = await _imageService.AddImageAsync(image);

            if (addImage == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام افزودن تصویر به مشکل خوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }


        #endregion

        #region edit image

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditImage(string imageId)
        {
            ViewBag.ImagesPlace = await _imageService.GetImagesPlaceAsync();

            var image = await _imageService.GetImageByIdAsync(imageId);

            if (image == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = new AddOrEditImageViewModel()
            {
                ImageId = image.ImageId,
                ImageUrl = image.ImageUrl,
                SelectedImage = image.ImageName,
                SelectedPlace = image.ImagePlaceId,
                Places = await _imageService.GetImagesPlaceAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditImage(AddOrEditImageViewModel model)
        {
            model.Places = await _imageService.GetImagesPlaceAsync();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var image = await _imageService.GetImageByIdAsync(model.ImageId);

            if (image == null)
            {
                ModelState.AddModelError("", "هنگام ویرایش تصویر به مشکل خوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            image.ImagePlaceId = model.SelectedPlace;
            image.ImageUrl = model.ImageUrl;

            if (image.ImageName != model.SelectedImage)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "banner");

                ImageConvertor.DeleteImage($"{path}/{image.ImageName}");

                string imageName = await ImageConvertor.CreateNewImage(model.Image, path, 3000);

                image.ImageName = imageName;
            }

            var editImage = await _imageService.EditImageAsync(image);

            if (editImage == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام ویرایش تصویر به مشکل خوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region deleteImage

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            var delete = await _imageService.DeleteImageAsync(imageId);

            if (delete == ResultTypes.Successful)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion
    }
}
