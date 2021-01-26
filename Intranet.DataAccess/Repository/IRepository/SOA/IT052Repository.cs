using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IT052Repository : IRepositorySOA<T052>
    {
        void Update(T052 t052);
    }
}
