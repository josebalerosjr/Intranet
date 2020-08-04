using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IHistoryRepository : IRepository<History>
    {
        void Update(History history);
    }
}