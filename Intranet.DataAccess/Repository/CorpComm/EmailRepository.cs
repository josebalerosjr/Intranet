using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        private readonly CorpCommDbContext _db;

        public EmailRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Email email)
        {
            var objFromDb = _db.Emails.FirstOrDefault(s => s.Id == email.Id);
            if (objFromDb != null)
            {
                objFromDb.EmailAddress = email.EmailAddress;
            }
        }
    }
}