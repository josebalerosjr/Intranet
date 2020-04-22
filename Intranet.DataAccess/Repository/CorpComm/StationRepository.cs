using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
