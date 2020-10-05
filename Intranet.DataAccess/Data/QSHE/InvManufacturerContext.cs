using Intranet.Models.QSHE;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.QSHE
{
    public class InvManufacturerContext : DbContext
    {
        public InvManufacturerContext(DbContextOptions<InvManufacturerContext> options) : base(options)
        {
        }

        public DbSet<InvManufacturer> invManufacturers { get; set; }
    }
}