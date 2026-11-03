using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class ProductDecimalRelationCacheRepository : CacheRepositoryNoGuiId<ProductDecimalRelationBusiness, string>, IProductDecimalRelationRepository, INoGuidRepository<ProductDecimalRelationBusiness>, IDisposable
{
	private IProductDecimalRelationRepository _dbRepository;

	public override IItemAsyncMapper<ProductDecimalRelationBusiness, ProductDecimalRelationBusiness> Mapper => ProductDecimalRelationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, ProductDecimalRelationBusiness> TimingCache { get; set; }

	public ProductDecimalRelationCacheRepository(IProductDecimalRelationRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<ProductDecimalRelationBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, ProductDecimalRelationBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(token, tranName);
	}
}
