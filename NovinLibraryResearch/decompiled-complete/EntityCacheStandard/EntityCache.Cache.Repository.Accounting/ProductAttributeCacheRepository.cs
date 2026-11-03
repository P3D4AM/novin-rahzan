using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class ProductAttributeCacheRepository : CacheRepository<ProductAttributeBusiness>, IProductAttributeRepository, IRepository<ProductAttributeBusiness>, INoGuidRepository<ProductAttributeBusiness>, IDisposable
{
	private IProductAttributeRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ProductAttributeBusiness, ProductAttributeBusiness> Mapper => ProductAttributeMapper.Instance;

	protected override TimingCache<Guid, ProductAttributeBusiness> TimingCache { get; set; }

	public new bool IsDisposed { get; private set; }

	public ProductAttributeCacheRepository(IProductAttributeRepository dbRepository, int CachingTime)
		: base((IRepository<ProductAttributeBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ProductAttributeBusiness>(base.ContextGuid, Mapper, 0);
	}

	public override Task<IEnumerable<ProductAttributeBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(token, tranName);
	}

	public override Task<IEnumerable<ProductAttributeBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(status, token, tranName);
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(ProductAttributeBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.SaveAsync(item, tryCount, token, tranName);
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductAttributeBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(items, tryCount, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ProductAttributeCacheRepository.cs", 46);
		}
	}

	public Task<IEnumerable<ProductAttributeReportBusiness>> GetAllModelAsync(bool? status, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllModelAsync(status, token, tranName);
	}

	public Task<IEnumerable<ProductAttributeValueReportBusiness>> GetAllValueAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllValueAsync(token, tranName);
	}

	public Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetNextOrdinalAsync(token, tranName);
	}

	public Task<ProductAttributeBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(name, token, tranName);
	}
}
