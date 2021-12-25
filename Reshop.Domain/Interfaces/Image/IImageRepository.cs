using Reshop.Domain.DTOs.Image;
using Reshop.Domain.Entities.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Image
{
    public interface IImageRepository
    {
        Task AddImageAsync(Entities.Image.Image image);
        void UpdateImage(Entities.Image.Image image);
        void RemoveImage(Entities.Image.Image image);
        Task<Entities.Image.Image> GetImageByIdAsync(string imageId);
        IEnumerable<ImageForShowViewModel> GetImagesForShow();
        IEnumerable<ImagesForShowInSiteViewModel> GetImagesOfPlace(string place);



        IEnumerable<ImagePlace> GetImagesPlace();



        Task SaveChangesAsync();
    }
}