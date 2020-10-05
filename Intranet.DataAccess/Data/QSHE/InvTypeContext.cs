using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class InvTypeContext : DbContext
    {
        public InvTypeContext(DbContextOptions<InvTypeContext> options) : base(options)
        {
        }

        public DbSet<InvType> invTypes { get; set; }
    }
}