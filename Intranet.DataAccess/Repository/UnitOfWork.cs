using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.CorpComm;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.CorpComm;

namespace Intranet.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CorpCommDbContext _db;

        public UnitOfWork(CorpCommDbContext db)
        {
            _db = db;
            Brand = new BrandRepository(_db);
            StationType = new StationTypeRepository(_db);
            Unit = new UnitRepository(_db);
            Department = new DepartmentRepository(_db);
            Status = new StatusRepository(_db);
            Size = new SizeRepository(_db);
            Email = new EmailRepository(_db);
            Location = new LocationRepository(_db);
            Event = new EventRepository(_db);
            Station = new StationRepository(_db);
            Collateral = new CollateralRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            History = new HistoryRepository(_db);
            SP_Call = new SP_Call(_db);
        }

        public IBrandRepository Brand { get; private set; }
        public IStationTypeRepository StationType { get; private set; }
        public IUnitRepository Unit { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IStatusRepository Status { get; private set; }
        public ISizeRepository Size { get; private set; }
        public IEmailRepository Email { get; private set; }
        public ILocationRepository Location { get; private set; }
        public IEventRepository Event { get; private set; }
        public IStationRepository Station { get; private set; }
        public ICollateralRepository Collateral { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IHistoryRepository History { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}