using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Image;

namespace Reshop.Web.Components.Image
{
    public class MainPicsComponent : ViewComponent
    {
        private readonly IImageService _imageService;

        public MainPicsComponent(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IViewComponentResult Invoke()
        {
            var images = _imageService.GetImagesOfPlace("MainPics");

            return View("/Views/Shared/Components/Image/MainPics.cshtml", images);
        }
    }
}
