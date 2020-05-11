using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        private readonly CorpCommDbContext _db;
        public UnitRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Unit unit)
        {
            var objFromDb = _db.Units.FirstOrDefault(s => s.Id == unit.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = unit.Name;
            }
        }
    }
}
