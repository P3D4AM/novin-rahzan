using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_ReceiptHeaderCacheRepository : CacheRepository<Scale_ReceiptHeaderBusiness>, IScale_ReceiptHeaderRepository, IRepository<Scale_ReceiptHeaderBusiness>, INoGuidRepository<Scale_ReceiptHeaderBusiness>, IDisposable
{
	private IScale_ReceiptHeaderRepository _dbRepository;

	public override IItemAsyncMapper<Scale_ReceiptHeaderBusiness, Scale_ReceiptHeaderBusiness> Mapper => Scale_ReceiptHeaderMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_ReceiptHeaderBusiness> TimingCache { get; set; }

	public Scale_ReceiptHeaderCacheRepository(IScale_ReceiptHeaderRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_ReceiptHeaderBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_ReceiptHeaderBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
