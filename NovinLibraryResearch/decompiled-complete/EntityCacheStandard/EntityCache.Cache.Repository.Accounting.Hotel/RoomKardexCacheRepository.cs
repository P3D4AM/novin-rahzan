using System;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class RoomKardexCacheRepository : CacheRepository<RoomKardexBusiness>, IRoomKardexRepository, IRepository<RoomKardexBusiness>, INoGuidRepository<RoomKardexBusiness>, IDisposable
{
	private IRoomKardexRepository _dbRepository;

	public override IItemAsyncMapper<RoomKardexBusiness, RoomKardexBusiness> Mapper => RoomKardexMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, RoomKardexBusiness> TimingCache
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public RoomKardexCacheRepository(IRoomKardexRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<RoomKardexBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, RoomKardexBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
