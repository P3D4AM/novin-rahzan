using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_Message_ReadSaleCacheRepository : CacheRepository<Scale_Message_ReadSaleBusiness>, IScale_Message_ReadSaleRepository, IRepository<Scale_Message_ReadSaleBusiness>, INoGuidRepository<Scale_Message_ReadSaleBusiness>, IDisposable
{
	private IScale_Message_ReadSaleRepository _dbRepository;

	public override IItemAsyncMapper<Scale_Message_ReadSaleBusiness, Scale_Message_ReadSaleBusiness> Mapper => Scale_Message_ReadSaleMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_Message_ReadSaleBusiness> TimingCache { get; set; }

	public Scale_Message_ReadSaleCacheRepository(IScale_Message_ReadSaleRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_Message_ReadSaleBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_Message_ReadSaleBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
