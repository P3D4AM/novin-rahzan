using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid.Summary;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.Caching;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Tolid;

public class TolidSummaryCacheRepository : CacheRepository<TolidSummaryBusiness>, ITolidSummaryRepository, IRepository<TolidSummaryBusiness>, INoGuidRepository<TolidSummaryBusiness>, IDisposable
{
	private ITolidSummaryRepository _dbRepository;

	public override IItemAsyncMapper<TolidSummaryBusiness, TolidSummaryBusiness> Mapper => TolidSummaryMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TolidSummaryBusiness> TimingCache { get; set; }

	public TolidSummaryCacheRepository(ITolidSummaryRepository dbRepository, int CachingTime)
		: base((IRepository<TolidSummaryBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TolidSummaryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<TolidSummaryReportBusiness>> GetAllAsync(TolidSummaryFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<List<TolidSummaryDetailBusiness>> GetAllAsync(TolidSummaryBusiness master, DateTime dateM, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(master, dateM, token, tranName);
	}

	public Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.NextNumberAsync(token, tranName);
	}
}
