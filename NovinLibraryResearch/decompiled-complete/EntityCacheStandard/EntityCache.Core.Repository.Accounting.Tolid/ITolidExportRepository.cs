using System;
using EntityCache.Business.Accounting.Tolid;

namespace EntityCache.Core.Repository.Accounting.Tolid;

public interface ITolidExportRepository : IPersistenceMasterRepository<TolidExportBusiness>, IDisposable
{
}
