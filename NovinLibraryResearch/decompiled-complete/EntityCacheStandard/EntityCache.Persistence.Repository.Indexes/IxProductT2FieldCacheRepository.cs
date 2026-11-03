using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT2FieldCacheRepository : IxCacheRepository, IIxProductT2FieldRepository, IIxRepository, IDisposable
{
	public IxProductT2FieldCacheRepository(IIxProductT2FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
