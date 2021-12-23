using Microsoft.EntityFrameworkCore;
using Reshop.Application.Security;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Comment;
using Reshop.Domain.Entities.Image;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Question;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;

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
        public virtual DbSet<CategoryGallery> CategoryGalleries { get; set; }
        public virtual DbSet<BrandToChildCategory> BrandToChildCategories { get; set; }

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
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderAddress> OrderAddresses { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

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

        #region Question

        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<QuestionLike> QuestionLikes { get; set; }
        public virtual DbSet<ReportQuestion> QuestionReports { get; set; }
        public virtual DbSet<QuestionAnswerLike> QuestionAnswerLikes { get; set; }
        public virtual DbSet<ReportQuestionAnswer> QuestionAnswerReports { get; set; }
        public virtual DbSet<ReportQuestionType> ReportQuestionTypes { get; set; }
        public virtual DbSet<ReportQuestionAnswerType> ReportQuestionAnswerTypes { get; set; }

        #endregion

        #region comment

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ReportComment> ReportComments { get; set; }
        public virtual DbSet<ReportCommentType> ReportCommentTypes { get; set; }
        public virtual DbSet<CommentFeedback> CommentFeedBacks { get; set; }

        #endregion

        #region image

        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImagePlace> ImagesPlace { get; set; }

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
            #region comment

            modelBuilder.Entity<CommentFeedback>()
                .HasKey(c => new { c.CommentId, c.UserId });

            modelBuilder.Entity<ReportComment>()
                .HasKey(c => new { c.CommentId, c.UserId });

            #endregion

            #region question

            modelBuilder.Entity<QuestionLike>()
                .HasKey(c => new { c.QuestionId, c.UserId });

            modelBuilder.Entity<ReportQuestion>()
                .HasKey(c => new { c.QuestionId, c.UserId });

            modelBuilder.Entity<QuestionAnswerLike>()
                .HasKey(c => new { c.QuestionAnswerId, c.UserId });

            modelBuilder.Entity<ReportQuestionAnswer>()
                .HasKey(c => new { c.QuestionAnswerId, c.UserId });

            #endregion

            #region category and childCategory

            modelBuilder.Entity<CategoryGallery>()
                .HasKey(c => new { c.CategoryId, c.ImageName });

            modelBuilder.Entity<BrandToChildCategory>()
                .HasKey(c => new { c.BrandId, c.ChildCategoryId });

            #endregion

            #region user

            modelBuilder.Entity<User>(i =>
            {
                i.Property(w => w.AccountBalance).HasColumnType("Money");
            });

            modelBuilder.Entity<Wallet>(i =>
            {
                i.Property(w => w.Amount).HasColumnType("Money");
            });

            modelBuilder.Entity<UserDiscountCode>()
                .HasKey(c => new { c.UserId, c.DiscountId });

            #endregion

            #region permission

            modelBuilder.Entity<UserRole>()
                .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(c => new { c.RoleId, c.PermissionId });

            #endregion

            #region shopper

            modelBuilder.Entity<ShopperStoreTitle>()
                .HasKey(c => new { c.ShopperId, c.StoreTitleId });

            modelBuilder.Entity<ShopperProductColorRequest>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });


            modelBuilder.Entity<ShopperProductColor>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });

            #endregion

            #region product

            modelBuilder.Entity<ProductGallery>()
                .HasKey(c => new { c.ProductId, c.ImageName });

            #endregion

            #region order

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

            #endregion

            // data seeds

            #region permission data seed

            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "32757e0d-0c77-4ecd-bf82-6888acff29f1",
                PermissionTitle = "AdminPanelMainPage",
            }, new Permission()
            {
                PermissionId = "3a86d2a6-8582-40c9-9c70-7b8c0efac6c1",
                PermissionTitle = "Shopper",
            });


            // product permissions data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "fa5205e1-7395-4c3e-a464-72e84d38975a",
                PermissionTitle = "ProductsMainPage",
            }, new Permission()
            {
                PermissionId = "4f27c20b-e51a-4152-9e7d-a5775ab969c6",
                PermissionTitle = "AddAUX",
                ParentId = "fa5205e1-7395-4c3e-a464-72e84d38975a"
            }, new Permission()
            {
                PermissionId = "5feb5422-b00a-47cd-b688-00ab6978441d",
                PermissionTitle = "EditAUX",
                ParentId = "fa5205e1-7395-4c3e-a464-72e84d38975a"
            }, new Permission()
            {
                PermissionId = "d62a9faf-087d-43a3-9bbe-66ae2737a0a5",
                PermissionTitle = "ProductDetail",
                ParentId = "fa5205e1-7395-4c3e-a464-72e84d38975a"
            }, new Permission()
            {
                PermissionId = "8db60935-056d-45a5-8044-a2e6e42e3edf",
                PermissionTitle = "ColorDetail-Product",
                ParentId = "d62a9faf-087d-43a3-9bbe-66ae2737a0a5"
            }, new Permission()
            {
                PermissionId = "91806aaf-6e67-4b3a-9554-faac7a5454f3",
                PermissionTitle = "DiscountDetail-Product",
                ParentId = "d62a9faf-087d-43a3-9bbe-66ae2737a0a5"
            });

            // brands data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "75a1d67a-8b13-472b-b93e-61186c41f5b2",
                PermissionTitle = "BrandsMainPage",
            }, new Permission()
            {
                PermissionId = "5a597555-a3ac-4c3a-8ab6-50961b43a4b6",
                PermissionTitle = "AddBrand",
                ParentId = "75a1d67a-8b13-472b-b93e-61186c41f5b2"
            }, new Permission()
            {
                PermissionId = "448e89b5-b5fe-4c65-8f73-49df8a749cc0",
                PermissionTitle = "EditBrand",
                ParentId = "75a1d67a-8b13-472b-b93e-61186c41f5b2"
            }, new Permission()
            {
                PermissionId = "d54aaed6-97f1-47fb-81ec-be6140c24f74",
                PermissionTitle = "BrandDetail",
                ParentId = "75a1d67a-8b13-472b-b93e-61186c41f5b2"
            }, new Permission()
            {
                PermissionId = "e87bb2b0-8c05-40ec-9b48-4008eeac79be",
                PermissionTitle = "AvailableBrand",
                ParentId = "75a1d67a-8b13-472b-b93e-61186c41f5b2"
            }, new Permission()
            {
                PermissionId = "c53de576-d7d5-4f62-9267-a88f17c9a952",
                PermissionTitle = "UnAvailableBrand",
                ParentId = "75a1d67a-8b13-472b-b93e-61186c41f5b2"
            });


            // official brand products data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b",
                PermissionTitle = "OfficialBrandProductsMainPage",
            }, new Permission()
            {
                PermissionId = "6d4e2f7b-a4bc-47b1-93ac-bcf9a0b3b3f0",
                PermissionTitle = "AddOfficialBrandProduct",
                ParentId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b"
            }, new Permission()
            {
                PermissionId = "526a98eb-79fd-4e54-bea8-ed7e85c0497f",
                PermissionTitle = "EditOfficialBrandProduct",
                ParentId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b"
            }, new Permission()
            {
                PermissionId = "28cf0756-89f5-4a16-a5f8-64adcb095bce",
                PermissionTitle = "OfficialBrandProductDetail",
                ParentId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b"
            }, new Permission()
            {
                PermissionId = "26eb15bc-b6c0-462e-8e7e-9652d56e549b",
                PermissionTitle = "AvailableOfficialBrandProduct",
                ParentId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b"
            }, new Permission()
            {
                PermissionId = "c06312fc-c36c-4e12-8964-5f893eeef3cf",
                PermissionTitle = "UnAvailableOfficialBrandProduct",
                ParentId = "d08d44f8-9f67-4079-b1c7-8f5bc126b66b"
            });

            // colors data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "8200832d-f2b6-4c7f-be15-b94592db8b0b",
                PermissionTitle = "ColorsMainPage",
            }, new Permission()
            {
                PermissionId = "2ce5f38d-6e9a-4de2-8fcf-dca1baf0902a",
                PermissionTitle = "AddColor",
                ParentId = "8200832d-f2b6-4c7f-be15-b94592db8b0b"
            }, new Permission()
            {
                PermissionId = "5660f404-46f3-42c9-818e-e5d6a8cc514b",
                PermissionTitle = "EditColor",
                ParentId = "8200832d-f2b6-4c7f-be15-b94592db8b0b"
            });

            // permissions data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "bef2eb25-1007-40ae-b667-dcff4c4a07a9",
                PermissionTitle = "PermissionsMainPage",
            }, new Permission()
            {
                PermissionId = "afa3e6c4-cd57-4bed-ba82-c656bed921ff",
                PermissionTitle = "AddPermission",
                ParentId = "bef2eb25-1007-40ae-b667-dcff4c4a07a9"
            }, new Permission()
            {
                PermissionId = "949826a7-070f-450a-abd8-effca32350c2",
                PermissionTitle = "EditPermission",
                ParentId = "bef2eb25-1007-40ae-b667-dcff4c4a07a9"
            }, new Permission()
            {
                PermissionId = "4c516b2a-0149-4626-8d19-c2d6ac51a425",
                PermissionTitle = "DeletePermission",
                ParentId = "bef2eb25-1007-40ae-b667-dcff4c4a07a9"
            });

            // roles data seed
            modelBuilder.Entity<Permission>().HasData(new Permission()
            {
                PermissionId = "ec223189-ef42-41bc-b01f-6d58db051e62",
                PermissionTitle = "RolesMainPage",
            }, new Permission()
            {
                PermissionId = "00dd702b-1e83-4338-ae4b-3f4db4244aa5",
                PermissionTitle = "AddRole",
                ParentId = "ec223189-ef42-41bc-b01f-6d58db051e62"
            }, new Permission()
            {
                PermissionId = "404efa94-c45f-4c50-ad3d-faaaf6b61807",
                PermissionTitle = "EditRole",
                ParentId = "ec223189-ef42-41bc-b01f-6d58db051e62"
            }, new Permission()
            {
                PermissionId = "c154f3e4-c485-47b2-8957-6d7b177ce466",
                PermissionTitle = "DeleteRole",
                ParentId = "ec223189-ef42-41bc-b01f-6d58db051e62"
            });

            #endregion

            #region role data seed

            modelBuilder.Entity<Role>().HasData(new Role()
            {
                RoleId = "5fd1d3e0-b54c-4ea1-9762-80c6483fd3f8",
                RoleTitle = "Shopper",
            }, new Role()
            {
                RoleId = "e9d0b742-79ff-4439-985e-bba8ae0d214d",
                RoleTitle = "Admin"
            });

            #endregion

            #region role permission data seed

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission()
            {
                RoleId = "e9d0b742-79ff-4439-985e-bba8ae0d214d",
                PermissionId = "32757e0d-0c77-4ecd-bf82-6888acff29f1",
            }, new RolePermission()
            {
                RoleId = "5fd1d3e0-b54c-4ea1-9762-80c6483fd3f8",
                PermissionId = "3a86d2a6-8582-40c9-9c70-7b8c0efac6c1",
            });

            #endregion

            #region user data seed

            modelBuilder.Entity<User>().HasData(new User()
            {
                UserId = "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                FullName = "کاربر پیش فرض",
                Email = "esmailemami84@gmail.com",
                UserAvatar = "userAvatar.jpg",
                PhoneNumber = "09903669556",
                InviteCode = "6D9698E6D85B4BA3AD0FC1F6B0DDD00F",
                NationalCode = "1111111111",
                RegisterDate = new DateTime(2021, 11, 20),
                IsBlocked = false,
                Password = PasswordHelper.EncodePasswordMd5("Esmail84")
            });

            #endregion

            #region origin data seed

            modelBuilder.Entity<State>().HasData(new State()
            {
                StateId = 1,
                StateName = "البرز"
            });

            modelBuilder.Entity<City>().HasData(new City()
            {
                CityId = 1,
                StateId = 1,
                CityName = "کمالشهر"
            });

            #endregion

            #region  store title data seed

            modelBuilder.Entity<StoreTitle>().HasData(new StoreTitle()
            {
                StoreTitleId = 1,
                StoreTitleName = "کالای دیجیتال"
            });

            #endregion

            #region shopper data seed

            modelBuilder.Entity<Shopper>().HasData(new Shopper()
            {
                ShopperId = "1939fee6-2a0d-4560-84aa-e7cb585bc3fb",
                UserId = "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                StoreName = "فروشگاه پیش فرض",
                BirthDay = new DateTime(2000, 11, 20),
                BusinessLicenseImageName = "",
                OnNationalCardImageName = "",
                IsActive = true,
                RegisterShopper = new DateTime(2021, 11, 20),
            });

            #endregion

            #region store address data seed

            modelBuilder.Entity<StoreAddress>().HasData(new StoreAddress()
            {
                StoreAddressId = "662e11d3-5e67-41a3-9a2c-f45bad122178",
                ShopperId = "1939fee6-2a0d-4560-84aa-e7cb585bc3fb",
                CityId = 1,
                Plaque = "14",
                StoreName = "فروشگاه پیش فرض",
                PostalCode = "1212121212",
                AddressText = "کمالشهر",
                LandlinePhoneNumber = "1212121212"
            });

            #endregion

            #region shopper store title data seed

            modelBuilder.Entity<ShopperStoreTitle>().HasData(new ShopperStoreTitle()
            {
                StoreTitleId = 1,
                ShopperId = "1939fee6-2a0d-4560-84aa-e7cb585bc3fb",
            });

            #endregion

            #region user role data seed

            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                UserId = "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                RoleId = "5fd1d3e0-b54c-4ea1-9762-80c6483fd3f8",
            }, new UserRole()
            {
                UserId = "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                RoleId = "e9d0b742-79ff-4439-985e-bba8ae0d214d",
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
