using System;
using EntityCache.Business.Accounting.Payamak;

namespace EntityCache.Core.Repository.Global;

public interface ISmsOutBoxRepository : IRepository<SmsOutBoxBusiness>, INoGuidRepository<SmsOutBoxBusiness>, IDisposable
{
}
