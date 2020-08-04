using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IStationRepository : IRepository<Station>
    {
        void Update(Station station);
    }
}