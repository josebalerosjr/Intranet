using Intranet.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.Admin
{
    public class ImageCarouselContext : DbContext
    {
        public ImageCarouselContext(DbContextOptions<ImageCarouselContext> options) : base(options)
        {
        }

        public DbSet<ImageCarousel> imageCarousels { get; set; }
    }
}