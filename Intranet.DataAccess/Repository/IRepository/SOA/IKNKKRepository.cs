using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.SOA
{
    public interface IKNKKRepository : IRepositorySOA<KNKK>
    {
        void Update(KNKK kNKK);
    }
}
