using Backend.Domain.EntityClass;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.Context
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        // data seed
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SiteSetting>().HasData(
                new SiteSetting()
                {
                    Id = 1,
                    CreateTime = DateTime.Now,
                    GoogleClientNumber = "",
                    GoogleScreetNumber = "",
                    Host = "https://localhost:7074/",
                    MetaTags = "",
                    SiteAdmin = "Mehmet Kelleli",
                    SiteDescriptions = "",
                    SiteLogoUrl = "",
                    SmtpHost = "outlook.office365.com",
                    SmtpPassword = "Aslanlar123",
                    SmtpPort = 587,
                    SmtpUser = "kelleli_mehmet@hotmail.com",
                });
            //bu kısımda ezilen kısım tekrar getirilir
            base.OnModelCreating(builder);
        }
    }
     
}
