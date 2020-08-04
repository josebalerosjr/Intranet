using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface ILocationRepository : IRepository<Location>
    {
        void Update(Location location);
    }
}