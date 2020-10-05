using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class ItemRegSHEContext : DbContext
    {
        public ItemRegSHEContext(DbContextOptions<ItemRegSHEContext> options) : base(options)
        {
        }

        public DbSet<ItemRegSHE> ItemRegSHEs { get; set; }
    }
}