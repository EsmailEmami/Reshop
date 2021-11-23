using Reshop.Application.Interfaces.Image;
using Reshop.Domain.Interfaces.Image;

namespace Reshop.Application.Services.Image
{
    public class ImageService : IImageService
    {
        #region constructor

        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        #endregion
    }
}
