using Reshop.Application.Enums;
using Reshop.Domain.Entities.Image;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Image;

namespace Reshop.Application.Interfaces.Image
{
    public interface IImageService
    {
        Task<ResultTypes> AddImageAsync(Domain.Entities.Image.Image image);
        Task<ResultTypes> EditImageAsync(Domain.Entities.Image.Image image);
        Task<ResultTypes> DeleteImageAsync(string imageId);
        Task<IEnumerable<ImageForShowViewModel>> GetImagesForShowAsync();
        Task<IEnumerable<ImagesForShowInSiteViewModel>> GetImagesOfPlaceAsync(string place);

        Task<Domain.Entities.Image.Image> GetImageByIdAsync(string imageId);

        Task<IEnumerable<ImagePlace>> GetImagesPlaceAsync();
    }
}
