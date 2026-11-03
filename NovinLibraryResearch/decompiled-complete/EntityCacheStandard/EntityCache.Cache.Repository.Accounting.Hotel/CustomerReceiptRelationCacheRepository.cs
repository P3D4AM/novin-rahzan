using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class CustomerReceiptRelationCacheRepository : CacheRepositoryNoGuiId<CustomerReceiptRelationBusiness, string>, ICustomerReceiptRelationRepository, INoGuidRepository<CustomerReceiptRelationBusiness>, IDisposable
{
	private ICustomerReceiptRelationRepository _dbRepository;

	public override IItemAsyncMapper<CustomerReceiptRelationBusiness, CustomerReceiptRelationBusiness> Mapper => CustomerReceiptRelationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, CustomerReceiptRelationBusiness> TimingCache { get; set; }

	public CustomerReceiptRelationCacheRepository(ICustomerReceiptRelationRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<CustomerReceiptRelationBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, CustomerReceiptRelationBusiness>(base.ContextGuid, Mapper, cachingTime);
	}

	public Task<IEnumerable<CustomerReceiptRelationBusiness>> GetAllAsync(Guid reciptGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(reciptGuid, token, tranName);
	}
}
