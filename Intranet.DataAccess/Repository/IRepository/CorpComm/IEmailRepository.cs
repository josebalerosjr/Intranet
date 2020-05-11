using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IEmailRepository : IRepository<Email>
    {
        void Update(Email email);
    }
}
