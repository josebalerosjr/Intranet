using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class HistoryRepository : Repository<History>, IHistoryRepository
    {
        private readonly CorpCommDbContext _db;
        public HistoryRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(History history)
        {
            var objFromDb = _db.Histories.FirstOrDefault(s => s.Id == history.Id);
            if (objFromDb != null)
            {
                objFromDb.RequestId = history.RequestId;
                objFromDb.RequestDate = history.RequestDate;
                objFromDb.LoginUser = history.LoginUser;
                objFromDb.CollateralName = history.CollateralName;
                objFromDb.CollateralId = history.CollateralId;
                objFromDb.EventType = history.EventType;
                objFromDb.Quantity = history.Quantity;
                objFromDb.StationEvent = history.StationEvent;
                objFromDb.EventDate = history.EventDate;
                objFromDb.ShippingDate = history.ShippingDate;
                objFromDb.DropOffPoint = history.DropOffPoint;
                objFromDb.rating = history.rating;
                objFromDb.RejectMarks = history.RejectMarks;
                objFromDb.ReconRemarks = history.ReconRemarks;
            }
        }
    }
}
