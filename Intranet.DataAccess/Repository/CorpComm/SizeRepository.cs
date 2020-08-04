using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly CorpCommDbContext _db;

        public SizeRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Size size)
        {
            var objFromDb = _db.Sizes.FirstOrDefault(s => s.Id == size.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = size.Name;
            }
        }
    }
}