﻿using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

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