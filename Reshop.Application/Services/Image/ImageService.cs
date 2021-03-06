using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Image;
using Reshop.Domain.DTOs.Image;
using Reshop.Domain.Entities.Image;
using Reshop.Domain.Interfaces.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<ResultTypes> AddImageAsync(Domain.Entities.Image.Image image)
        {
            try
            {
                await _imageRepository.AddImageAsync(image);
                await _imageRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditImageAsync(Domain.Entities.Image.Image image)
        {
            try
            {
                _imageRepository.UpdateImage(image);
                await _imageRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteImageAsync(string imageId)
        {
            try
            {
                var image = await _imageRepository.GetImageByIdAsync(imageId);

                if (image == null)
                    return ResultTypes.Failed;

                _imageRepository.RemoveImage(image);

                await _imageRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<ImageForShowViewModel> GetImagesForShow() =>
            _imageRepository.GetImagesForShow();

        public IEnumerable<ImagesForShowInSiteViewModel> GetImagesOfPlace(string place) =>
            _imageRepository.GetImagesOfPlace(place);

        public async Task<Domain.Entities.Image.Image> GetImageByIdAsync(string imageId) =>
            await _imageRepository.GetImageByIdAsync(imageId);

        public IEnumerable<ImagePlace> GetImagesPlace() =>
            _imageRepository.GetImagesPlace();
    }
}
