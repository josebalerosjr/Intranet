using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class ImageCarouselContext : DbContext
    {
        public ImageCarouselContext(DbContextOptions<ImageCarouselContext> options) : base(options)
        {
        }

        public DbSet<ImageCarousel> imageCarousels { get; set; }
    }
}