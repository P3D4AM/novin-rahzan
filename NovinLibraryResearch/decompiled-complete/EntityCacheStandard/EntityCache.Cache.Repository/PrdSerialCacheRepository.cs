using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products.ProductSerial;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class PrdSerialCacheRepository : SyncCacheRepository<PrdSerialBusiness>, IPrdSerialRepository, ISyncRepository<PrdSerialBusiness>, IEditableDelivery, IRepository<PrdSerialBusiness>, INoGuidRepository<PrdSerialBusiness>, IDisposable
{
	private IPrdSerialRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<PrdSerialBusiness, PrdSerialBusiness> Mapper => PrdSerialMapper.Instance;

	protected override TimingCache<Guid, PrdSerialBusiness> TimingCache { get; set; }

	public PrdSerialCacheRepository(IPrdSerialRepository dbRepository, int CachingTime)
		: base((ISyncRepository<PrdSerialBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, PrdSerialBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<PrdSerialBusiness> GetAsync(string serialNumber, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(serialNumber, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByPrdGuidAsync(Guid prdGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await dbRepository.RemoveByPrdGuidAsync(prdGuid, tryCount, token, tranName), ReturnedState.None, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdSerialCacheRepository.cs", 43);
			if (ret.HasError)
			{
				return ret;
			}
			foreach (PrdSerialBusiness item2 in await GetAllAsync(token, tranName))
			{
				if (item2.ProductGuid == prdGuid)
				{
					TimingCache.TryRemove(item2.Guid, out var _);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdSerialCacheRepository.cs", 52);
			ret.AddReturnedValue(ex, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdSerialCacheRepository.cs", 53);
		}
		return ret;
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdSerialCacheRepository.cs", 65);
		}
	}

	public Task<IEnumerable<PrdSerialBusiness>> GetAllAsync(ProductSerialFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<IEnumerable<PrdSerialDetailReportBusiness>> GetAllAsync(string serial, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(serial, token, tranName);
	}

	public Task<IEnumerable<PrdSerialBusiness>> GetAllWithoutDetailAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllWithoutDetailAsync(token, tranName);
	}

	public Task<IEnumerable<ProductSerialTechnicalInfoSuggestModel>> GetAllTechnicalSuggestionAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllTechnicalSuggestionAsync(token, tranName);
	}

	public Task<IEnumerable<PrdSerialReportBusiness>> GetAllModelAsync(ProductSerialFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllModelAsync(filter, token, tranName);
	}
}
