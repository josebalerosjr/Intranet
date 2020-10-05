using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class InvLocationContext : DbContext
    {
        public InvLocationContext(DbContextOptions<InvLocationContext> options) : base(options)
        {
        }

        public DbSet<InvLocation> invLocations { get; set; }
    }
}