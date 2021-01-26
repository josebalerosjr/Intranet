using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IKNA1Repository : IRepositorySOA<KNA1>
    {
        void Update(KNA1 kNA1);
    }
}
