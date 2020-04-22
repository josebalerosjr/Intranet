using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly CorpCommDbContext _db;
        public OrderHeaderRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.Update(obj);
        }
    }
}
