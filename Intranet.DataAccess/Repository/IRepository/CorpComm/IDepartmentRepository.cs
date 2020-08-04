using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        void Update(Department department);
    }
}