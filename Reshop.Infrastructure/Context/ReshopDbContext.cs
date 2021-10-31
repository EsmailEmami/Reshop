using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Comment;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Question;
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
