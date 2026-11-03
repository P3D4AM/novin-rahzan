using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Filters;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ProductCompactCacheRepository : CompactCacheRepository<ProductCompactBusiness>, IProductCompactRepository, ICompactRepository<ProductCompactBusiness>, IDisposable
{
	private List<ProductCompactBusiness> _getLast20;

	private DateTime _getLast20CachedTime = DateTime.Now;

	public new IProductCompactRepository DbRepository => (IProductCompactRepository)base.DbRepository;

	public ProductCompactCacheRepository(IProductCompactRepository dbRepository, int cachingTime)
		: base((ICompactRepository<ProductCompactBusiness>)dbRepository, cachingTime, (IItemAsyncMapper<ProductCompactBusiness, ProductCompactBusiness>)ProductMapper.Instance)
	{
	}

	public Task<IEnumerable<ProductCompactBusiness>> GetVariationProductAsync(Guid prdguid, CancellationToken token, string tranName)
	{
		return DbRepository.GetVariationProductAsync(prdguid, token, tranName);
	}

	public async Task<ProductCompactBusiness> GetAsync(Guid guid, bool? status, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (TimingCache.TryGetValue(guid, out var ret))
			{
				return ret;
			}
			ret = await DbRepository.GetAsync(guid, status, token, tranName);
			if (ret != null)
			{
				await TimingCache.UpdateTimeAsync(ret.Guid, ret, tranName);
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "Get By Guid \r\n" + $"CacheIsNullOrDispose:{ClsCache.GetCache(base.ContextGuid)?.IsDisposed ?? true}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 44);
			throw;
		}
	}

	public override Task<ProductCompactBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, true, token, tranName);
	}

	public Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, Guid guid, bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.GetAsync(depotGuid, guid, status, token, tranName);
	}

	public Task<IEnumerable<ProductPriceReportBusiness>> GetAllProductPriceModelAsync(ProductPriceReportConfig config, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllProductPriceModelAsync(config, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 60);
		}
	}

	protected new async Task LoadCacheAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = -1;
		using (new JaegerMonitor("LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				if (base.isCached)
				{
					return;
				}
				await Locks.DbReadSemaphore.WaitAsync();
				try
				{
					if (base.isCached)
					{
						return;
					}
					base.isCached = true;
					List<ProductCompactBusiness> list = await DbRepository.GetAllAsync(true, token, tranName);
					line = 20;
					if (list == null)
					{
						return;
					}
					foreach (ProductCompactBusiness item in list)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
					}
				}
				finally
				{
					Locks.DbReadSemaphore.Release();
					line = 40;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"new desc line:{line}", "LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 93);
				throw;
			}
		}
	}

	public Task<List<ProductCompactBusiness>> GetAllAsync(Guid? depotGuid, Guid? guid, Document Type, CancellationToken token, ProductCompactFilter filter, string tranName)
	{
		return DbRepository.GetAllAsync(depotGuid, guid, Type, token, filter, tranName);
	}

	public override Task<List<ProductCompactBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAsync(status, token, tranName);
	}

	public async Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, string code, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				if (string.IsNullOrEmpty(code))
				{
					return null;
				}
				line = 1;
				code = code.ToLower().FixString();
				line = 2;
				if (string.IsNullOrEmpty(code))
				{
					return null;
				}
				line = 3;
				ProductCompactBusiness productCompactBusiness = TimingCache.GetAll()?.FirstOrDefault((ProductCompactBusiness q) => q.IdProduct == code);
				if (productCompactBusiness != null)
				{
					return productCompactBusiness;
				}
				line = 5;
				ProductCompactBusiness temp = await DbRepository.GetAsync(depotGuid, code, status, token, tranName);
				line = 6;
				if (temp == null)
				{
					return null;
				}
				await TimingCache.UpdateTimeAsync(temp.Guid, temp, tranName);
				line = 7;
				return temp;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, $" byCode// #Line_{line}\r\n" + $"Cache is null or disposed:{ClsCache.GetCache(base.ContextGuid)?.IsDisposed ?? true}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 129);
				throw;
			}
		}
	}

	public async Task<IEnumerable<ProductCompactBusiness>> GetLast20Async(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs"))
		{
			try
			{
				if (DateTime.Now.Subtract(_getLast20CachedTime).TotalMinutes > 20.0)
				{
					_getLast20 = null;
				}
				if (_getLast20?.Any() ?? false)
				{
					return _getLast20;
				}
				_getLast20 = (await DbRepository.GetLast20Async(token, tranName))?.ToList();
				_getLast20CachedTime = DateTime.Now;
				return _getLast20;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 152);
				throw;
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateGroupOfProductsAsync(List<ProductGroupRelationBusiness> originalList, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateGroupOfProductsAsync(originalList, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasPicImageAsync(string fileName, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = (await GetAllAsync(true, token, tranName)).Any((ProductCompactBusiness x) => x.Pic == fileName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasPicImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 167);
			ret.AddReturnedValue(ex, "", "HasPicImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 168);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasOtherPicImageAsync(string fileName, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = (await GetAllAsync(true, token, tranName)).Any((ProductCompactBusiness x) => x.ImageListData.Contains(fileName));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasOtherPicImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 182);
			ret.AddReturnedValue(ex, "", "HasOtherPicImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 183);
		}
		return ret;
	}

	public MapperItemContainer<Guid> MapAsync(string contextGuid, MapperItemContainer<Guid> source, MapperItemContainer<Guid> destination)
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			destination.Value = source.Value;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCompactCacheRepository.cs", 200);
			throw;
		}
	}

	public void ResetCache(string tranName)
	{
		TimingCache?.ClearAll();
		base.isCached = false;
	}

	public Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySlugAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllEmptySlugAsync(token, tranName);
	}

	public Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllEmptySkuAsync(token, tranName);
	}

	public Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuOrSlugAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllEmptySkuOrSlugAsync(token, tranName);
	}

	public Task<IEnumerable<ProductCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllForSendToServerSniftsAsync(token, tranName);
	}

	public Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName)
	{
		return DbRepository.CheckExistAsync(lst, token, tranName);
	}

	public Task<ProductInfoModel> GetPrdInfoModelAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.GetPrdInfoModelAsync(guid, token, tranName);
	}

	public Task<List<ProductMicroBusiness>> GetAllAsync(ProductCompactFilter filter, Guid? guid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAsync(filter, guid, depotGuid, token, tranName);
	}

	public Task<ProductMicroBusiness> GetMicroAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.GetMicroAsync(guid, token, tranName);
	}
}
