using System;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class FareOrderCreditCacheRepository : CacheRepository<FareOrderCreditBusiness>, IFareOrderCredit, IRepository<FareOrderCreditBusiness>, INoGuidRepository<FareOrderCreditBusiness>, IDisposable
{
	private IFareOrderCredit dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, FareOrderCreditBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<FareOrderCreditBusiness, FareOrderCreditBusiness> Mapper => FareOrderCreditMapper.Instance;

	public FareOrderCreditCacheRepository(IFareOrderCredit dbRepository, int CachingTime)
		: base((IRepository<FareOrderCreditBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, FareOrderCreditBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\FareOrderCreditCacheRepository.cs", 34);
		}
	}
}
