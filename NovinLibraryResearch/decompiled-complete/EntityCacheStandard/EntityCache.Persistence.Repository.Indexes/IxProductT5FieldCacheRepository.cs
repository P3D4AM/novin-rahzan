using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT5FieldCacheRepository : IxCacheRepository, IIxProductT5FieldRepository, IIxRepository, IDisposable
{
	public IxProductT5FieldCacheRepository(IIxProductT5FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
