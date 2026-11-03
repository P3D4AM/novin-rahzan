using System;
using EntityCache.Business.Accounting.Coding;

namespace EntityCache.Core.Repository.Accounting.Coding;

public interface IAccountLogRepository : IRepository<AccountLogBusiness>, INoGuidRepository<AccountLogBusiness>, IDisposable
{
}
