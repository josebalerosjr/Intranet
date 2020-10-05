using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class InvUnitContext : DbContext
    {
        public InvUnitContext(DbContextOptions<InvUnitContext> options) : base(options)
        {
        }

        public DbSet<InvUnit> invUnits { get; set; }
    }
}