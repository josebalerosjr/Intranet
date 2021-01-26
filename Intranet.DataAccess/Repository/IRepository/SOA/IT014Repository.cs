using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IT014Repository : IRepositorySOA<T014>
    {
        void Update(T014 t014);
    }
}
