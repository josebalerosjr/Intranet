using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IR_BKPFRepository : IRepositorySOA<R_BKPF>
    {
        void Update(R_BKPF r_BKPF);
    }
}
