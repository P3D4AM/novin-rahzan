using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class PushNotificationCacheRepository : CacheRepository<PushNotificationBusiness>, IPushNotificationRepository, IRepository<PushNotificationBusiness>, INoGuidRepository<PushNotificationBusiness>, IDisposable
{
	private IPushNotificationRepository dbRepository;

	protected override TimingCache<Guid, PushNotificationBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<PushNotificationBusiness, PushNotificationBusiness> Mapper => PushNotificationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public PushNotificationCacheRepository(IPushNotificationRepository dbRepository, int CachingTime)
		: base((IRepository<PushNotificationBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, PushNotificationBusiness>(base.ContextGuid, Mapper, CachingTime);
		this.dbRepository = dbRepository;
	}

	public Task<ReturnedSaveFuncInfoWithValue<List<PushNotificationBusiness>>> GetAllAsync(CancellationToken pToken)
	{
		throw new NotImplementedException();
	}

	public Task<DateTime> GetLastModifiedDate(CancellationToken token, string tranName)
	{
		return dbRepository.GetLastModifiedDate(token, tranName);
	}

	public Task<IEnumerable<PushNotificationBusiness>> GetActiveNotifcation(ENSource source, CancellationToken token, string tranName)
	{
		return dbRepository.GetActiveNotifcation(source, token, tranName);
	}
}
