using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductAllFielsCacheRepository : IxCacheRepository, IIxProductAllFieldsRepository, IIxRepository, IDisposable
{
	public IxProductAllFielsCacheRepository(IIxProductAllFieldsRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
