using Reshop.Application.Interfaces.Discount;
using Reshop.Domain.Interfaces.Discount;

namespace Reshop.Application.Services.Discount
{
    public class DiscountService : IDiscountService
    {
        #region constructor

        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        #endregion
    }
}