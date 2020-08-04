using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IEmailRepository : IRepository<Email>
    {
        void Update(Email email);
    }
}