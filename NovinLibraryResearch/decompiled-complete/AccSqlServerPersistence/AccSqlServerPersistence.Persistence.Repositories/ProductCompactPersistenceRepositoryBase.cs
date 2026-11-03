using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Filters;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class ProductCompactPersistenceRepositoryBase : PersistenceCompactRepository<ProductCompactBusiness, Product>, IProductCompactRepository, ICompactRepository<ProductCompactBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Product, ProductCompactBusiness>, IItemMapper<Product, ProductCompactBusiness>
	{
		public ProductCompactBusiness Map(string contextGuid, Product source, ProductCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new Exception("Please use projection !");
		}

		public Task<ProductCompactBusiness> MapAsync(string contextGuid, Product source, ProductCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new Exception("Please use projection !");
		}

		public Task<List<ProductCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<Product> source, List<ProductCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private SemaphoreSlim _getSemaphore = new SemaphoreSlim(1, 20);

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Product, ProductCompactBusiness> MapFromDatabase { get; set; } = new mapper();


	public override IItemMapper<Product, ProductCompactBusiness> MapFromDatabase_nonAsync { get; set; } = new mapper();


	public ProductCompactPersistenceRepositoryBase(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public abstract Task<List<ProductCompactBusiness>> GetAllAsync(Guid? depotGuid, Guid? guid, Document Type, CancellationToken token, ProductCompactFilter filter, string tranName);

	public override ProductCompactBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotSupportedException("please use asyncOverLoad...");
	}

	public Task<ProductCompactBusiness> GetAsync(Guid guid, bool? status, CancellationToken token, string tranName)
	{
		return GetAsync(null, guid, status, token, tranName);
	}

	public abstract Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, string code, bool? status, CancellationToken token, string tranName);

	public abstract Task<ReturnedSaveFuncInfo> UpdateGroupOfProductsAsync(List<ProductGroupRelationBusiness> originalList, CancellationToken token, string tranName);

	public Task<ReturnedSaveFuncInfoWithValue<bool>> HasPicImageAsync(string fileName, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Please Get Data From Cache");
	}

	public Task<ReturnedSaveFuncInfoWithValue<bool>> HasOtherPicImageAsync(string fileName, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Please Get Data From Cache");
	}

	public async Task<IEnumerable<ProductCompactBusiness>> GetVariationProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 47));
			IEnumerable<ProductCompactBusiness> result;
			try
			{
				result = (from prd in (IQueryable<Product>)acc_ModelContext.Products
					join varRel in (IEnumerable<ProductAttributeRelation>)acc_ModelContext.ProductAttributeRelations on prd.Guid equals varRel.ProductGuid
					where varRel.ProductGuid == prdGuid
					select prd into source
					select new ProductCompactBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						Guid = source.Guid,
						Modified = source.Modified,
						Amount = source.PrdAnbarRelations.Where((PrdAnbarRelation q) => q.PrdGuid == prdGuid).Sum((PrdAnbarRelation q) => q.Amount),
						Code = source.Code,
						Description = source.Description,
						DiscountPercent = source.DiscountPercent,
						IdProduct = source.IdProduct,
						IsTax = source.IsTax,
						Name = source.Name,
						Pic = source.Pic,
						Special = source.Special,
						Status = source.Status,
						V2Qt = source.V2Qt,
						GuidVahed = source.GuidVahed,
						V2Guid = source.V2Guid,
						OrderPoint = source.OrderPoint,
						ServerStatus = source.ServerStatus,
						ImageListData = source.ImageList.SplitString(",")
					})?.ToList();
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 81);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> GetScaleStatusAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetScaleStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetScaleStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 92));
			bool scaleSt;
			try
			{
				_ = 1;
				try
				{
					scaleSt = (await c.Products.FindAsync(new object[1] { guid }, token)).ScaleSt;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetScaleStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 101);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetScaleStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return scaleSt;
		}
	}

	public async Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, Guid guid, bool? status, CancellationToken token, string tranName)
	{
		await _getSemaphore.WaitAsync();
		try
		{
			using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 116));
				ProductCompactBusiness result;
				try
				{
					int num;
					_ = num - 2;
					_ = 1;
					try
					{
						if (guid == Guid.Empty)
						{
							result = null;
						}
						else
						{
							AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
							ProductCompactFilter filter = new ProductCompactFilter
							{
								SearchLocation = SearchLocation.All,
								ProductSearchOption = ProductSearchOption.All,
								Sort = ProductSort.NameAsc,
								Status = status,
								LoadJustParents = false,
								LoadBarcodeInsteadOfCode = accGlobalSettings.Products.LoadBarcodeInsteadOfCode
							};
							result = (await GetAllAsync(depotGuid, guid, Document.None, token, filter, tranName))?.SingleOrDefault();
						}
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						if (ex2.Message.Contains("Operation cancelled by user."))
						{
							throw new OperationCanceledException();
						}
						WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 136);
						throw;
					}
				}
				finally
				{
					if (string.IsNullOrEmpty(tranName))
					{
						await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
					}
				}
				return result;
			}
			IL_0403:
			ProductCompactBusiness result2;
			return result2;
		}
		finally
		{
			_getSemaphore.Release();
		}
	}

	public override Task<List<ProductCompactBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(null, null, Document.None, token, new ProductCompactFilter
		{
			Status = status,
			SearchLocation = SearchLocation.All,
			ProductSearchOption = ProductSearchOption.All,
			Sort = ProductSort.NameAsc
		}, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_getSemaphore?.Dispose();
				_getSemaphore = null;
				MapFromDatabase = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 169);
		}
	}

	public async Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 173));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				if (!(lst?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					List<Product> list = await EntityFrameworkQueryableExtensions.ToListAsync<Product>(((IQueryable<Product>)c.Products).Where((Product q) => lst.Contains(q.Guid)), token);
					result = ((list != null && list.Any()) ? list.Select((Product q) => q.Guid).ToList() : null);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 185);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public void ResetCache(string tranName)
	{
		throw new NotImplementedException();
	}

	public abstract Task<IEnumerable<ProductPriceReportBusiness>> GetAllProductPriceModelAsync(ProductPriceReportConfig config, CancellationToken token, string tranName);

	public abstract Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySlugAsync(CancellationToken token, string tranName);

	public abstract Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuAsync(CancellationToken token, string tranName);

	public abstract Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuOrSlugAsync(CancellationToken token, string tranName);

	public async Task<IEnumerable<ProductCompactBusiness>> GetLast20Async(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 203));
			IEnumerable<ProductCompactBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<ProductCompactBusiness> queryable = from source in (from x in c.Products.AsQueryable()
							where x.Status
							orderby x.CreateDate descending
							select x).Take(20)
						select new ProductCompactBusiness(ContextGuid)
						{
							ContextGuid = ContextGuid,
							Guid = source.Guid,
							Modified = source.Modified,
							Code = source.Code,
							Description = source.Description,
							DiscountPercent = source.DiscountPercent,
							IdProduct = source.IdProduct,
							IsTax = source.IsTax,
							Name = source.Name,
							Pic = source.Pic,
							Special = source.Special,
							Status = source.Status,
							V2Qt = source.V2Qt,
							GuidVahed = source.GuidVahed,
							V2Guid = source.V2Guid,
							OrderPoint = source.OrderPoint,
							ServerStatus = source.ServerStatus,
							ImageListData = source.ImageList.SplitString(",")
						};
					result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ProductCompactBusiness>(queryable, token) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 252);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLast20Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<ProductCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 265));
			IEnumerable<ProductCompactBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Product> queryable = ((IQueryable<Product>)c.Products)?.Where((Product q) => q.SendToServerDate == null || q.SendToServerDate < q.Modified)?.OrderBy((Product q) => Guid.NewGuid())?.Take(50);
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<ProductCompactBusinessWithEmptyConstructor>(queryable.Select((Product source) => new ProductCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Guid = source.Guid,
						Modified = source.Modified,
						Code = source.Code,
						Description = source.Description,
						DiscountPercent = source.DiscountPercent,
						IdProduct = source.IdProduct,
						IsTax = source.IsTax,
						Name = source.Name,
						Pic = source.Pic,
						Special = source.Special,
						Status = source.Status,
						V2Qt = source.V2Qt,
						GuidVahed = source.GuidVahed,
						V2Guid = source.V2Guid,
						OrderPoint = source.OrderPoint,
						ServerStatus = source.ServerStatus,
						ImageListData = source.ImageList.SplitString(","),
						Type = source.Type,
						VahedName = source.VahedItem.Name
					}), token)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 315);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ProductInfoModel> GetPrdInfoModelAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetPrdInfoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetPrdInfoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 328));
			ProductInfoModel result;
			try
			{
				_ = 1;
				try
				{
					Product product = await c.Products.FindAsync(new object[1] { guid }, token);
					result = new ProductInfoModel
					{
						BPercent1 = product.BPercent1,
						BPercent2 = product.BPercent2,
						BPercent3 = product.BPercent3,
						BPercent4 = product.BPercent4,
						IsSerial = product.IsSerial,
						ScaleSt = product.ScaleSt,
						V3Guid = product.V3Guid,
						V3Qt = product.V3Qt,
						V4Guid = product.V4Guid,
						V4Qt = product.V4Qt,
						TolidFormulaGuid = product.TolidFormulaGuid,
						Mojodi = product.Mojodi
					};
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetPrdInfoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 352);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetPrdInfoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<List<ProductMicroBusiness>> GetAllAsync(ProductCompactFilter filter, Guid? guid, Guid? depotGuid, CancellationToken token, string tranName);

	public async Task<ProductMicroBusiness> GetMicroAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetMicroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetMicroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 366));
			ProductMicroBusiness result;
			try
			{
				_ = 1;
				try
				{
					Product product = await c.Products.FindAsync(new object[1] { guid });
					result = ((product != null) ? new ProductMicroBusiness(base.ContextGuid)
					{
						Code = product.Code,
						ContextGuid = base.ContextGuid,
						Description = product.Description,
						DiscountPercent = product.DiscountPercent,
						Guid = guid,
						IdProduct = product.IdProduct,
						Kind = product.Kind,
						MasterGuid = product.MasterGuid,
						Modified = product.Modified,
						Name = product.Name,
						Special = product.Special,
						Status = product.Status,
						Type = product.Type,
						V2Guid = product.V2Guid,
						V2Qt = product.V2Qt
					} : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetMicroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs", 394);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetMicroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}
}
