using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IUnitRepository : IRepository<Unit>
    {
        void Update(Unit unit);
    }
}