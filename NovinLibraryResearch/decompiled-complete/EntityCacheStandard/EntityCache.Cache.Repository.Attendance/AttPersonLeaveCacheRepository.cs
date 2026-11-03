using System;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.Caching;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class AttPersonLeaveCacheRepository : SyncCacheRepository<AttPersonLeaveBusiness>, IAttPersonLeaveRepository, ISyncRepository<AttPersonLeaveBusiness>, IEditableDelivery, IRepository<AttPersonLeaveBusiness>, INoGuidRepository<AttPersonLeaveBusiness>, IDisposable
{
	private IAttPersonLeaveRepository _dbRepository;

	public override IItemAsyncMapper<AttPersonLeaveBusiness, AttPersonLeaveBusiness> Mapper => AttPersonLeaveMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, AttPersonLeaveBusiness> TimingCache { get; set; }

	public AttPersonLeaveCacheRepository(IAttPersonLeaveRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<AttPersonLeaveBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, AttPersonLeaveBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
