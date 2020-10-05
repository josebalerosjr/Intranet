using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class ItemRegContext : DbContext
    {
        public ItemRegContext(DbContextOptions<ItemRegContext> options) : base(options)
        {
        }

        public DbSet<ItemReg> ItemRegs { get; set; }
    }
}