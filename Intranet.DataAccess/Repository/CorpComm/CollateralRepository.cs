using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.CorpComm;
using System.Linq;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class CollateralRepository : Repository<Collateral>, ICollateralRepository
    {
        private readonly CorpCommDbContext _db;

        public CollateralRepository(CorpCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Collateral collateral)
        {
            var objFromDb = _db.Collaterals.FirstOrDefault(s => s.Id == collateral.Id);
            if (objFromDb != null)
            {
                if (collateral.ImgUrl != null)
                {
                    objFromDb.ImgUrl = collateral.ImgUrl;
                }
                objFromDb.Name = collateral.Name;
                objFromDb.Description = collateral.Description;
                objFromDb.SizeId = collateral.SizeId;
                objFromDb.UnitId = collateral.UnitId;
                objFromDb.BrandId = collateral.BrandId;
                objFromDb.LocationId = collateral.LocationId;
                objFromDb.Count = collateral.Count;
                objFromDb.Price = collateral.Price;
                objFromDb.isActive = collateral.isActive;
            }
        }
    }
}