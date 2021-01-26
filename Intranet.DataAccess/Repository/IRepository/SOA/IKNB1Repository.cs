using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IKNB1Repository : IRepositorySOA<KNB1>
    {
        void Update(KNB1 kNB1);
    }
}
