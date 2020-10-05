using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class InvTypeContext : DbContext
    {
        public InvTypeContext(DbContextOptions<InvTypeContext> options) : base(options)
        {
        }

        public DbSet<InvType> invTypes { get; set; }
    }
}