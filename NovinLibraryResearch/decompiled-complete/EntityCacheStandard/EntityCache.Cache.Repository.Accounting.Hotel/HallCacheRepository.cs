using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.Caching;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class HallCacheRepository : CacheRepository<HallBusiness>, IHallRepository, IRepository<HallBusiness>, INoGuidRepository<HallBusiness>, IDisposable
{
	private IHallRepository _dbRepository;

	public override IItemAsyncMapper<HallBusiness, HallBusiness> Mapper => HallMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, HallBusiness> TimingCache { get; set; }

	public HallCacheRepository(IHallRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<HallBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, HallBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<HallBusiness> GetAsync(string title, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(title, token, tranName);
	}

	public Task<IEnumerable<HallBusiness>> GetAllAsync(HallFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
