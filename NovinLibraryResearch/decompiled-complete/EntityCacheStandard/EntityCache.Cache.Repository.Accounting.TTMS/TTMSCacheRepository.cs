using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TTMS;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TTMS;
using EntityCache.Mapping.BusinessMappers.Accounting.TTMS;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.TTMS;

public class TTMSCacheRepository : CacheRepository<TTMSBusiness>, ITTMSRepository, IRepository<TTMSBusiness>, INoGuidRepository<TTMSBusiness>, IDisposable
{
	private ITTMSRepository _dbRepository;

	public override IItemAsyncMapper<TTMSBusiness, TTMSBusiness> Mapper => TTMSMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TTMSBusiness> TimingCache { get; set; }

	public TTMSCacheRepository(ITTMSRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<TTMSBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TTMSBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<TTMSReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(token, tranName);
	}
}
