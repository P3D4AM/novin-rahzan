using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_ScaleWeightCacheRepository : CacheRepository<Scale_ScaleWeightBusiness>, IScale_ScaleWeightRepository, IRepository<Scale_ScaleWeightBusiness>, INoGuidRepository<Scale_ScaleWeightBusiness>, IDisposable
{
	private IScale_ScaleWeightRepository _dbRepository;

	public override IItemAsyncMapper<Scale_ScaleWeightBusiness, Scale_ScaleWeightBusiness> Mapper => Scale_ScaleWeightMapper.Instance;

	public override bool HasStatus
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasModified
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	protected override TimingCache<Guid, Scale_ScaleWeightBusiness> TimingCache
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public Scale_ScaleWeightCacheRepository(IScale_ScaleWeightRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_ScaleWeightBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_ScaleWeightBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
