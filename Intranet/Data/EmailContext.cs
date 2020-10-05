using Microsoft.EntityFrameworkCore;

namespace Intranet.Models
{
    public class EmailContext : DbContext
    {
        public EmailContext(DbContextOptions<EmailContext> options) : base(options)
        {
        }

        public DbSet<Email> Emails { get; set; }
    }
}