using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductNameAndCodeFieldCacheRepository : IxCacheRepository, IIxProductNameAndCodeFieldRepository, IIxRepository, IDisposable
{
	public IxProductNameAndCodeFieldCacheRepository(IIxProductNameAndCodeFieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
