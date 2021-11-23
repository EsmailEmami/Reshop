using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Image;
using Reshop.Domain.Interfaces.Image;
using Reshop.Infrastructure.Context;

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

        public async Task<IEnumerable<Domain.Entities.Image.Image>> GetImagesAsync() =>
            await _context.Images.ToListAsync();

        public async Task<IEnumerable<ImagePlace>> GetImagesPlaceAsync() =>
            await _context.ImagesPlace.ToListAsync();

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
