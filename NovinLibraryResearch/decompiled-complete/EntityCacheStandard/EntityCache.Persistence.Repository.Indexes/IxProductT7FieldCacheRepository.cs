using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT7FieldCacheRepository : IxCacheRepository, IIxProductT7FieldRepository, IIxRepository, IDisposable
{
	public IxProductT7FieldCacheRepository(IIxProductT7FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
