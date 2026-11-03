using System;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class SanadStatusReportUpdatesCacheRepository : SyncCacheRepository<SanadStatusReportUpdatesBusiness>, ISanadStatusReportUpdatesRepository, ISyncRepository<SanadStatusReportUpdatesBusiness>, IEditableDelivery, IRepository<SanadStatusReportUpdatesBusiness>, INoGuidRepository<SanadStatusReportUpdatesBusiness>, IDisposable
{
	private ISanadStatusReportUpdatesRepository _dbRepository;

	public override IItemAsyncMapper<SanadStatusReportUpdatesBusiness, SanadStatusReportUpdatesBusiness> Mapper => SanadStatusReportUpdatesMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadStatusReportUpdatesBusiness> TimingCache { get; set; }

	public SanadStatusReportUpdatesCacheRepository(ISanadStatusReportUpdatesRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<SanadStatusReportUpdatesBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SanadStatusReportUpdatesBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\SanadStatusReportUpdatesCacheRepository.cs", 35);
		}
	}
}
