using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        Task<string> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
    }
}