using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface ICollateralRepository : IRepository<Collateral>
    {
        void Update(Collateral collateral);
    }
}
