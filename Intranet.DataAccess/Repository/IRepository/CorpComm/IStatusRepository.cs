using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IStatusRepository : IRepository<Status>
    {
        void Update(Status status);
    }
}