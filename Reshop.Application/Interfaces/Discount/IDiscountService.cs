using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.Discount;
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
        Task<DiscountsGeneralDataViewModel> GetShopperProductColorDiscountsGeneralDataAsync(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId);

        #endregion

        #region product

        Task<Tuple<IEnumerable<DiscountsForShowViewModel>, int, int>> GetProductColorDiscountsWithPaginationAsync(int productId, int colorId,int pageId= 1,int take =25,string filter = "");
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId);
        Task<DiscountsGeneralDataViewModel> GetProductColorDiscountsGeneralDataAsync(int productId, int colorId);

        #endregion
    }
}