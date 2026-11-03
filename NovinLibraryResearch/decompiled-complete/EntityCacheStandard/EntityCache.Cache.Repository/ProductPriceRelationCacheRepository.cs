using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ProductPriceRelationCacheRepository : SyncCacheRepository<ProductPriceRelationBusiness>, IProductPriceRelationRepository, ISyncRepository<ProductPriceRelationBusiness>, IEditableDelivery, IRepository<ProductPriceRelationBusiness>, INoGuidRepository<ProductPriceRelationBusiness>, IDisposable
{
	private IProductPriceRelationRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ProductPriceRelationBusiness, ProductPriceRelationBusiness> Mapper => ProductPriceRelationMapper.Instance;

	protected override TimingCache<Guid, ProductPriceRelationBusiness> TimingCache { get; set; }

	public ProductPriceRelationCacheRepository(IProductPriceRelationRepository dbRepository, int CachingTime)
		: base((ISyncRepository<ProductPriceRelationBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ProductPriceRelationBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Product\\ProductPriceRelationCacheRepository.cs", 39);
		}
	}

	public Task<IEnumerable<ProductPriceRelationModel>> GetAllModelAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllModelAsync(filter, token, tranName);
	}

	public Task<IEnumerable<ProductPriceRelationBusiness>> GetAllAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<int> DbCountAsync(Guid? productGuid, Guid? priceLevelGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.DbCountAsync(productGuid, priceLevelGuid, token, tranName);
	}
}
