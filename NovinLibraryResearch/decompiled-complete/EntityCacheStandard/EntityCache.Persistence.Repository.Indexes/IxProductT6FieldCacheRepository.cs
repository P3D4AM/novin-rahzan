using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT6FieldCacheRepository : IxCacheRepository, IIxProductT6FieldRepository, IIxRepository, IDisposable
{
	public IxProductT6FieldCacheRepository(IIxProductT6FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
