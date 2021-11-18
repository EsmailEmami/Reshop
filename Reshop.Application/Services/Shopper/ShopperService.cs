using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Services.Shopper
{
    public class ShopperService : IShopperService
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IShopperRepository _shopperRepository;

        public ShopperService(IProductRepository productRepository, IShopperRepository shopperRepository)
        {
            _productRepository = productRepository;
            _shopperRepository = shopperRepository;
        }

        #endregion


        public async Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int>> GetShoppersInformationWithPagination(string type = "all", string filter = "", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int shoppersCount = await _shopperRepository.GetShoppersCountWithTypeAsync(type.FixedText());

            var shoppers = _shopperRepository.GetShoppersWithPagination(type.FixedText(), skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * shoppersCount / take);


            return new Tuple<IEnumerable<ShoppersListForAdmin>, int, int>(shoppers, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<ShopperProductsListForShow>, int, int>> GetShopperProductsInformationWithPagination(string shopperId, string type = "all", string filter = "", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int shoppersCount = await _shopperRepository.GetShopperProductsCountWithTypeAsync(shopperId, type.FixedText());

            var products = _shopperRepository.GetShopperProductsWithPagination(shopperId, type.FixedText(), skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * shoppersCount / take);


            return new Tuple<IEnumerable<ShopperProductsListForShow>, int, int>(products, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int, int>> GetProductShoppersInformationWithPagination(int productId, string type = "all", string filter = "", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int shoppersCount = await _shopperRepository.GetShoppersCountWithTypeAsync(type.FixedText());

            var shoppers = _shopperRepository.GetProductShoppersWithPagination(productId, type.FixedText(), skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * shoppersCount / take);


            return new Tuple<IEnumerable<ShoppersListForAdmin>, int, int, int>(shoppers, pageId, totalPages, productId);
        }

        public async Task<bool> IsShopperProductColorOfShopperAsync(string shopperId, string shopperProductColorId) =>
            await _shopperRepository.IsShopperProductColorOfShopperAsync(shopperId, shopperProductColorId);

        public async Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                await _shopperRepository.AddShopperAsync(shopper);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                _shopperRepository.EditShopper(shopper);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct)
        {
            try
            {
                await _shopperRepository.AddShopperProductAsync(shopperProduct);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperProductAsync(ShopperProduct shopperProduct)
        {
            try
            {
                _shopperRepository.UpdateShopperProduct(shopperProduct);
                await _shopperRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Tuple<IEnumerable<ShopperRequestsForShowViewModel>, int, int>> GetShopperRequestsForShowAsync(string shopperId, string type = "all", int pageId = 1, int take = 18, string filter = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4
            int count = 0;

            var model = new List<ShopperRequestsForShowViewModel>();

            switch (type)
            {
                case "all":
                    {
                        count += await _shopperRepository.GetShopperProductColorRequestsCountAsync(shopperId, filter);
                        count += await _shopperRepository.GetShopperProductRequestsCountAsync(shopperId, filter);

                        model.AddRange(_shopperRepository.GetShopperProductColorRequestsForShow(shopperId, skip, take, filter));
                        model.AddRange(_shopperRepository.GetShopperProductRequestsForShow(shopperId, skip, take, filter));
                        break;
                    }
                case "product":
                    {
                        count += await _shopperRepository.GetShopperProductRequestsCountAsync(shopperId, filter);

                        model.AddRange(_shopperRepository.GetShopperProductRequestsForShow(shopperId, skip, take, filter));
                        break;
                    }
                case "color":
                    {
                        count += await _shopperRepository.GetShopperProductColorRequestsCountAsync(shopperId, filter);

                        model.AddRange(_shopperRepository.GetShopperProductColorRequestsForShow(shopperId, skip, take, filter));
                        break;
                    }
            }

            int totalPages = (int)Math.Ceiling(1.0 * count / take);


            return new Tuple<IEnumerable<ShopperRequestsForShowViewModel>, int, int>(model, pageId, totalPages);
        }

        public async Task<ShopperProductRequest> GetShopperProductRequestAsync(string shopperProductRequestId) =>
            await _shopperRepository.GetShopperProductRequestAsync(shopperProductRequestId);

        public async Task<ShopperProductColorRequest> GetShopperProductColorRequestAsync(string shopperProductColorRequestId) =>
            await _shopperRepository.GetShopperProductColorRequestAsync(shopperProductColorRequestId);

        public async Task<ShopperProductRequestForShowViewModel> GetShopperProductRequestForShowAsync(
            string shopperProductRequestId) =>
            await _shopperRepository.GetShopperProductRequestForShowAsync(shopperProductRequestId);

        public async Task<ShopperProductColorRequestForShowViewModel> GetShopperProductColorRequestForShowAsync(
            string shopperProductColorRequestId) =>
            await _shopperRepository.GetShopperProductColorRequestForShowAsync(shopperProductColorRequestId);

        public async Task<ShopperProductRequestForShowShopperViewModel> GetShopperProductRequestForShowShopperAsync(
            string shopperProductRequestId) =>
            await _shopperRepository.GetShopperProductRequestForShowShopperAsync(shopperProductRequestId);

        public async Task<ShopperProductColorRequestForShowShopperViewModel>
            GetShopperProductColorRequestForShowShopperAsync(string shopperProductColorRequestId) =>
            await _shopperRepository.GetShopperProductColorRequestForShowShopperAsync(shopperProductColorRequestId);

        public async Task<ResultTypes> FinishShopperProductRequestAsync(FinishShopperRequestViewModel model)
        {
            try
            {
                var shopperProductRequest = await _shopperRepository.GetShopperProductRequestAsync(model.RequestId);

                if (shopperProductRequest == null)
                    return ResultTypes.Failed;

                if (await _shopperRepository.IsShopperProductExistAsync(shopperProductRequest.ShopperId, shopperProductRequest.ProductId))
                    return ResultTypes.Failed;


                shopperProductRequest.IsRead = true;
                shopperProductRequest.IsSuccess = model.IsSuccess;
                shopperProductRequest.Reason = model.Reason;

                if (model.IsSuccess)
                {
                    var shopperProduct = new ShopperProduct()
                    {
                        CreateDate = DateTime.Now,
                        IsActive = shopperProductRequest.IsActive,
                        IsFinally = true,
                        ProductId = shopperProductRequest.ProductId,
                        ShopperId = shopperProductRequest.ShopperId,
                        Warranty = shopperProductRequest.Warranty
                    };
                    await _shopperRepository.AddShopperProductAsync(shopperProduct);
                }

                _shopperRepository.UpdateShopperProductRequest(shopperProductRequest);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> FinishShopperProductColorRequestAsync(FinishShopperRequestViewModel model)
        {
            try
            {
                var shopperProductColorRequest = await _shopperRepository.GetShopperProductColorRequestAsync(model.RequestId);

                if (shopperProductColorRequest == null)
                    return ResultTypes.Failed;

                if (await _shopperRepository.IsShopperProductColorExistAsync(shopperProductColorRequest.ShopperProductId, shopperProductColorRequest.ColorId))
                {
                    return ResultTypes.Failed;
                }


                shopperProductColorRequest.IsRead = true;
                shopperProductColorRequest.IsSuccess = model.IsSuccess;
                shopperProductColorRequest.Reason = model.Reason;

                if (model.IsSuccess)
                {
                    var shopperProductColor = new ShopperProductColor()
                    {
                        ColorId = shopperProductColorRequest.ColorId,
                        CreateDate = DateTime.Now,
                        IsActive = shopperProductColorRequest.IsActive,
                        IsFinally = true,
                        Price = shopperProductColorRequest.Price,
                        QuantityInStock = shopperProductColorRequest.QuantityInStock,
                        ShortKey = NameGenerator.GenerateShortKey(),
                        ShopperProductId = shopperProductColorRequest.ShopperProductId
                    };
                    await _shopperRepository.AddShopperProductColorAsync(shopperProductColor);
                }

                _shopperRepository.UpdateShopperProductColorRequest(shopperProductColorRequest);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsShopperExistAsync(string shopperId) =>
            await _shopperRepository.IsShopperExistAsync(shopperId);

        public async Task<bool> IsShopperProductOfShopperAsync(string shopperId, string shopperProductId) =>
            await _shopperRepository.IsShopperProductOfShopperAsync(shopperId, shopperProductId);

        public async Task<ResultTypes> UnAvailableShopperProductAsync(string shopperId, int productId)
        {
            var shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (string.IsNullOrEmpty(shopperProductId))
            {
                return ResultTypes.Failed;
            }

            var shopperProduct = await _productRepository.GetShopperProductAsync(shopperProductId);
            if (shopperProduct is null)
            {
                return ResultTypes.Failed;
            }

            if (!shopperProduct.IsFinally)
            {
                return ResultTypes.Failed;
            }

            try
            {
                shopperProduct.IsActive = false;
                _shopperRepository.UpdateShopperProduct(shopperProduct);

                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<string> GetShopperIdOrUserAsync(string userId)
        {
            try
            {
                return await _shopperRepository.GetShopperIdOfUserByUserId(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetShopperProductIdAsync(string shopperId, int productId) =>
            await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

        public async Task<ShopperDataForAdmin> GetShopperDataForAdminAsync(string shopperId) =>
            await _shopperRepository.GetShopperDataForAdminAsync(shopperId);

        public async Task<EditShopperProductViewModel> GetShopperProductDataForEditAsync(string shopperProductId) =>
            await _shopperRepository.GetShopperProductDataForEditAsync(shopperProductId);

        public async Task<bool> IsAnyActiveShopperProductRequestExistAsync(string shopperId, int productId, bool type) =>
            await _shopperRepository.IsAnyActiveShopperProductRequestExistAsync(shopperId, productId, type);

        public async Task<bool> IsAnyActiveShopperProductRequestExistAsync(string shopperProductId, bool type)
        {
            string shopperId = await _shopperRepository.GetShopperIdOfShopperProductAsync(shopperProductId);
            int productId = await _shopperRepository.GetProductIdOfShopperProductAsync(shopperProductId);

            if (string.IsNullOrEmpty(shopperId) || productId == 0)
                return false;

            return await _shopperRepository.IsAnyActiveShopperProductRequestExistAsync(shopperId, productId, type);
        }

        public async Task<ResultTypes> AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest)
        {
            try
            {
                await _shopperRepository.AddShopperProductRequestAsync(shopperProductRequest);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperProductColorRequestAsync(ShopperProductColorRequest shopperProductColorRequest)
        {
            try
            {
                await _shopperRepository.AddShopperProductColorRequestAsync(shopperProductColorRequest);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<IEnumerable<StoreAddress>> GetShopperStoreAddressesAsync(string shopperUserId)
        {
            var shopperId = await _shopperRepository.GetShopperIdOfUserByUserId(shopperUserId);

            return _shopperRepository.GetShopperStoreAddresses(shopperId);
        }

        public async Task<ResultTypes> AddStoreAddressAsync(StoreAddress storeAddress)
        {
            try
            {
                await _shopperRepository.AddStoreAddressAsync(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditStoreAddressAsync(StoreAddress storeAddress)
        {
            try
            {
                _shopperRepository.EditStoreAddress(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveStoreAddressAsync(string storeAddressId)
        {
            var storeAddress = await _shopperRepository.GetStoreAddressByIdAsync(storeAddressId);

            if (storeAddress is null) return ResultTypes.Failed;

            try
            {
                _shopperRepository.RemoveStoreAddress(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<StoreTitle> GetStoreTitles() => _shopperRepository.GetStoreTitles();

        public async Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId) =>
            await _shopperRepository.GetStoreTitleByIdAsync(storeTitleId);

        public async Task<ResultTypes> AddStoreTitleAsync(StoreTitle storeTitle)
        {
            try
            {
                await _shopperRepository.AddStoreTitleAsync(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditStoreTitleAsync(StoreTitle storeTitle)
        {
            try
            {
                _shopperRepository.UpdateStoreTitle(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteStoreTitleAsync(int storeTitleId)
        {
            var storeTitle = await _shopperRepository.GetStoreTitleByIdAsync(storeTitleId);

            if (storeTitle is null) return ResultTypes.Failed;

            try
            {
                _shopperRepository.RemoveStoreTitle(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperStoreTitleAsync(string shopperId, List<int> storeTitlesId)
        {
            try
            {
                foreach (var storeTitleId in storeTitlesId)
                {
                    await _shopperRepository.AddShopperStoreTitleAsync(new ShopperStoreTitle()
                    {
                        ShopperId = shopperId,
                        StoreTitleId = storeTitleId
                    });
                }

                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<string> GetShopperStoreTitlesName(string shopperId) =>
            _shopperRepository.GetShopperStoreTitlesName(shopperId);


        public IEnumerable<Tuple<int, string>> GetShopperStoreTitles(string shopperId) =>
            _shopperRepository.GetShopperStoreTitles(shopperId);

        public IEnumerable<Color> GetColors() =>
            _shopperRepository.GetColors();

        public IEnumerable<Tuple<int, string>> GetColorsIdAndName() =>
            _shopperRepository.GetColorsIdAndName();

        public async Task<string> GetShopperProductColorIdAsync(string shopperId, int productId, int colorId)
        {
            string shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId is null)
                return null;

            return await _shopperRepository.GetShopperProductColorIdAsync(shopperProductId, colorId);
        }

        public async Task<string> GetShopperProductColorIdAsync(string shopperProductId, int colorId) =>
            await _shopperRepository.GetShopperProductColorIdAsync(shopperProductId, colorId);

        public async Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductColorId) =>
            await _shopperRepository.GetShopperProductColorAsync(shopperProductColorId);

        public async Task<ResultTypes> AddShopperProductColorAsync(ShopperProductColor shopperProductColor)
        {
            try
            {
                await _shopperRepository.AddShopperProductColorAsync(shopperProductColor);
                await _shopperRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperProductColorAsync(ShopperProductColor shopperProductColor)
        {
            try
            {
                _shopperRepository.UpdateShopperProductColor(shopperProductColor);
                await _shopperRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsShopperProductColorExistAsync(string shopperProductId, int colorId) =>
            await _shopperRepository.IsShopperProductColorExistAsync(shopperProductId, colorId);

        public async Task<ShopperProductColorDetailViewModel> GetShopperProductColorDetailAsync(string shopperProductColorId) =>
            await _shopperRepository.GetShopperProductColorDetailAsync(shopperProductColorId);

        public async Task<bool> IsAnyActiveShopperProductColorRequestExistAsync(string shopperProductId, int colorId, bool type) =>
            await _shopperRepository.IsAnyActiveShopperProductColorRequestExistAsync(shopperProductId, colorId, type);

        public async Task<IEnumerable<LastThirtyDayProductDataChart>> GetLastThirtyDayProductDataChartAsync(int productId, string shopperId)
        {
            var shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId is null)
                return null;


            return _shopperRepository.GetLastThirtyDayProductDataChart(shopperProductId);
        }

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(string shopperProductColorId) =>
            _shopperRepository.GetLastThirtyDayColorProductDataChart(shopperProductColorId);

        public IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfProductChart(int productId) =>
            _shopperRepository.GetLastThirtyDayBestShoppersOfProductChart(productId);

        public IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfColorProductChart(string shopperProductColorId) =>
            _shopperRepository.GetLastThirtyDayBestShoppersOfColorProductChart(shopperProductColorId);

        public IEnumerable<Tuple<string, int>> GetBestShoppersOfProductChart(int productId) =>
            _shopperRepository.GetBestShoppersOfProductChart(productId);

        public IEnumerable<Tuple<string, int>> GetBestShoppersOfColorProductChart(string shopperProductColorId) =>
            _shopperRepository.GetBestShoppersOfColorProductChart(shopperProductColorId);

        public async Task<IEnumerable<Tuple<string, int, int, int>>> GetColorsOfShopperProductDataChartAsync(int productId, string shopperId)
        {
            string shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId is null)
                return null;

            return _shopperRepository.GetColorsOfShopperProductDataChart(shopperProductId);
        }
    }
}
