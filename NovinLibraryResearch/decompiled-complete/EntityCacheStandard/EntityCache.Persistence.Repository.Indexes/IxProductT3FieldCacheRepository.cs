using System;
using EntityCache.Cache;
using EntityCache.Core.Repository.Indexes;

namespace EntityCache.Persistence.Repository.Indexes;

public class IxProductT3FieldCacheRepository : IxCacheRepository, IIxProductT3FieldRepository, IIxRepository, IDisposable
{
	public IxProductT3FieldCacheRepository(IIxProductT3FieldRepository dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds)
	{
	}
}
