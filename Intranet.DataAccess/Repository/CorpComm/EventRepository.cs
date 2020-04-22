using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly CorpCommDbContext _db;
        public EventRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Event @event)
        {
            var objFromDb = _db.Events.FirstOrDefault(s => s.Id == @event.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = @event.Name;
                objFromDb.BudgetLimit = @event.BudgetLimit;
            }
        }
    }
}
