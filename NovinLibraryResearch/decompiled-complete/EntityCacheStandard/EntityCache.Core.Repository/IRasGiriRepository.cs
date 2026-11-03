using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IRasGiriRepository : IRepository<RasGiriBusiness>, INoGuidRepository<RasGiriBusiness>, IDisposable
{
}
