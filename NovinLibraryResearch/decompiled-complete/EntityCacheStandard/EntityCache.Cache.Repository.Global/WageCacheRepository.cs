using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Khazane;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class WageCacheRepository : CacheRepository<WageBusiness>, IWageRepository, IRepository<WageBusiness>, INoGuidRepository<WageBusiness>, IDisposable
{
	private IWageRepository _dbRepository;

	public override IItemAsyncMapper<WageBusiness, WageBusiness> Mapper => WageMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, WageBusiness> TimingCache { get; set; }

	public WageCacheRepository(IWageRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<WageBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, WageBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<WageBusiness>> GetAllAsync(WageFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<List<WageBusiness>>> IsDuplicateCodeAsync(EnKhazaneMaster master, EnWageType typeEnteghal, decimal minPrice, decimal maxPrice, Guid guid, EnWageCalculationMethod calcMethod, CancellationToken token, string tranName)
	{
		return _dbRepository.IsDuplicateCodeAsync(master, typeEnteghal, minPrice, maxPrice, guid, calcMethod, token, tranName);
	}

	public Task<WageBusiness> GetByServerGuidAsync(Guid serverGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByServerGuidAsync(serverGuid, token, tranName);
	}
}
