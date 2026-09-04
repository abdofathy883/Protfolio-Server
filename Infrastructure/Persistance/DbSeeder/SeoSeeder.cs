using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.DbSeeder
{
    public static class SeoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<PortfolioDbContext>();

            if (!await context.SeoContents.AnyAsync())
            {
                var pages = new List<SeoContent>();
                pages.Add(new SeoContent { Route = "/", Language = Language.ar });
                pages.Add(new SeoContent { Route = "/", Language = Language.en });

                await context.SeoContents.AddRangeAsync(pages);
            }

            await context.SaveChangesAsync();
        }
    }
}
