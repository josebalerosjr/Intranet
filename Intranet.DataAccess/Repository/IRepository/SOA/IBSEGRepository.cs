using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IBSEGRepository : IRepositorySOA<BSEG>
    {
        void Update(BSEG bSEG);
    }
}
