using System;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IResidStatusRepository : IRepository<ResidStatusBusiness>, INoGuidRepository<ResidStatusBusiness>, IDisposable
{
}
