using Intranet.Models.CorpComm;
using Intranet.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;

namespace Intranet.DataAccess.Data
{
    public class CorpCommDbContext : DbContext
    {
        public CorpCommDbContext(DbContextOptions<CorpCommDbContext> options) : base(options)
        {
        }

        public CorpCommDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(SD.ConString);
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<StationType> StationTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Collateral> Collaterals { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<History> Histories { get; set; }
    }
}