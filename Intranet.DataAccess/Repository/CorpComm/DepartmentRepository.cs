using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly CorpCommDbContext _db;

        public DepartmentRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Department department)
        {
            var objFromDb = _db.Departments.FirstOrDefault(s => s.Id == department.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = department.Name;
            }
        }
    }
}