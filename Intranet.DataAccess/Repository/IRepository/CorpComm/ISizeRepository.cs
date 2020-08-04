using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface ISizeRepository : IRepository<Size>
    {
        void Update(Size size);
    }
}