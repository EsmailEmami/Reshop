using Reshop.Application.Enums;
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
        Task<IEnumerable<Domain.Entities.Image.Image>> GetImagesAsync();
        Task<Domain.Entities.Image.Image> GetImageByIdAsync(string imageId);

        Task<IEnumerable<ImagePlace>> GetImagesPlaceAsync();
    }
}
