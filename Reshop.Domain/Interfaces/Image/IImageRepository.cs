using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Image;
using Reshop.Domain.Entities.Image;

namespace Reshop.Domain.Interfaces.Image
{
    public interface IImageRepository
    {
        Task AddImageAsync(Entities.Image.Image image);
        void UpdateImage(Entities.Image.Image image);
        void RemoveImage(Entities.Image.Image image);
        Task<Entities.Image.Image> GetImageByIdAsync(string imageId);
        Task<IEnumerable<ImageForShowViewModel>> GetImagesForShowAsync();
        Task<IEnumerable<ImagesForShowInSiteViewModel>> GetImagesOfPlaceAsync(string place);



        Task<IEnumerable<ImagePlace>> GetImagesPlaceAsync();



        Task SaveChangesAsync();
    }
}