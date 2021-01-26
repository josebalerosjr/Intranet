using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IT001Repository : IRepositorySOA<T001>
    {
        void Update(T001 t001);
    }
}
