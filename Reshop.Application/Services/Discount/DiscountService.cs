using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Discount;
using Reshop.Domain.DTOs.Discount;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Discount;
using Reshop.Domain.Interfaces.User;

namespace Reshop.Application.Services.Discount
{
    public class DiscountService : IDiscountService
    {
        #region constructor

        private readonly IDiscountRepository _discountRepository;
        private readonly ICartRepository _cartRepository;

        public DiscountService(IDiscountRepository discountRepository, ICartRepository cartRepository)
        {
            _discountRepository = discountRepository;
            _cartRepository = cartRepository;
        }

        #endregion

        public async Task<DiscountUseType> UseDiscountAsync(string orderId, string discountCode)
        {
            var discount = await _discountRepository.GetDiscountByCodeAsync(discountCode);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.StartDate != null && discount.StartDate < DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.EndDate != null && discount.EndDate >= DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = await _cartRepository.GetOrderByIdAsync(orderId);

            if (_discountRepository.IsUserDiscountCodeExist(order.UserId, discount.DiscountId))
                return DiscountUseType.UserUsed;

            var percent = (order.Sum * discount.DiscountPercent) / 100;
            order.Sum -= percent;
            _cartRepository.UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _discountRepository.UpdateDiscount(discount);


            var userDiscountCode = new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId
            };
            await _discountRepository.AddUserDiscountCodeAsync(userDiscountCode);

            await _discountRepository.SaveChangesAsync();

            return DiscountUseType.Success;
        }

        public async Task<ResultTypes> AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount)
        {
            try
            {
                await _discountRepository.AddShopperProductDiscountAsync(shopperProductDiscount);
                await _discountRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount)
        {
            try
            {
                _discountRepository.UpdateShopperProductDiscount(shopperProductDiscount);
                await _discountRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ShopperProductDiscount> GetLastShopperProductColorDiscountAsync(string shopperProductColorId) =>
            await _discountRepository.GetLastShopperProductDiscountAsync(shopperProductColorId);

        public async Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId) =>
            await _discountRepository.IsActiveShopperProductColorDiscountExistsAsync(shopperProductColorId);

        public async Task<DiscountsGeneralDataViewModel> GetShopperProductColorDiscountsGeneralDataAsync(string shopperProductColorId) =>
            await _discountRepository.GetShopperProductColorDiscountsGeneralDataAsync(shopperProductColorId);

        public IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId) =>
            _discountRepository.GetLastTwentyDiscountDataOfShopperProductColorChart(shopperProductColorId);

        public async Task<Tuple<IEnumerable<DiscountsForShowViewModel>, int, int>> GetProductColorDiscountsWithPaginationAsync(int productId, int colorId, int pageId = 1, int take = 25, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int count = await _discountRepository.GetProductColorDiscountsCountAsync(productId, colorId);

            var data = _discountRepository.GetProductColorDiscountsWithPaginationAsync(productId, colorId, skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * count / take);

            return new Tuple<IEnumerable<DiscountsForShowViewModel>, int, int>(data, pageId, totalPages);
        }

        public IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId) =>
            _discountRepository.GetLastTwentyDiscountDataOfProductColorChart(productId, colorId);

        public async Task<DiscountsGeneralDataViewModel> GetProductColorDiscountsGeneralDataAsync(int productId, int colorId) =>
            await _discountRepository.GetProductColorDiscountsGeneralDataAsync(productId, colorId);
    }
}