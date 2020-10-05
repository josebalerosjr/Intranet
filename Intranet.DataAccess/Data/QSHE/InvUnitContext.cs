using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class InvUnitContext : DbContext
    {
        public InvUnitContext(DbContextOptions<InvUnitContext> options) : base(options)
        {
        }

        public DbSet<InvUnit> invUnits { get; set; }
    }
}