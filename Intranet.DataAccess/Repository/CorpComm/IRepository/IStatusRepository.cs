using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm.IRepository
{
    public interface IStatusRepository : IRepository<Status>
    {
        void Update(Status status);
    }
}
