using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Image;
using Reshop.Domain.Entities.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.Image
{
    public interface IImageService
    {
        Task<ResultTypes> AddImageAsync(Domain.Entities.Image.Image image);
        Task<ResultTypes> EditImageAsync(Domain.Entities.Image.Image image);
        Task<ResultTypes> DeleteImageAsync(string imageId);
        IEnumerable<ImageForShowViewModel> GetImagesForShow();
        IEnumerable<ImagesForShowInSiteViewModel> GetImagesOfPlace(string place);

        Task<Domain.Entities.Image.Image> GetImageByIdAsync(string imageId);

        IEnumerable<ImagePlace> GetImagesPlace();
    }
}
