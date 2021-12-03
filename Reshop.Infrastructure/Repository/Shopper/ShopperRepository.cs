using Microsoft.EntityFrameworkCore;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Shopper
{
    public class ShopperRepository : IShopperRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public ShopperRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion


        public async Task<bool> IsShopperExistAsync(string shopperId) =>
            await _context.Shoppers.AnyAsync(c => c.ShopperId == shopperId);

        public async Task<bool> IsUserShopperAsync(string userId) =>
            await _context.Shoppers.AnyAsync(c => c.UserId == userId);

        public async Task<Domain.Entities.Shopper.Shopper> GetShopperByIdAsync(string shopperId) =>
            await _context.Shoppers.FindAsync(shopperId);

        public async Task AddShopperAsync(Domain.Entities.Shopper.Shopper shopper)
            =>
                await _context.Shoppers.AddAsync(shopper);

        public void EditShopper(Domain.Entities.Shopper.Shopper shopper) =>
            _context.Shoppers.Update(shopper);

        public void RemoveShopper(Domain.Entities.Shopper.Shopper shopper) =>
             _context.Shoppers.Remove(shopper);

        public async Task<EditShopperViewModel> GetShopperDataForEditAsync(string shopperId)
            =>
                await _context.Shoppers.Where(c => c.ShopperId == shopperId)
                    .Select(c => new EditShopperViewModel()
                    {
                        ShopperId = c.ShopperId,
                        FullName = c.User.FullName,
                        Email = c.User.Email,
                        PhoneNumber = c.User.PhoneNumber,
                        NationalCode = c.User.NationalCode,
                        BirthDay = c.BirthDay.ToString(),
                        StoreName = c.StoreName,
                        IsActive = c.IsActive,
                        BusinessLicenseImageName = c.BusinessLicenseImageName,
                        OnNationalCardImageName = c.OnNationalCardImageName,
                        SelectedStoreTitles = c.ShopperTitles.Select(s=> s.StoreTitleId),
                        SelectedRoles = c.User.UserRoles.Select(s=> s.RoleId)
                    }).SingleOrDefaultAsync();

        public async Task<string> GetShopperIdOfUserByUserId(string userId) =>
            await _context.Shoppers.Where(c => c.UserId == userId).Select(c => c.ShopperId).SingleOrDefaultAsync();

        public async Task<bool> IsShopperProductColorOfShopperAsync(string shopperId, string shopperProductColorId) =>
            await _context.ShopperProductColors.AnyAsync(c => c.ShopperProductColorId == shopperProductColorId &&
                                                              c.ShopperProduct.ShopperId == shopperId);

        public async Task<string> GetShopperProductIdAsync(string shopperId, int productId) =>
            await _context.ShopperProducts.Where(c => c.ShopperId == shopperId && c.ProductId == productId)
                .Select(c => c.ShopperProductId).SingleOrDefaultAsync();

        public async Task<string> GetShopperStoreNameAsync(string shopperId) =>
            await _context.Shoppers.Where(c => c.ShopperId == shopperId)
                .Select(c => c.StoreName)
                .SingleOrDefaultAsync();

        public async Task<Tuple<string, string>> GetShopperIdAndStoreNameOfShopperProductColorAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => c.ShopperProduct.Shopper)
                .Select(c => new Tuple<string, string>(c.ShopperId, c.StoreName))
                .SingleOrDefaultAsync();


        public IEnumerable<ShoppersListForAdmin> GetShoppersWithPagination(string type = "all", int skip = 0,
            int take = 18, string filter = null)
        {
            IQueryable<Domain.Entities.Shopper.Shopper> shoppers = _context.Shoppers;


            switch (type)
            {
                case "all":
                    break;
                case "active":
                    shoppers = shoppers.Where(c => c.IsActive);
                    break;
                case "existed":
                    shoppers = shoppers.Where(c => !c.IsActive);
                    break;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                shoppers = shoppers
                    .Where(c => c.User.FullName.Contains(filter) ||
                                c.User.NationalCode.Contains(filter) ||
                                c.User.Email.Contains(filter));
            }


            shoppers = shoppers.Skip(skip).Take(take);

            return shoppers.Select(c => new ShoppersListForAdmin()
            {
                ShopperId = c.ShopperId,
                ShopperName = c.User.FullName,
                StoreName = c.StoreName,
                IsActive = c.IsActive
            });
        }

        public IEnumerable<ShoppersListForAdmin> GetProductShoppersWithPagination(int productId, string type = "all",
            int skip = 0, int take = 18, string filter = null)
        {
            IQueryable<Domain.Entities.Shopper.Shopper> shoppers = _context.ShopperProducts
                .Where(c => c.ProductId == productId)
                .Select(c => c.Shopper).Distinct();


            switch (type)
            {
                case "all":
                    break;
                case "active":
                    shoppers = shoppers.Where(c => c.IsActive);
                    break;
                case "existed":
                    shoppers = shoppers.Where(c => !c.IsActive);
                    break;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                shoppers = shoppers
                    .Where(c => c.User.FullName.Contains(filter) ||
                                c.User.NationalCode.Contains(filter) ||
                                c.User.Email.Contains(filter));
            }

            shoppers = shoppers.Skip(skip).Take(take);
            return shoppers.Select(c => new ShoppersListForAdmin()
            {
                ShopperId = c.ShopperId,
                ShopperName = c.User.FullName,
                StoreName = c.StoreName,
                IsActive = c.IsActive
            });
        }

        public IEnumerable<ShopperProductsListForShow> GetShopperProductsWithPagination(string shopperId,
            string type = "all", int skip = 0, int take = 18, string filter = null)
        {
            IQueryable<ShopperProduct> products = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId);


            switch (type)
            {
                case "all":
                    break;
                case "active":
                    products = products.Where(c => c.IsActive);
                    break;
                case "existed":
                    products = products.Where(c => !c.IsActive);
                    break;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                products = products
                    .Where(c => c.Product.ProductTitle.Contains(filter));
            }


            products = products.Skip(skip).Take(take);

            return products.Select(c => new ShopperProductsListForShow()
            {
                ShopperProductId = c.ShopperProductId,
                ProductName = c.Product.ProductTitle,
                ColorsCount = c.ShopperProductColors.Count,
                CreateDate = c.CreateDate,
                IsActive = c.IsActive
            });
        }

        public async Task<int> GetShoppersCountWithTypeAsync(string type = "all")
        {
            return type switch
            {
                "all" => await _context.Shoppers.CountAsync(),
                "active" => await _context.Shoppers.Where(c => c.IsActive).CountAsync(),
                "existed" => await _context.Shoppers.Where(c => !c.IsActive).CountAsync(),
                _ => await _context.Shoppers.CountAsync()
            };
        }

        public async Task<int> GetShopperProductsCountWithTypeAsync(string shopperId, string type = "all")
        {
            if (type == "all")
            {
                return await _context.ShopperProducts.Where(c => c.ShopperId == shopperId)
                    .Select(c => c.Product).CountAsync();
            }

            return await _context.ShopperProducts.Where(c => c.ShopperId == shopperId)
                .Select(c => c.Product).Where(c => c.ProductType == type).CountAsync();
        }

        public async Task<ShopperDataForAdmin> GetShopperDataForAdminAsync(string shopperId) =>
            await _context.Shoppers.Where(c => c.ShopperId == shopperId)
                .Select(c => new ShopperDataForAdmin()
                {
                    ShopperId = c.ShopperId,
                    ShopperFullName = c.User.FullName,
                    BirthDay = c.BirthDay,
                    Email = c.User.Email,
                    FullIdOfIdentityCard = "be zodi ezafeh mikonam",
                    IsActive = c.IsActive,
                    IssuanceOfIdentityCard = "be zodi ezafeh mikomnam",
                    NationalCode = c.User.NationalCode,
                    PhoneNumber = c.User.PhoneNumber,
                    OnNationalCardImageName = c.OnNationalCardImageName,
                    BusinessLicenseImageName = c.BusinessLicenseImageName,
                    StoreAddresses = c.StoresAddress.Select(s => new StoreAddressForShowViewModel()
                    {
                        CityName = s.City.CityName,
                        StateName = s.City.State.StateName,
                        AddressText = s.AddressText,
                        LandlinePhoneNumber = s.LandlinePhoneNumber,
                        Plaque = s.Plaque,
                        PostalCode = s.PostalCode,
                        StoreAddressId = s.StoreAddressId,
                        StoreName = s.StoreName
                    })
                }).SingleOrDefaultAsync();

        public async Task<EditShopperProductViewModel> GetShopperProductDataForEditAsync(string shopperProductId) =>
            await _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                .Select(c => new EditShopperProductViewModel()
                {
                    ShopperId = c.ShopperId,
                    ProductId = c.ProductId,
                    Warranty = c.Warranty,
                    IsActive = c.IsActive,
                }).SingleOrDefaultAsync();

        public async Task<bool> IsAnyActiveShopperProductRequestExistAsync(string shopperId, int productId, bool type) =>
            await _context.ShopperProductRequests
                .AnyAsync(c => c.ShopperId == shopperId
                               && c.ProductId == productId
                               && c.RequestType == type
                               && !c.IsRead);


        public async Task AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest) =>
            await _context.ShopperProductRequests.AddAsync(shopperProductRequest);

        public async Task AddShopperProductColorRequestAsync(ShopperProductColorRequest shopperProductColorRequest) =>
            await _context.ShopperProductColorRequests.AddAsync(shopperProductColorRequest);

        public IEnumerable<ShopperRequestsForShowViewModel> GetShopperProductColorRequestsForShow(string shopperId, int skip, int take, string filter = null)
        {
            IQueryable<ShopperRequestsForShowViewModel> requests = _context.ShopperProductColorRequests
                .Where(c => c.ShopperProduct.ShopperId == shopperId)
                 .Select(c => new ShopperRequestsForShowViewModel()
                 {
                     RequestId = c.ShopperProductColorRequestId,
                     ProductName = c.ShopperProduct.Product.ProductTitle,
                     RequestType = "color",
                     Type = c.RequestType,
                     RequestDate = c.RequestDate,
                     IsRead = c.IsRead,
                     IsSuccess = c.IsSuccess,
                     Description = c.Reason
                 });

            if (!string.IsNullOrEmpty(filter))
            {
                requests = requests.Where(c => c.ProductName.Contains(filter));
            }

            requests = requests.Skip(skip).Take(take);


            return requests;
        }

        public IEnumerable<ShopperRequestsForShowViewModel> GetShopperProductRequestsForShow(string shopperId, int skip, int take, string filter = null)
        {
            IQueryable<ShopperRequestsForShowViewModel> requests = _context.ShopperProductRequests.Where(c => c.ShopperId == shopperId)
                .Select(c => new ShopperRequestsForShowViewModel()
                {
                    RequestId = c.ShopperProductRequestId,
                    ProductName = c.Product.ProductTitle,
                    RequestType = "product",
                    Type = c.RequestType,
                    RequestDate = c.RequestDate,
                    IsRead = c.IsRead,
                    IsSuccess = c.IsSuccess,
                    Description = c.Reason
                });

            if (!string.IsNullOrEmpty(filter))
            {
                requests = requests.Where(c => c.ProductName.Contains(filter));
            }

            requests = requests.Skip(skip).Take(take);


            return requests;
        }

        public async Task<int> GetShopperProductColorRequestsCountAsync(string shopperId, string filter = null)
        {
            IQueryable<ShopperProductColorRequest> requests = _context.ShopperProductColorRequests
                .Where(c => c.ShopperProduct.ShopperId == shopperId);

            if (!string.IsNullOrEmpty(filter))
            {
                requests = requests.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            return await requests.CountAsync();
        }

        public async Task<int> GetShopperProductRequestsCountAsync(string shopperId, string filter = null)
        {
            IQueryable<ShopperProductRequest> requests = _context.ShopperProductRequests
                .Where(c => c.ShopperId == shopperId);

            if (!string.IsNullOrEmpty(filter))
            {
                requests = requests.Where(c => c.Product.ProductTitle.Contains(filter));
            }

            return await requests.CountAsync();
        }

        public async Task<ShopperProductRequest> GetShopperProductRequestAsync(string shopperProductRequestId) =>
            await _context.ShopperProductRequests.FindAsync(shopperProductRequestId);

        public async Task<ShopperProductColorRequest> GetShopperProductColorRequestAsync(string shopperProductColorRequestId) =>
            await _context.ShopperProductColorRequests.FindAsync(shopperProductColorRequestId);

        public async Task<ShopperProductRequestForShowViewModel> GetShopperProductRequestForShowAsync(
            string shopperProductRequestId) =>
            await _context.ShopperProductRequests.Where(c => c.ShopperProductRequestId == shopperProductRequestId)
                .Select(c => new ShopperProductRequestForShowViewModel()
                {
                    ShopperId = c.ShopperId,
                    ProductId = c.ProductId,
                    StoreName = c.Shopper.StoreName,
                    ProductName = c.Product.ProductTitle,
                    RequestDate = c.RequestDate,
                    RequestType = c.RequestType,
                    IsRead = c.IsRead,
                    IsSuccess = c.IsSuccess,
                    Description = c.Reason,
                    IsActive = c.IsActive,
                    Warranty = c.Warranty,
                    ShopperProductRequestId = c.ShopperProductRequestId,
                    UserId = c.RequestUserId,
                    UserFullName = c.User.FullName,
                }).SingleOrDefaultAsync();

        public async Task<ShopperProductColorRequestForShowViewModel> GetShopperProductColorRequestForShowAsync(string shopperProductColorRequestId) =>
            await _context.ShopperProductColorRequests.Where(c => c.ShopperProductColorRequestId == shopperProductColorRequestId)
                .Select(c => new ShopperProductColorRequestForShowViewModel()
                {
                    ShopperId = c.ShopperProduct.ShopperId,
                    ProductId = c.ShopperProduct.ProductId,
                    StoreName = c.ShopperProduct.Shopper.StoreName,
                    ProductName = c.ShopperProduct.Product.ProductTitle,
                    RequestDate = c.RequestDate,
                    RequestType = c.RequestType,
                    IsRead = c.IsRead,
                    IsSuccess = c.IsSuccess,
                    Description = c.Reason,
                    IsActive = c.IsActive,
                    ShopperProductColorRequestId = c.ShopperProductColorRequestId,
                    UserId = c.RequestUserId,
                    UserFullName = c.User.FullName,
                    ColorName = c.Color.ColorName,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock
                }).SingleOrDefaultAsync();

        public async Task<ShopperProductRequestForShowShopperViewModel> GetShopperProductRequestForShowShopperAsync(string shopperProductRequestId) =>
            await _context.ShopperProductRequests.Where(c => c.ShopperProductRequestId == shopperProductRequestId)
                .Select(c => new ShopperProductRequestForShowShopperViewModel()
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductTitle,
                    RequestDate = c.RequestDate,
                    RequestType = c.RequestType,
                    IsRead = c.IsRead,
                    IsSuccess = c.IsSuccess,
                    Description = c.Reason,
                    IsActive = c.IsActive,
                    Warranty = c.Warranty,
                    ShopperProductRequestId = c.ShopperProductRequestId,
                    UserFullName = c.User.FullName,
                }).SingleOrDefaultAsync();

        public async Task<ShopperProductColorRequestForShowShopperViewModel> GetShopperProductColorRequestForShowShopperAsync(string shopperProductColorRequestId) =>
            await _context.ShopperProductColorRequests.Where(c => c.ShopperProductColorRequestId == shopperProductColorRequestId)
                .Select(c => new ShopperProductColorRequestForShowShopperViewModel()
                {
                    ProductId = c.ShopperProduct.ProductId,
                    ProductName = c.ShopperProduct.Product.ProductTitle,
                    RequestDate = c.RequestDate,
                    RequestType = c.RequestType,
                    IsRead = c.IsRead,
                    IsSuccess = c.IsSuccess,
                    Description = c.Reason,
                    IsActive = c.IsActive,
                    ShopperProductColorRequestId = c.ShopperProductColorRequestId,
                    UserFullName = c.User.FullName,
                    ColorName = c.Color.ColorName,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock
                }).SingleOrDefaultAsync();

        public void UpdateShopperProductRequest(ShopperProductRequest shopperProductRequest) =>
            _context.ShopperProductRequests.Update(shopperProductRequest);

        public void UpdateShopperProductColorRequest(ShopperProductColorRequest shopperProductColorRequest) =>
            _context.ShopperProductColorRequests.Update(shopperProductColorRequest);

        public void UpdateShopperProduct(ShopperProduct shopperProduct) => _context.ShopperProducts.Update(shopperProduct);

        public void RemoveShopperProduct(ShopperProduct shopperProduct) =>
            _context.ShopperProducts.Remove(shopperProduct);

        public async Task AddShopperProductAsync(ShopperProduct shopperProduct) =>
            await _context.ShopperProducts.AddAsync(shopperProduct);

        public IEnumerable<ShopperProduct> GetShoppersOfProduct(int productId) =>
            _context.ShopperProducts.Where(c => c.ProductId == productId);

        public async Task<bool> IsShopperProductExistAsync(string shopperId, int productId) =>
            await _context.ShopperProducts.AnyAsync(c => c.ShopperId == shopperId && c.ProductId == productId);

        public async Task<bool> IsShopperProductExistAsync(string shopperProductId) =>
            await _context.ShopperProducts.AnyAsync(c => c.ShopperProductId == shopperProductId);

        public async Task<bool> IsShopperProductOfShopperAsync(string shopperId, string shopperProductId) =>
            await _context.ShopperProducts.AnyAsync(c => c.ShopperProductId == shopperProductId && c.ShopperId == shopperId);

        public async Task<string> GetShopperIdOfShopperProductAsync(string shopperProductId) =>
            await _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                .Select(c => c.ShopperId)
                .SingleOrDefaultAsync();

        public async Task<int> GetProductIdOfShopperProductAsync(string shopperProductId) =>
            await _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                .Select(c => c.ProductId)
                .SingleOrDefaultAsync();

        public IEnumerable<StoreTitle> GetStoreTitles() => _context.StoreTitles;

        public async Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId) =>
            await _context.StoreTitles.FindAsync(storeTitleId);

        public async Task AddStoreTitleAsync(StoreTitle storeTitle)
            =>
                await _context.StoreTitles.AddAsync(storeTitle);

        public void UpdateStoreTitle(StoreTitle storeTitle)
            =>
                _context.StoreTitles.Update(storeTitle);

        public void RemoveStoreTitle(StoreTitle storeTitle)
            =>
                _context.StoreTitles.Remove(storeTitle);

        public async Task AddShopperStoreTitleAsync(ShopperStoreTitle shopperStoreTitle)
            =>
                await _context.ShopperStoreTitles.AddAsync(shopperStoreTitle);

        public void RemoveShopperStoreTitle(ShopperStoreTitle shopperStoreTitle)
            =>
                 _context.ShopperStoreTitles.Remove(shopperStoreTitle);

        public IEnumerable<string> GetShopperStoreTitlesName(string shopperId) =>
            _context.ShopperStoreTitles
                .Where(c => c.ShopperId == shopperId)
                .Select(c => c.StoreTitle.StoreTitleName);

        public IEnumerable<Tuple<int, string>> GetShopperStoreTitles(string shopperId) =>
            _context.ShopperStoreTitles
                .Where(c => c.ShopperId == shopperId)
                .Select(c => c.StoreTitle)
                .Select(c => new Tuple<int, string>(c.StoreTitleId, c.StoreTitleName));

        public async Task<IEnumerable<ShopperStoreTitle>> GetShopperStoreTitlesAsync(string shopperId) =>
            await _context.ShopperStoreTitles
                .Where(c => c.ShopperId == shopperId).ToListAsync();

        public IEnumerable<StoreAddress> GetShopperStoreAddresses(string shopperId) =>
            _context.StoresAddress.Where(c => c.ShopperId == shopperId);

        public async Task<IEnumerable<StoreAddressForShowViewModel>> GetShopperStoreAddressesForShowAsync(string shopperId) =>
            await _context.StoresAddress.Where(c => c.ShopperId == shopperId)
                .Select(c => new StoreAddressForShowViewModel()
                {
                    StoreAddressId = c.StoreAddressId,
                    StoreName = c.StoreName,
                    StateName = c.City.State.StateName,
                    CityName = c.City.CityName,
                    AddressText = c.AddressText,
                    Plaque = c.Plaque,
                    LandlinePhoneNumber = c.LandlinePhoneNumber,
                    PostalCode = c.PostalCode
                })
                .ToListAsync();

        public async Task AddStoreAddressAsync(StoreAddress storeAddress) =>
            await _context.StoresAddress.AddAsync(storeAddress);

        public void EditStoreAddress(StoreAddress storeAddress) =>
            _context.StoresAddress.Update(storeAddress);

        public void RemoveStoreAddress(StoreAddress storeAddress) =>
            _context.StoresAddress.Remove(storeAddress);

        public async Task<StoreAddress> GetStoreAddressByIdAsync(string storeAddressId) =>
            await _context.StoresAddress.FindAsync(storeAddressId);


        public IEnumerable<Color> GetColors() =>
            _context.Colors;

        public IEnumerable<Tuple<int, string>> GetColorsIdAndName() =>
            _context.Colors.Select(c => new Tuple<int, string>(c.ColorId, c.ColorName));

        public async Task<bool> IsAnyActiveShopperProductColorRequestExistAsync(string shopperProductId, int colorId, bool type) =>
            await _context.ShopperProductColorRequests
                .AnyAsync(c => c.ShopperProductId == shopperProductId
                               && c.ColorId == colorId
                               && !c.IsRead
                               && c.RequestType == type);

        public async Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.FindAsync(shopperProductColorId);

        public async Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductId, int colorId) =>
            await _context.ShopperProductColors.SingleOrDefaultAsync(c => c.ShopperProductId == shopperProductId && c.ColorId == colorId);


        public async Task AddShopperProductColorAsync(ShopperProductColor shopperProductColor) =>
            await _context.ShopperProductColors.AddAsync(shopperProductColor);

        public async Task<bool> IsShopperProductColorExistAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.AnyAsync(c => c.ShopperProductColorId == shopperProductColorId);

        public async Task<bool> IsShopperProductColorExistAsync(string shopperProductId, int colorId) =>
            await _context.ShopperProductColors.AnyAsync(c => c.ShopperProductId == shopperProductId && c.ColorId == colorId);

        public void UpdateShopperProductColor(ShopperProductColor shopperProductColor) =>
            _context.ShopperProductColors.Update(shopperProductColor);

        public async Task<string> GetShopperProductColorIdAsync(string shopperProductId, int colorId) =>
            await _context.ShopperProductColors
                .Where(c => c.ShopperProductId == shopperProductId && c.ColorId == colorId)
                .Select(c => c.ShopperProductColorId).SingleOrDefaultAsync();

        public async Task<ShopperProductColorDetailViewModel> GetShopperProductColorDetailAsync(
            string shopperProductColorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => new ShopperProductColorDetailViewModel()
                {
                    ProductId = c.ShopperProduct.ProductId,
                    ColorId = c.ColorId,
                    ColorName = c.Color.ColorName,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock,
                    ReturnedCount = 10,
                    SellCount = _context.OrderDetails.Where(o => o.ShopperProductColorId == c.ShopperProductColorId && o.Order.IsPayed).Select(s => s.Count).Sum(),
                    LastMonthSellCount = _context.OrderDetails.Where(o => o.ShopperProductColorId == c.ShopperProductColorId && o.Order.IsPayed && o.Order.PayDate >= DateTime.Now.AddDays(-30)).Select(s => s.Count).Sum(),
                    Income = _context.OrderDetails.Where(o => o.ShopperProductColorId == c.ShopperProductColorId && o.Order.IsPayed).Select(s => s.Order.Sum).Sum(),
                }).SingleOrDefaultAsync();

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(string shopperProductId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProductId == shopperProductId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .OrderBy(c => c.Order.PayDate)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Order.PayDate.Value.ToShamsiDate(),
                    ViewCount = 10,
                    SellCount = c.Count,
                });

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(string shopperProductColorId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProductColorId == shopperProductColorId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .OrderBy(c => c.Order.PayDate)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Order.PayDate.Value.ToShamsiDate(),
                    ViewCount = 10,
                    SellCount = c.Count,
                });

        public IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfProductChart(int productId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .GroupBy(c => c.ShopperProductColor.ShopperProduct.Shopper.StoreName)
                .Select(c => new Tuple<string, int>(c.Key, c.Sum(s => s.Count))).Take(20);

        public IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfColorProductChart(int productId, int colorId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ColorId == colorId &&
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .GroupBy(c => c.ShopperProductColor.ShopperProduct.Shopper.StoreName)
                .Select(c => new Tuple<string, int>(c.Key, c.Sum(s => s.Count))).Take(20);

        public IEnumerable<Tuple<string, int>> GetBestShoppersOfProductChart(int productId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.Order.IsPayed)
                .GroupBy(c => c.ShopperProductColor.ShopperProduct.Shopper.StoreName)
                .Select(c => new Tuple<string, int>(c.Key, c.Sum(s => s.Count))).Take(20);

        public IEnumerable<Tuple<string, int>> GetBestShoppersOfColorProductChart(int productId, int colorId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ColorId == colorId &&
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.Order.IsPayed)
                .GroupBy(c => c.ShopperProductColor.ShopperProduct.Shopper.StoreName)
                .Select(c => new Tuple<string, int>(c.Key, c.Sum(s => s.Count))).Take(20);

        public IEnumerable<Tuple<string, int, int, int>> GetColorsOfShopperProductDataChart(string shopperProductId) =>
            _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                .SelectMany(c => c.ShopperProductColors)
                .Select(c => new Tuple<string, int, int, int>(
                    c.Color.ColorName,
                    10,
                    _context.OrderDetails
                        .Where(o => o.ShopperProductColorId == c.ShopperProductColorId)
                        .Sum(s => s.Count), 10));

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
