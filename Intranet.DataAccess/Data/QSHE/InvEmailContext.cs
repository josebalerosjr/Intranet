using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class InvEmailContext : DbContext
    {
        public InvEmailContext(DbContextOptions<InvEmailContext> options) : base(options)
        {
        }

        public DbSet<InvEmail> invEmails { get; set; }
    }
}