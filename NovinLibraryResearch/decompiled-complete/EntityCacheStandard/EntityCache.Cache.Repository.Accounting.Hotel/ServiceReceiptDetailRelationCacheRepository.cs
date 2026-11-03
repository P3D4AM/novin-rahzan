using System;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class ServiceReceiptDetailRelationCacheRepository : CacheRepositoryNoGuiId<ServiceReceiptDetailRelationBusiness, string>, IServiceReceiptDetailRelationRepository, INoGuidRepository<ServiceReceiptDetailRelationBusiness>, IDisposable
{
	private IServiceReceiptDetailRelationRepository _dbRepository;

	public override IItemAsyncMapper<ServiceReceiptDetailRelationBusiness, ServiceReceiptDetailRelationBusiness> Mapper => ServiceReceieptDetailRelationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, ServiceReceiptDetailRelationBusiness> TimingCache { get; set; }

	public ServiceReceiptDetailRelationCacheRepository(IServiceReceiptDetailRelationRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<ServiceReceiptDetailRelationBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, ServiceReceiptDetailRelationBusiness>(base.ContextGuid, Mapper, cachingTime);
	}
}
