using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductDescFieldCacheRepository : IxCacheRepository, IIxProductDescFieldRepository, IIxRepository, IDisposable
{
	public IxProductDescFieldCacheRepository(IIxProductDescFieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
