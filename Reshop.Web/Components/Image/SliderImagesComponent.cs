using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Image;

namespace Reshop.Web.Components.Image
{
    public class SliderImagesComponent : ViewComponent
    {
        private readonly IImageService _imageService;

        public SliderImagesComponent(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IViewComponentResult Invoke()
        {
            var images = _imageService.GetImagesOfPlace("SliderMenu");

            return View("/Views/Shared/Components/Image/SliderImages.cshtml", images);
        }
    }
}
