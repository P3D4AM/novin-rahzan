using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class SanadPrintCacheRepository : SyncCacheRepository<SanadPrintBusiness>, ISanadPrint, ISyncRepository<SanadPrintBusiness>, IEditableDelivery, IRepository<SanadPrintBusiness>, INoGuidRepository<SanadPrintBusiness>, IDisposable
{
	private ISanadPrint dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadPrintBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<SanadPrintBusiness, SanadPrintBusiness> Mapper => SanadPrintMapper.Instance;

	public SanadPrintCacheRepository(ISanadPrint dbRepository, int CachingTime)
		: base((ISyncRepository<SanadPrintBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SanadPrintBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\SanadPrintCacheRepository.cs", 38);
		}
	}

	public Task<SanadPrintBusiness> GetAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(asGuid, asType, token, tranName);
	}

	public Task<SanadPrintBusiness> GetAsync(long number, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(number, token, tranName);
	}
}
