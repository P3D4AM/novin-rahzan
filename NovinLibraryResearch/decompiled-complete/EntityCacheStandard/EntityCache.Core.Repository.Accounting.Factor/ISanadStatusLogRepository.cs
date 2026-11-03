using System;
using EntityCache.Business.Accounting.Factor;

namespace EntityCache.Core.Repository.Accounting.Factor;

public interface ISanadStatusLogRepository : IRepository<SanadStatusLogBusiness>, INoGuidRepository<SanadStatusLogBusiness>, IDisposable
{
}
