using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.SOA;
using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.SOA
{
    public class T052Repository : RepositorySOA<T052>, IT052Repository
    {
        private readonly SOADbContext _db;

        public T052Repository(SOADbContext db) : base(db) {
            _db = db;
        }

        public void Update(T052 t052)
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
