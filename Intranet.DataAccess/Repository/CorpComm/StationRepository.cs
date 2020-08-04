using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class StationRepository : Repository<Station>, IStationRepository
    {
        private readonly CorpCommDbContext _db;

        public StationRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Station station)
        {
            var objFromDb = _db.Stations.FirstOrDefault(s => s.Id == station.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = station.Name;
                objFromDb.StationTypeId = station.StationTypeId;
            }
        }
    }
}