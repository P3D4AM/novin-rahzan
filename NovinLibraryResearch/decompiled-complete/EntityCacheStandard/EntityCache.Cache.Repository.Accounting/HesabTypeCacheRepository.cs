using System;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class HesabTypeCacheRepository : CacheRepository<HesabTypeBusiness>, IHesabTypeRepository, IRepository<HesabTypeBusiness>, INoGuidRepository<HesabTypeBusiness>, IDisposable
{
	private IHesabTypeRepository dbRepository;

	protected override TimingCache<Guid, HesabTypeBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<HesabTypeBusiness, HesabTypeBusiness> Mapper => HesabTypeMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public HesabTypeCacheRepository(IHesabTypeRepository dbRepository, int CachingTime)
		: base((IRepository<HesabTypeBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, HesabTypeBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\HesabTypeCacheRepository.cs", 35);
		}
	}
}
