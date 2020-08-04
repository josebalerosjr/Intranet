using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IStationTypeRepository : IRepository<StationType>
    {
        void Update(StationType stationType);
    }
}