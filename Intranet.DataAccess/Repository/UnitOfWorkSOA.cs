using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.SOA;
using Intranet.DataAccess.Repository.SOA;
using Intranet.Models.SOA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository
{
    public class UnitOfWorkSOA : IUnitOfWorkSOA
    {
        private readonly SOADbContext _db;

        public UnitOfWorkSOA(SOADbContext db)
        {
            _db = db;
            ADRC = new ADRCRepository(_db);
            BKPF = new BKPFRepository(_db);
            BSAD = new BSADRepository(_db);
            BSEG = new BSEGRepository(_db);
            BSID = new BSIDRepository(_db);
            KNA1 = new KNA1Repository(_db);
            KNB1 = new KNB1Repository(_db);
            KNKK = new KNKKRepository(_db);
            KNVK = new KNVKRepository(_db);
            R_BKPF = new R_BKPFRepository(_db);
            T001 = new T001Repository(_db);
            T014 = new T014Repository(_db);
            T052 = new T052Repository(_db);

            SP_CallSOA = new SP_CallSOA(_db);
        }

        public IADRCRepository ADRC { get; private set; }
        public IBKPFRepository BKPF { get; private set; }
        public IBSADRepository BSAD { get; private set; }
        public IBSEGRepository BSEG { get; private set; }
        public IBSIDRepository BSID { get; private set; }
        public IKNA1Repository KNA1 { get; private set; }
        public IKNB1Repository KNB1 { get; private set; }
        public IKNKKRepository KNKK { get; private set; }
        public IKNVKRepository KNVK { get; private set; }
        public IR_BKPFRepository R_BKPF { get; private set; }
        public IT001Repository T001 { get; private set; }
        public IT014Repository T014 { get; private set; }
        public IT052Repository T052 { get; private set; }
        public ISP_CallSOA SP_CallSOA { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
