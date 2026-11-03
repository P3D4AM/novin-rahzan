using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT1FieldCacheRepository : IxCacheRepository, IIxProductT1FieldRepository, IIxRepository, IDisposable
{
	public IxProductT1FieldCacheRepository(IIxProductT1FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
