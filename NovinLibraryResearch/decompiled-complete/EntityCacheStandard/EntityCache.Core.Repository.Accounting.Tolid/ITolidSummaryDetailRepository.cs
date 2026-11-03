using System;
using EntityCache.Business.Accounting.Tolid.Summary;

namespace EntityCache.Core.Repository.Accounting.Tolid;

public interface ITolidSummaryDetailRepository : IPersistenceMasterRepository<TolidSummaryDetailBusiness>, IDisposable
{
}
