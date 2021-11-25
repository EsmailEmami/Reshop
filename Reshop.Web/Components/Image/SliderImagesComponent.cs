using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var images = await _imageService.GetImagesOfPlaceAsync("SliderMenu");

            return View("/Views/Shared/Components/Image/SliderImages.cshtml", images);
        }
    }
}
