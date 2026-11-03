using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class VisitCacheRepository : SyncCacheRepository<VisitBusiness>, IVisitRepository, ISyncRepository<VisitBusiness>, IEditableDelivery, IRepository<VisitBusiness>, INoGuidRepository<VisitBusiness>, IDisposable
{
	private IVisitRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, VisitBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<VisitBusiness, VisitBusiness> Mapper => VisitMapper.Instance;

	public VisitCacheRepository(IVisitRepository _dbRepository, int CachingTime)
		: base((ISyncRepository<VisitBusiness>)_dbRepository, CachingTime, cachingEnable: true)
	{
		dbRepository = _dbRepository;
		TimingCache = new TimingCache<Guid, VisitBusiness>(base.Context?.Guid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\VisitCacheRepository.cs", 34);
		}
	}
}
