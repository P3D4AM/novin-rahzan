using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductCodeFieldCacheRepository : IxCacheRepository, IIxProductCodeFieldRepository, IIxRepository, IDisposable
{
	public IxProductCodeFieldCacheRepository(IIxProductCodeFieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
