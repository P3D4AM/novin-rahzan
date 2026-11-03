using System;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Gift;
using EntityCache.Mapping.BusinessMappers.Accounting.Gift;
using PacketParser.Caching;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Gift;

public class GiftCacheRepository : SyncCacheRepository<GiftBusiness>, IGiftRepository, ISyncRepository<GiftBusiness>, IEditableDelivery, IRepository<GiftBusiness>, INoGuidRepository<GiftBusiness>, IDisposable
{
	private IGiftRepository _dbRepository;

	public override IItemAsyncMapper<GiftBusiness, GiftBusiness> Mapper => GiftMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, GiftBusiness> TimingCache { get; set; }

	public GiftCacheRepository(IGiftRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<GiftBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, GiftBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
