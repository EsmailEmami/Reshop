using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Image;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Image
{
    public class MainPicsComponent : ViewComponent
    {
        private readonly IImageService _imageService;

        public MainPicsComponent(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var images = await _imageService.GetImagesOfPlaceAsync("MainPics");

            return View("/Views/Shared/Components/Image/MainPics.cshtml", images);
        }
    }
}
