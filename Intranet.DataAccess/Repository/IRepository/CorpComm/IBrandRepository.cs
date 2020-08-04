using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IBrandRepository : IRepository<Brand>
    {
        void Update(Brand brand);
    }
}