using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Business.Accounting.Hotel.ReportsT;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class ReceiptCacheRepository : CacheRepository<ReceiptBusiness>, IReceiptRepository, IRepository<ReceiptBusiness>, INoGuidRepository<ReceiptBusiness>, IDisposable
{
	private IReceiptRepository _dbRepository;

	public override IItemAsyncMapper<ReceiptBusiness, ReceiptBusiness> Mapper => ReceiptMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ReceiptBusiness> TimingCache { get; set; }

	public ReceiptCacheRepository(IReceiptRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<ReceiptBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		try
		{
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, ReceiptBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Hotel\\ReceiptCacheRepository.cs", 33);
		}
	}

	public Task<int> GetNextNumberRecipt(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumberRecipt(token, tranName);
	}

	Task<IEnumerable<ReceiptReportBusiness>> IReceiptRepository.GetAllAsync(ReceiptFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
