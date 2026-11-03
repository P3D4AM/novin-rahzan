using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order.OpenInvoices;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class OpenInvoiceCacheRepository : CacheRepository<OpenInvoiceBusiness>, IOpenInvoiceRepository, IRepository<OpenInvoiceBusiness>, INoGuidRepository<OpenInvoiceBusiness>, IDisposable, IEditableDelivery
{
	private IOpenInvoiceRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<OpenInvoiceBusiness, OpenInvoiceBusiness> Mapper => OpenInvoiceMapper.Instance;

	protected override TimingCache<Guid, OpenInvoiceBusiness> TimingCache { get; set; }

	public OpenInvoiceCacheRepository(IOpenInvoiceRepository dbRepository, int CachingTime)
		: base((IRepository<OpenInvoiceBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, OpenInvoiceBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OpenInvoices\\OpenInvoiceCacheRepository.cs", 38);
		}
	}

	public Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumberAsync(token, tranName);
	}

	public Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		return _dbRepository.Id2ToGuidAsync(id2, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<OpenInvoiceBusiness> GetByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByOrderGuidAsync(orderGuid, token, tranName);
	}
}
