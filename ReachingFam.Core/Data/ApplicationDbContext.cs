using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Models;

namespace ReachingFam.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApprovalNotification> ApprovalNotifications { get; set; }
        public DbSet<ApprovalQueue> Approvals { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodItemOption> FoodItemOptions { get; set; }
        public DbSet<Hamper> Hampers { get; set; }
        public DbSet<HamperItem> HamperItems { get; set; }
        public DbSet<InwardItem> InwardItems { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerGiveOut> PartnerGiveOuts { get; set; }
        public DbSet<PartnerHamperItem> PartnerHamperItems { get; set; }
        public DbSet<PhotoSpeak> PhotoSpeaks { get; set; }
        public DbSet<SignIn> SignIns { get; set; }
        public DbSet<SmtpSetting> SmtpSettings { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<VolunteerGiveOut> VolunteerGiveOuts { get; set; }
        public DbSet<VolunteerHamperItem> VolunteerHamperItems { get; set; }
        public DbSet<Waste> Wastes { get; set; }
    }
}
