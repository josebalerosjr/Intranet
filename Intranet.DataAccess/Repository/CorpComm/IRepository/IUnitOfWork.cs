using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.DataAccess.Repository.CorpComm.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brand { get; }
        IStationTypeRepository StationType { get; }
        IUnitRepository Unit { get; }
        IDepartmentRepository Department { get; }
        IStatusRepository Status { get; }
        ISizeRepository Size { get; }
        IEmailRepository Email { get; }
        ILocationRepository Location { get; }
        ISP_Call SP_Call { get; }

        void Save();
    }
}