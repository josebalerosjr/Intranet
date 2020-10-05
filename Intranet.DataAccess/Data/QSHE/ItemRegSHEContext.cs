using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class ItemRegSHEContext : DbContext
    {
        public ItemRegSHEContext(DbContextOptions<ItemRegSHEContext> options) : base(options)
        {
        }

        public DbSet<ItemRegSHE> ItemRegSHEs { get; set; }
    }
}