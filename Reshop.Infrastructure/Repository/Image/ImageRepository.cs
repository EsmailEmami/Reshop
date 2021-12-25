using Reshop.Domain.DTOs.Image;
using Reshop.Domain.Entities.Image;
using Reshop.Domain.Interfaces.Image;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly ReshopDbContext _context;

        public ImageRepository(ReshopDbContext context)
        {
            _context = context;
        }

        public async Task AddImageAsync(Domain.Entities.Image.Image image) =>
            await _context.Images.AddAsync(image);

        public void UpdateImage(Domain.Entities.Image.Image image) =>
            _context.Images.Update(image);

        public void RemoveImage(Domain.Entities.Image.Image image) =>
            _context.Images.Remove(image);

        public async Task<Domain.Entities.Image.Image> GetImageByIdAsync(string imageId) =>
            await _context.Images.FindAsync(imageId);

        public IEnumerable<ImageForShowViewModel> GetImagesForShow() =>
             _context.Images.Select(c => new ImageForShowViewModel()
             {
                 ImageId = c.ImageId,
                 ImageName = c.ImageName,
                 PlaceName = c.ImagePlace.Place,
                 Url = c.ImageUrl,
             });

        public IEnumerable<ImagesForShowInSiteViewModel> GetImagesOfPlace(string place) =>
             _context.ImagesPlace.Where(c => c.Place == place)
                .SelectMany(c => c.Images)
                .Select(c => new ImagesForShowInSiteViewModel()
                {
                    Url = c.ImageUrl,
                    ImageName = c.ImageName,
                });

        public IEnumerable<ImagePlace> GetImagesPlace() =>
             _context.ImagesPlace;

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
