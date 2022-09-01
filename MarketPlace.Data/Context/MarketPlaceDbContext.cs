using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Contacts;
using MarketPlace.Data.Entities.SiteSettings;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MarketPlace.Data.Entities.ProductOrder;
using MarketPlace.Data.Entities.Products;
using MarketPlace.Data.Entities.Store;
using MarketPlace.Data.Entities.Wallet;

namespace MarketPlace.Data.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region Constructor

        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options)
        {

        }

        #endregion

        #region account

        public DbSet<User> Users { get; set; }

        #endregion

        #region Site

        public DbSet<Site> SiteSettings { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SiteBanner> SiteBanners { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }

        #endregion

        #region Store

        public DbSet<Seller> Sellers { get; set; }

        #endregion

        #region Products

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductColor> ProductColors { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        public DbSet<TechnicalFeatures> TechnicalFeatures { get; set; }

        public DbSet<ProductOffer> ProductOffers { get; set; }

        #endregion

        #region ProductDiscount 

        public DbSet<ProductDiscount> productDiscounts { get; set; }

        public DbSet<ProductDiscountUsage> ProductDiscountUsages { get; set; }

        #endregion

        #region Contacts

        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        #region Wallet

        public DbSet<SellerWallet> SellerWallets { get; set; }

        #endregion

        #region On Model Creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationShip in modelBuilder.Model.GetEntityTypes().SelectMany(d => d.GetForeignKeys()))
            {
                relationShip.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<TicketMessage>()
                .HasOne(s => s.Sender)
                .WithMany(s => s.TicketMessages)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(s => s.Ticket)
                .WithMany(s => s.TicketMessages)
                .HasForeignKey(s => s.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductFeature>().HasQueryFilter(c => !c.IsDelete);
            //modelBuilder.Entity<OrderDetail>().HasQueryFilter(y => !y.IsDelete);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
