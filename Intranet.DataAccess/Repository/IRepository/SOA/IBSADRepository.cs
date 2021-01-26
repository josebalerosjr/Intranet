using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IBSADRepository : IRepositorySOA<BSAD>
    {
        void Update(BSAD bSAD);
    }
}
