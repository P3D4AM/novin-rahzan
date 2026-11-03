using System;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class ResidStatusCacheRepository : CacheRepository<ResidStatusBusiness>, IResidStatusRepository, IRepository<ResidStatusBusiness>, INoGuidRepository<ResidStatusBusiness>, IDisposable
{
	private IResidStatusRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ResidStatusBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<ResidStatusBusiness, ResidStatusBusiness> Mapper => ResidStatusMapper.Instance;

	public ResidStatusCacheRepository(IResidStatusRepository dbRepository, int CachingTime)
		: base((IRepository<ResidStatusBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ResidStatusBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ResidStatusCacheRepository.cs", 36);
		}
	}
}
