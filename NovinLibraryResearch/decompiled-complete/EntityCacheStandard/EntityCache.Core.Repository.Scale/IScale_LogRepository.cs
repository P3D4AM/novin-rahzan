using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_LogRepository : IRepository<Scale_LogBusiness>, INoGuidRepository<Scale_LogBusiness>, IDisposable
{
}
