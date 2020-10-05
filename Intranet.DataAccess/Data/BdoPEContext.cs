using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class BdoPEContext : DbContext
    {
        public BdoPEContext(DbContextOptions<BdoPEContext> options) : base(options)
        {
        }

        public DbSet<BdoPE> bdoPEs { get; set; }
    }
}