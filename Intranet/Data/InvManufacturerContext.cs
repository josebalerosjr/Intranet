using Intranet.Models;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data
{
    public class InvManufacturerContext : DbContext
    {
        public InvManufacturerContext(DbContextOptions<InvManufacturerContext> options) : base(options)
        {
        }

        public DbSet<InvManufacturer> invManufacturers { get; set; }
    }
}