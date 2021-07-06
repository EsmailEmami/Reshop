using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Infrastructure.Context;

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

        public async Task<Domain.Entities.User.User> GetShopperByIdAsync(string userId)
            =>
                await _context.Users.Include(c => c.Shopper)
                    .SingleOrDefaultAsync(c => c.UserId == userId);

        public async Task AddShopperAsync(Domain.Entities.Shopper.Shopper shopper)
            =>
                await _context.Shoppers.AddAsync(shopper);

        public void EditShopper(Domain.Entities.Shopper.Shopper shopper) =>
            _context.Shoppers.Update(shopper);

        public async Task<EditShopperViewModel> GetShopperDataForEditAsync(string userId)
            =>
                await _context.Users.Where(c => c.UserId == userId)
                    .Select(c => new EditShopperViewModel()
                    {
                        UserId = c.UserId,
                        FullName = c.FullName,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        NationalCode = c.NationalCode,
                        BirthDay = c.Shopper.BirthDay.ToString(),
                        StoreName = c.Shopper.StoreName,
                        BusinessLicenseImageName = c.Shopper.BusinessLicenseImageName,
                        OnNationalCardImageName = c.Shopper.OnNationalCardImageName,
                        BackNationalCardImageName = c.Shopper.BackNationalCardImageName
                    }).SingleOrDefaultAsync();

        public string GetShopperIdOfUserByUserId(string userId) =>
            _context.Users.Find(userId).ShopperId;

        public IEnumerable<ShoppersListForAdmin> GetShoppersWithPagination(string type = "all", int skip = 0, int take = 18, string filter = null)
        {
            IQueryable<Domain.Entities.User.User> shoppers = _context.Users
                .Where(c => c.ShopperId != null)
                .Skip(skip).Take(take);


            switch (type)
            {
                case "all":
                    break;
                case "active":
                    shoppers = shoppers.Where(c => c.IsUserShopper);
                    break;
                case "block":
                    shoppers = shoppers.Where(c => !c.IsBlocked);
                    break;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                shoppers = shoppers
                    .Where(c => c.FullName.Contains(filter) ||
                                c.NationalCode.Contains(filter) ||
                                c.PhoneNumber.Contains(filter) ||
                                c.Email.Contains(filter));
            }

            return shoppers.Select(c => new ShoppersListForAdmin()
            {
                ShopperUserId = c.UserId,
                ShopperName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                StoreName = c.Shopper.StoreName
            });
        }


        public void UpdateShopperProduct(ShopperProduct shopperProduct) => _context.ShopperProducts.Update(shopperProduct);

        public void RemoveShopperProduct(ShopperProduct shopperProduct) =>
            _context.ShopperProducts.Remove(shopperProduct);

        public async Task AddShopperProductAsync(ShopperProduct shopperProduct) =>
            await _context.ShopperProducts.AddAsync(shopperProduct);

        public IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId) =>
            _context.ShopperProducts.Where(c => c.ProductId == productId)
                .Select(c => new Tuple<string, string, string>(c.ShopperUserId, c.User.Shopper.StoreName, c.Warranty));

        public IEnumerable<ShopperProduct> GetProductShoppersProduct(int productId) =>
            _context.ShopperProducts.Where(c => c.ProductId == productId);

        public async Task<bool> IsShopperProductExistAsync(string shopperUserId, int productId) =>
            await _context.ShopperProducts.AnyAsync(c => c.ShopperUserId == shopperUserId && c.ProductId == productId);

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
            _context.ShopperStoreTitles.Where(c => c.ShopperId == shopperId).Select(c=> c.StoreTitle.StoreTitleName);

        public async Task AddStoreAddressAsync(StoreAddress storeAddress) =>
            await _context.StoresAddress.AddAsync(storeAddress);

        public void EditStoreAddress(StoreAddress storeAddress) =>
            _context.StoresAddress.Update(storeAddress);

        public void RemoveStoreAddress(StoreAddress storeAddress) =>
            _context.StoresAddress.Remove(storeAddress);

        public async Task<StoreAddress> GetStoreAddressByIdAsync(string storeAddressId) =>
            await _context.StoresAddress.FindAsync(storeAddressId);


        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
