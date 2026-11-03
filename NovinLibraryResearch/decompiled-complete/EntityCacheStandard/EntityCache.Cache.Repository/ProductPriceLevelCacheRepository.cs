using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.Products;

namespace EntityCache.Cache.Repository;

public class ProductPriceLevelCacheRepository : SyncCacheRepository<ProductPriceLevelBusiness>, IProductPriceLevelRepository, ISyncRepository<ProductPriceLevelBusiness>, IEditableDelivery, IRepository<ProductPriceLevelBusiness>, INoGuidRepository<ProductPriceLevelBusiness>, IDisposable
{
	private IProductPriceLevelRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ProductPriceLevelBusiness, ProductPriceLevelBusiness> Mapper => ProductPriceLevelMapper.Instance;

	protected override TimingCache<Guid, ProductPriceLevelBusiness> TimingCache { get; set; }

	public ProductPriceLevelCacheRepository(IProductPriceLevelRepository dbRepository, int CachingTime)
		: base((ISyncRepository<ProductPriceLevelBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ProductPriceLevelBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Product\\ProductPriceLevelCacheRepository.cs", 42);
		}
	}

	public Task<ProductPriceLevelBusiness> GetAsync(string name, EnPriceLevelType type, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, type, token, tranName);
	}

	public Task<short> GetMaxOrdinalAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetMaxOrdinalAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> ChangeOrdinalAsync(List<ChangeOrdinalRecord> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.ChangeOrdinalAsync(list, token, tranName), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Product\\ProductPriceLevelCacheRepository.cs", 53);
			foreach (ChangeOrdinalRecord item in list)
			{
				TimingCache.TryGetValue(item.Guid, out var temp);
				if (temp != null)
				{
					ProductPriceLevelBusiness productPriceLevelBusiness = await _dbRepository.GetAsync(item.Guid, token, tranName);
					if (productPriceLevelBusiness != temp)
					{
						await Mapper.MapAsync(base.Context.Guid, productPriceLevelBusiness, temp);
					}
				}
				temp = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Product\\ProductPriceLevelCacheRepository.cs", 68);
			res.AddReturnedValue(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Product\\ProductPriceLevelCacheRepository.cs", 69);
		}
		return res;
	}

	public Task<IEnumerable<ProductPriceLevelBusiness>> GetAllAsync(ProductPriceLevelFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
