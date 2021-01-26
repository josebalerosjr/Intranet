using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IBSIDRepository : IRepositorySOA<BSID>
    {
        void Update(BSID bSID);
    }
}
