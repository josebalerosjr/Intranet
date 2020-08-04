using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class StationTypeRepository : Repository<StationType>, IStationTypeRepository
    {
        private readonly CorpCommDbContext _db;

        public StationTypeRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StationType stationType)
        {
            var objFromDb = _db.StationTypes.FirstOrDefault(s => s.Id == stationType.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = stationType.Name;
            }
        }
    }
}