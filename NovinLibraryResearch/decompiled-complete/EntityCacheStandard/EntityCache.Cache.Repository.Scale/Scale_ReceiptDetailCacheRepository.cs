using System;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using EntityCache.Mapping.BusinessMappers.Scale;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Scale;

public class Scale_ReceiptDetailCacheRepository : CacheRepository<Scale_ReceiptDetailBusiness>, IScale_ReceiptDetailRepository, IRepository<Scale_ReceiptDetailBusiness>, INoGuidRepository<Scale_ReceiptDetailBusiness>, IDisposable
{
	private IScale_ReceiptDetailRepository _dbRepository;

	public override IItemAsyncMapper<Scale_ReceiptDetailBusiness, Scale_ReceiptDetailBusiness> Mapper => Scale_ReceiptDetailMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, Scale_ReceiptDetailBusiness> TimingCache { get; set; }

	public Scale_ReceiptDetailCacheRepository(IScale_ReceiptDetailRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<Scale_ReceiptDetailBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Scale_ReceiptDetailBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
