using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class PaymentEntryContext : DbContext
    {
        public PaymentEntryContext(DbContextOptions<PaymentEntryContext> options) : base(options)
        {
        }

        public DbSet<PaymentEntry> paymentEntry { get; set; }
    }
}