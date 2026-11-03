using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductNameFieldCacheRepository : IxCacheRepository, IIxProductNameFieldRepository, IIxRepository, IDisposable
{
	public IxProductNameFieldCacheRepository(IIxProductNameFieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
