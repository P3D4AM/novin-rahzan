using System;
using EntityCache.Business.Global;
using EntityCache.Core;

namespace EntityCache.Cache.Repository.Global;

public interface IPostRepository : IRepository<PostBusiness>, INoGuidRepository<PostBusiness>, IDisposable
{
}
