using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly CorpCommDbContext _db;
        public BrandRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var objFromDb = _db.Brands.FirstOrDefault(s => s.Id == brand.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = brand.Name;
            }
        }
    }
}
