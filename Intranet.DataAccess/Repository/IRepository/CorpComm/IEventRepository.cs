using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IEventRepository : IRepository<Event>
    {
        void Update(Event @event);
    }
}