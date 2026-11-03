using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT8FieldCacheRepository : IxCacheRepository, IIxProductT8FieldRepository, IIxRepository, IDisposable
{
	public IxProductT8FieldCacheRepository(IIxProductT8FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
