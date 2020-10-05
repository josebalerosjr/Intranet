using Intranet.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.Admin
{
    public class EmailContext : DbContext
    {
        public EmailContext(DbContextOptions<EmailContext> options) : base(options)
        {
        }

        public DbSet<Email> Emails { get; set; }
    }
}