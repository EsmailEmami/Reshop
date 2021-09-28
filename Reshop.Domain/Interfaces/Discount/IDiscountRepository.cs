using Reshop.Domain.DTOs.Discount;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Discount
{
    public interface IDiscountRepository
    {
        #region user discount

        Entities.User.Discount GetDiscountByCode(string discountCode);
        Task<Entities.User.Discount> GetDiscountByCodeAsync(string discountCode);
        void UpdateDiscount(Entities.User.Discount discount);
        bool IsUserDiscountCodeExist(string userId, string discountId);
        Task AddUserDiscountCodeAsync(UserDiscountCode userDiscountCode);

        #endregion

        #region shopper product discount

        Task<ShopperProductDiscount> GetLastShopperProductDiscountAsync(string shopperProductColorId);
        Task AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        void UpdateShopperProductDiscount(ShopperProductDiscount shopperProductDiscount);
        Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId);
        Task<DiscountsGeneralDataViewModel> GetShopperProductColorDiscountsGeneralDataAsync(string shopperProductColorId);

        #endregion

        #region product

        IEnumerable<DiscountsForShowViewModel> GetProductColorDiscountsWithPaginationAsync(int productId, int colorId, int skip, int take, string filter = "");
        Task<int> GetProductColorDiscountsCountAsync(int productId, int colorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId);
        Task<DiscountsGeneralDataViewModel> GetProductColorDiscountsGeneralDataAsync(int productId, int colorId);

        #endregion

        Task SaveChangesAsync();
    }
}