using System;
using EntityCache.Business.Attendance;

namespace EntityCache.Core.Repository.Attendance;

public interface ILogOnPersonRepository : IRepository<LogOnPersonBusiness>, INoGuidRepository<LogOnPersonBusiness>, IDisposable
{
}
