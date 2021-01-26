using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.SOA;
using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.SOA
{
    public class KNKKRepository : RepositorySOA<KNKK>, IKNKKRepository
    {
        private readonly SOADbContext _db;

        public KNKKRepository(SOADbContext db) : base(db) {
            _db = db;
        }

        public void Update(KNKK kNKK)
        {
            //var objFromDb = _db.T001s.FirstOrDefault(s => s.Id == t001.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Name = t001.BURKS;
            //    objFromDb.Name = t001.ADRNR;
            //    objFromDb.Name = t001.BUTXT;
            //    objFromDb.Name = t001.STCEG;
            //}
        }
    }
}
