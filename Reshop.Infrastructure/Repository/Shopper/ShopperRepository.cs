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

        public async Task<AddOrEditShopperViewModel> GetShopperDataForEditAsync(string userId)
            =>
                await _context.Users.Where(c => c.UserId == userId)
                    .Include(c => c.Shopper)
                    .Select(c => new AddOrEditShopperViewModel()
                    {
                        UserId = c.UserId,
                        FullName = c.FullName,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        LandlinePhoneNumber = c.Shopper.LandlinePhoneNumber,
                        NationalCode = c.NationalCode,
                        PostalCode = c.PostalCode,
                        BirthDay = c.Shopper.BirthDay.ToString(),
                        StoreName = c.Shopper.StoreName,

                    }).SingleOrDefaultAsync();

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperUserId, int productId)
            =>
                await _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId).SingleOrDefaultAsync();

        public void UpdateShopperProduct(ShopperProduct shopperProduct) => _context.ShopperProducts.Update(shopperProduct);

        public async Task AddShopperProductAsync(ShopperProduct shopperProduct) =>
            await _context.ShopperProducts.AddAsync(shopperProduct);

        public IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId) => 
            _context.ShopperProducts.Where(c => c.ProductId == productId)
                .Select(c => new Tuple<string, string, string>(c.ShopperUserId, c.User.Shopper.StoreName, c.Warranty));

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


        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
