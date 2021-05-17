using System.Threading.Tasks;
using Reshop.Application.Enums;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        Task<Domain.Entities.User.User> AddShopperAsync(Domain.Entities.User.User user, Domain.Entities.Shopper.Shopper shopper);
    }
}