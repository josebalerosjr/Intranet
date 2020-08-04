using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface ICollateralRepository : IRepository<Collateral>
    {
        void Update(Collateral collateral);
    }
}