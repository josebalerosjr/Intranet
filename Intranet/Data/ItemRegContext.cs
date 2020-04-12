using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class ItemRegContext : DbContext
    {
        public ItemRegContext(DbContextOptions<ItemRegContext> options) : base(options)
        {
        }

        public DbSet<ItemReg> ItemRegs { get; set; }
    }
}