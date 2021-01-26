using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IBKPFRepository : IRepositorySOA<BKPF>
    {
        void Update(BKPF bKPF);
    }
}
