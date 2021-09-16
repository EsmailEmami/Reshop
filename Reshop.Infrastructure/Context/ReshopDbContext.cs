using System;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;

namespace Reshop.Infrastructure.Context
{
    public class ReshopDbContext : DbContext
    {
        public ReshopDbContext(DbContextOptions<ReshopDbContext> options) : base(options)
        {
        }
        public ReshopDbContext()
        {
        }

        #region category and child category

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChildCategory> ChildCategories { get; set; }
        public virtual DbSet<ChildCategoryToCategory> ChildCategoryToCategories { get; set; }
        public virtual DbSet<ProductToChildCategory> ProductToChildCategories { get; set; }

        #endregion

        #region product

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<OfficialBrandProduct> OfficialBrandProducts { get; set; }
        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<LaptopDetail> LaptopDetails { get; set; }
        public virtual DbSet<MobileDetail> MobileDetails { get; set; }
        public virtual DbSet<TabletDetail> TabletDetails { get; set; }
        public virtual DbSet<SpeakerDetail> SpeakerDetails { get; set; }
        public virtual DbSet<AUXDetail> AuxDetails { get; set; }
        public virtual DbSet<PowerBankDetail> PowerBankDetails { get; set; }
        public virtual DbSet<WristWatchDetail> WristWatchDetails { get; set; }
        public virtual DbSet<SmartWatchDetail> SmartWatchDetails { get; set; }
        public virtual DbSet<HandsfreeAndHeadPhoneDetail> HandsfreeAndHeadPhoneDetails { get; set; }
        public virtual DbSet<FlashMemoryDetail> FlashMemoryDetails { get; set; }
        public virtual DbSet<BatteryChargerDetail> BatteryChargerDetails { get; set; }
        public virtual DbSet<MemoryCardDetail> MemoryCardDetails { get; set; }
        public virtual DbSet<MobileCoverDetail> MobileCoverDetails { get; set; }

        #endregion

        #region user

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<WalletType> WalletTypes { get; set; }
        public virtual DbSet<UserDiscountCode> UserDiscountCodes { get; set; }
        public virtual DbSet<UserInvite> UserInvites { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region shopper

        public virtual DbSet<Shopper> Shoppers { get; set; }
        public virtual DbSet<ShopperProduct> ShopperProducts { get; set; }
        public virtual DbSet<ShopperStoreTitle> ShopperStoreTitles { get; set; }
        public virtual DbSet<StoreTitle> StoreTitles { get; set; }
        public virtual DbSet<ShopperProductRequest> ShopperProductRequests { get; set; }
        public virtual DbSet<StoreAddress> StoresAddress { get; set; }
        public virtual DbSet<ShopperProductDiscount> ShopperProductDiscounts { get; set; }
        public virtual DbSet<ShopperProductColor> ShopperProductColors { get; set; }
        public virtual DbSet<ShopperProductColorRequest> ShopperProductColorRequests { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductToChildCategory>()
                .HasKey(c => new { c.ProductId, c.ChildCategoryId });

            modelBuilder.Entity<ChildCategoryToCategory>()
                .HasKey(c => new { c.CategoryId, c.ChildCategoryId });

            modelBuilder.Entity<UserDiscountCode>()
                .HasKey(c => new { c.UserId, c.DiscountId });

            modelBuilder.Entity<UserRole>()
                .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<ShopperStoreTitle>()
                .HasKey(c => new { c.ShopperId, c.StoreTitleId });

            modelBuilder.Entity<ProductGallery>()
                .HasKey(c => new { c.ProductId, c.ImageName });

            modelBuilder.Entity<RolePermission>()
                .HasKey(c => new { c.RoleId, c.PermissionId });

            modelBuilder.Entity<Order>(i =>
            {
                i.Property(w => w.Sum).HasColumnType("Money");
                i.Property(w => w.OrderDiscount).HasColumnType("Money");
                i.Property(w => w.ShippingCost).HasColumnType("Money");
            });

            modelBuilder.Entity<OrderDetail>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
                i.Property(w => w.ProductDiscountPrice).HasColumnType("Money");
                i.Property(w => w.Sum).HasColumnType("Money");
            });

            modelBuilder.Entity<ShopperProductColorRequest>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });

            modelBuilder.Entity<Wallet>(i =>
            {
                i.Property(w => w.Amount).HasColumnType("Money");
            });

            modelBuilder.Entity<User>(i =>
            {
                i.Property(w => w.AccountBalance).HasColumnType("Money");
            });


            modelBuilder.Entity<ShopperProductColor>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });


            //#region data seed

            //modelBuilder.Entity<Brand>().HasData(new Brand()
            //{
            //    BrandId = 1,
            //    BrandName = "brandName1221"
            //});

            //modelBuilder.Entity<OfficialBrandProduct>().HasData(new OfficialBrandProduct()
            //{
            //    OfficialBrandProductId = 1,
            //    OfficialBrandProductName = "OfficialBrand1221",
            //    BrandId = 1,
            //});

            //modelBuilder.Entity<AUXDetail>().HasData(new AUXDetail()
            //{
            //    AUXDetailId = 1,
            //    CableLenght = 2,
            //    CableMaterial = "CableMaterial1221"
            //}, new AUXDetail()
            //{
            //    AUXDetailId = 2,
            //    CableLenght = 2.6,
            //    CableMaterial = "CableMaterial1221"
            //});


            //modelBuilder.Entity<Product>().HasData(new Product()
            //{
            //    ProductId = 1,
            //    ProductTitle = "baby1",
            //    Description = "hello baby1",
            //    ShortKey = "ck6u",
            //    ProductType = "AUX",
            //    BrandId = 1,
            //    OfficialBrandProductId = 1,
            //    CreateDate = DateTime.Now,
            //    IsActive = true,
            //    AuxDetailId = 1,
            //}, new Product()
            //{
            //    ProductId = 2,
            //    ProductTitle = "baby2",
            //    Description = "hello baby2",
            //    ProductType = "AUX",
            //    ShortKey = "ck6a",
            //    BrandId = 1,
            //    OfficialBrandProductId = 2,
            //    CreateDate = DateTime.Now,
            //    IsActive = true,
            //    AuxDetailId = 2,
            //});



            //#endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
