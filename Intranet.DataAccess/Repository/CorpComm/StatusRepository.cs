using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        private readonly CorpCommDbContext _db;

        public StatusRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Status status)
        {
            var objFromDb = _db.Statuses.FirstOrDefault(s => s.Id == status.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = status.Name;
            }
        }
    }
}