using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IStatusRepository : IRepository<Status>
    {
        void Update(Status status);
    }
}