using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.SOA;
using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository
{
    public interface IUnitOfWorkSOA : IDisposable
    {
        IADRCRepository ADRC { get; }
        IBKPFRepository BKPF { get; }
        IBSADRepository BSAD { get; }
        IBSEGRepository BSEG { get; }
        IBSIDRepository BSID { get; }
        IKNA1Repository KNA1 { get; }
        IKNB1Repository KNB1 { get; }
        IKNKKRepository KNKK { get; }
        IKNVKRepository KNVK { get; }
        IR_BKPFRepository R_BKPF { get; }
        IT001Repository T001 { get; }
        IT014Repository T014 { get; }
        IT052Repository T052 { get; }
        ISP_CallSOA SP_CallSOA { get; }

        void Save();
    }
}
