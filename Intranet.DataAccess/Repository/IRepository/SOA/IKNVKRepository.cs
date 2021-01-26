using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IKNVKRepository : IRepositorySOA<KNVK>
    {
        void Update(KNVK kNVK);
    }
}
