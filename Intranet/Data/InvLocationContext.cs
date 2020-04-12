using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class InvLocationContext : DbContext
    {
        public InvLocationContext(DbContextOptions<InvLocationContext> options) : base(options)
        {
        }

        public DbSet<InvLocation> invLocations { get; set; }
    }
}