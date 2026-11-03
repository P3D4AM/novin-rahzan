using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT4FieldCacheRepository : IxCacheRepository, IIxProductT4FieldRepository, IIxRepository, IDisposable
{
	public IxProductT4FieldCacheRepository(IIxProductT4FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
