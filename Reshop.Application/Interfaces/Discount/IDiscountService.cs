using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Application.Interfaces.Discount
{
    public interface IDiscountService
    {
        #region user

        Task<DiscountUseType> UseDiscountAsync(string orderId, string discountCode);

        #endregion

        #region shopper product

        Task<ResultTypes> AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        Task<ResultTypes> EditShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        Task<ShopperProductDiscount> GetLastShopperProductColorDiscountAsync(string shopperProductColorId);
        Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId);
        Task<ShopperProductColorDiscountDetailViewModel> GetShopperProductColorDiscountDetailAsync(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId);

        #endregion

        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId);
    }
}