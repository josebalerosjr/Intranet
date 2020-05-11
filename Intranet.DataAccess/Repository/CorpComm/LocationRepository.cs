using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly CorpCommDbContext _db;
        public LocationRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Location location)
        {
            var objFromDb = _db.Locations.FirstOrDefault(s => s.Id == location.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = location.Name;
            }
        }
    }
}
