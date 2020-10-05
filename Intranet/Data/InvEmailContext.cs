using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class InvEmailContext : DbContext
    {
        public InvEmailContext(DbContextOptions<InvEmailContext> options) : base(options)
        {
        }

        public DbSet<InvEmail> invEmails { get; set; }
    }
}