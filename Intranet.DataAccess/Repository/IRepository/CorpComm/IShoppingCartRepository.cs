using Intranet.Models.CorpComm;

namespace Intranet.DataAccess.Repository.IRepository.CorpComm
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}