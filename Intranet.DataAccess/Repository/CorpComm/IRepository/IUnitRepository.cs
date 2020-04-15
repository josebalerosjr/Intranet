using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm.IRepository
{
    public interface IUnitRepository : IRepository<Unit>
    {
        void Update(Unit unit);
    }
}
