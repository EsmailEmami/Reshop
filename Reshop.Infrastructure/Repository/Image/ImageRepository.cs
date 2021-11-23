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
    }
}
