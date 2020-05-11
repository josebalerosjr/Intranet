using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IStationTypeRepository : IRepository<StationType>
    {
        void Update(StationType stationType);
    }
}
