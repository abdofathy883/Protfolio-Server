using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class PortfolioDbContext: IdentityDbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
            
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ContactForm> ContactEntries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTranslation> ProjectTranslations { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<SeoContent> SeoContents { get; set; }
    }
}
