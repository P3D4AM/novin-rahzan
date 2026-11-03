using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_PRDRepository : IRepository<Scale_PRDBusiness>, INoGuidRepository<Scale_PRDBusiness>, IDisposable
{
}
