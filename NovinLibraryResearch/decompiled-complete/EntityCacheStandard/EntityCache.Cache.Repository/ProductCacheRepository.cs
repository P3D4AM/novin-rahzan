using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ProductCacheRepository : DetailCacheRepository<ProductBusiness, ProductCompactBusiness>, IProductRepository, IDetailRepository<ProductBusiness>, IEditableDelivery, IDisposable
{
	private new IProductRepository DbRepository;

	private new IProductCompactRepository CompactCacheRepository;

	private IPrdAnbarRelationRepository PrdAnbarRelationRepository;

	public override IItemAsyncMapper<ProductBusiness, ProductBusiness> MapperTtoT => ProductMapper.Instance;

	public override IItemAsyncMapper<ProductBusiness, ProductCompactBusiness> MapperTtoTCompact => ProductMapper.Instance;

	public ProductCacheRepository(ProductCompactCacheRepository compactRep, IProductRepository dbRepository, IPrdAnbarRelationRepository prdAnbarRelationRepository, int CachingTimeSeconds)
		: base((ICompactCacheRepository<ProductCompactBusiness>)compactRep, (IDetailRepository<ProductBusiness>)dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
		CompactCacheRepository = compactRep;
		PrdAnbarRelationRepository = prdAnbarRelationRepository;
	}

	public Task<List<ProductBusiness>> GetVariationProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetVariationProductAsync(prdGuid, token, tranName);
	}

	public Task<ProductBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetFromDatabaseAsync(guid, token, tranName);
	}

	public async Task<ProductBusiness> GetByIdProductAsync(string IdProduct, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				ProductBusiness productBusiness = base.Entities.GetAll().SingleOrDefault((ProductBusiness x) => x.IdProduct == IdProduct);
				if (productBusiness != null)
				{
					return productBusiness;
				}
				ProductBusiness res = await DbRepository.GetByIdProductAsync(IdProduct, token, tranName);
				if (res != null)
				{
					await base.Entities.UpdateTimeAsync(res.Guid, res, tranName);
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 58);
				return null;
			}
		}
	}

	public async Task<long> GetNextCodeAsync(Guid defGroupGuid, EnSuggestionType suggestionMode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs"))
		{
			try
			{
				return await DbRepository.GetNextCodeAsync(defGroupGuid, suggestionMode, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 85);
				return 1000L;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAmountAsync(EnSystem system, ProductsChanges prd, bool rollback, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				if (base.IsDisposed)
				{
					return ret.AddError(system, "disposed!", "IsDisposed", "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 98);
				}
				line = 1;
				ProductCompactBusiness productCompactBusiness = await CompactCacheRepository.GetAsync(prd.PrdGuid, token, tranName);
				line = 2;
				if (rollback)
				{
					productCompactBusiness.Amount -= prd.qt;
				}
				else
				{
					productCompactBusiness.Amount += prd.qt;
				}
				line = 5;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await PrdAnbarRelationRepository.UpdateAccountAsync(prd, rollback, tryCount, token, tranName), ReturnedState.None, "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 107);
				line = 7;
				base.Entities.TryRemove(prd.PrdGuid, out var _);
				line = 8;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 114);
				ret.AddReturnedValue(ex, "", "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 115);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				if (items?.Any() ?? false)
				{
					line = 2;
					foreach (ProductBusiness prd in items)
					{
						line = 3;
						bool flag = prd != null;
						if (flag)
						{
							flag = await DbRepository.GetAsync(prd.Guid, token, tranName) == null;
						}
						if (flag && prd.ServerStatus == ServerStatusForClient.None)
						{
							prd.ServerStatus = ServerStatusForClient.Sent;
						}
					}
					line = 50;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await base.SaveRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 143);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//IsDisposed:{base.IsDisposed}//Items:{items?.Count() ?? (-1)}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 148);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 149);
			}
			return ret;
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		base.Entities?.ClearAll();
	}

	public Task<List<PrdAnbarRelationBusiness>> GetAnbarRelationsAsync(Guid? prdGuid, Guid? dptGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return PrdAnbarRelationRepository.GetAnbarRelationsAsync(prdGuid, dptGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAnbarRelationsAsync(IEnumerable<PrdAnbarRelationBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return PrdAnbarRelationRepository.SaveRangeAsync(lst, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAnbarRelationsAsync(IEnumerable<Guid> prdGuids, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return PrdAnbarRelationRepository.RemoveAnbarRelationsAsync(prdGuids, tryCount, token, tranName);
	}

	public Task<List<ProductBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new Exception("products.Get all is not valid");
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> productGuids, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateServerStatusAsync(productGuids, status, token, tranName);
	}

	public Task<ProductBusiness> GetBySkuAsync(string sku, CancellationToken token, string tranName)
	{
		return DbRepository.GetBySkuAsync(sku, token, tranName);
	}

	public Task<ProductBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName)
	{
		return DbRepository.GetBySlugAsync(slug, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeTaxPercentAsync(IEnumerable<Guid> productGuidList, Guid? taxGuid, CancellationToken token, string tranName)
	{
		return DbRepository.ChangeTaxPercentAsync(productGuidList, taxGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateSendToServerDateAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ReNewAllCodesAsync(CancellationToken token, string tranName)
	{
		return DbRepository.ReNewAllCodesAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ReNewEmptyCodesAsync(CancellationToken token, string tranName)
	{
		return DbRepository.ReNewEmptyCodesAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateRegularPriceAsync(Guid priceLevelGuid, Guid regularPriceGuid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateRegularPriceAsync(priceLevelGuid, regularPriceGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebProduct> items, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo result = await DbRepository.SaveRangeAsync(items, token, tranName);
		ResetCache(token, tranName);
		CompactCacheRepository.ResetCache(tranName);
		return result;
	}

	public Task<HashSet<Guid>> GetAllForIndexingAsync(EnProductIndex mode, int count, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllForIndexingAsync(mode, count, token, tranName);
	}

	public Task<IEnumerable<ProductForIndexingModel>> GetAllForIndexingAsync(HashSet<Guid> guids, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllForIndexingAsync(guids, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateIndexTypeAsync(HashSet<Guid> guids, EnProductIndex mode, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateIndexTypeAsync(guids, mode, token, tranName);
	}

	public Task<IEnumerable<ProductDiscountModel>> GetAllDiscountModelAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllDiscountModelAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateDiscountPercentAsync(List<Tuple<Guid, decimal>> changes, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateDiscountPercentAsync(changes, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateStatusAsync(Guid guid, bool newStatus, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateStatusAsync(guid, newStatus, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateMasterProductAmountAsync(Guid? guid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateMasterProductAmountAsync(guid, token, tranName);
	}

	public Task<List<ProductTolidReportModel>> GetAllTolidReportAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllTolidReportAsync(productGuid, token, tranName);
	}

	public Task<ProductBusiness> GetAsync(Guid guid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetAsync(guid, depotGuid, token, tranName);
	}

	public Task<ProductBusiness> GetByMappedIdAsync(string mappedId, CancellationToken token, string tranName)
	{
		return DbRepository.GetByMappedIdAsync(mappedId, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> UpdateProductsAsync(List<ProductUpdateBaseModel> originalList, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await DbRepository.UpdateProductsAsync(originalList, token, tranName), ReturnedState.None, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 212);
			if (!res.HasError)
			{
				ResetCache(token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 217);
			res.AddReturnedValue(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductCacheRepository.cs", 218);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool status, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateSpecialAsync(groupGuid, status, token, tranName);
	}

	public Task<List<ProductAttributeAmountModel>> GetAllAttributeAmountAsync(Guid productGuid, Guid rowAttributeGuid, Guid columnAttributeGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAttributeAmountAsync(productGuid, rowAttributeGuid, columnAttributeGuid, token, tranName);
	}
}
