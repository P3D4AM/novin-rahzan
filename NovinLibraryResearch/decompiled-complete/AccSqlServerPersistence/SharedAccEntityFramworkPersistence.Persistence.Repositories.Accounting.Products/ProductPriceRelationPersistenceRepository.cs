using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Products;

internal class ProductPriceRelationPersistenceRepository : PersistenceSyncRepository<ProductPriceRelationBusiness, ProductPriceRelation>, IProductPriceRelationRepository, ISyncRepository<ProductPriceRelationBusiness>, IEditableDelivery, IRepository<ProductPriceRelationBusiness>, INoGuidRepository<ProductPriceRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductPriceRelationBusiness, ProductPriceRelation>, IItemAsyncMapper<ProductPriceRelation, ProductPriceRelationBusiness>
	{
		public Task<ProductPriceRelation> MapAsync(string contextGuid, ProductPriceRelationBusiness source, ProductPriceRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new ProductPriceRelation();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Type = source.Type;
				destination.ProductGuid = source.ProductGuid;
				destination.Price = source.Price;
				destination.Quantity = source.Quantity;
				destination.Profit = source.Profit;
				destination.KasrRial = source.KasrRial;
				destination.PriceLevelGuid = source.PriceLevelGuid;
				destination.UnitGuid = source.UnitGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 256);
				throw;
			}
		}

		public Task<ProductPriceRelationBusiness> MapAsync(string contextGuid, ProductPriceRelation source, ProductPriceRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new ProductPriceRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Type = source.Type;
				destination.ProductGuid = source.ProductGuid;
				destination.Price = source.Price;
				destination.Quantity = source.Quantity;
				destination.Profit = source.Profit;
				destination.KasrRial = source.KasrRial;
				destination.PriceLevelGuid = source.PriceLevelGuid;
				destination.UnitGuid = source.UnitGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 291);
				throw;
			}
		}

		public Task<List<ProductPriceRelation>> MapListAsync(string contextGuid, IEnumerable<ProductPriceRelationBusiness> source, List<ProductPriceRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductPriceRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceRelation> source, List<ProductPriceRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ProductPriceRelation, ProductPriceRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductPriceRelationBusiness, ProductPriceRelation> MapToDatabase => Mapper;

	public override IItemMapper<ProductPriceRelation, ProductPriceRelationBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ProductPriceRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 43);
		}
	}

	public async Task<IEnumerable<ProductPriceRelationModel>> GetAllModelAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 47));
		IEnumerable<ProductPriceRelationModel> result;
		try
		{
			IQueryable<ProductPriceLevel> queryable = ((IQueryable<ProductPriceLevel>)acc_ModelContext.ProductPriceLevel).Where((ProductPriceLevel q) => q.Status && !q.IsDefault);
			if (filter?.Type.HasValue ?? false)
			{
				queryable = queryable?.Where((ProductPriceLevel q) => q.Type == filter.Type.Value);
			}
			if ((filter?.PriceLevelGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ProductPriceLevel q) => q.Guid == filter.PriceLevelGuid.Value);
			}
			if (queryable == null || !queryable.Any())
			{
				result = null;
			}
			else
			{
				List<ProductPriceRelationModel> list = queryable.Select((ProductPriceLevel q) => new ProductPriceRelationModel
				{
					ProductGuid = filter.ProductGuid,
					PriceLevelGuid = q.Guid,
					Ordinal = q.Ordinal,
					PriceLevelName = q.Name,
					Type = q.Type,
					Status = q.Status
				})?.ToList();
				List<ProductPriceRelation> list2 = ((IQueryable<ProductPriceRelation>)acc_ModelContext.ProductPriceRelation).Where((ProductPriceRelation q) => q.ProductGuid == filter.ProductGuid)?.ToList();
				if (list2 != null && list2.Any())
				{
					foreach (ProductPriceRelationModel item in list)
					{
						if (item != null)
						{
							ProductPriceRelation productPriceRelation = list2.FirstOrDefault((ProductPriceRelation q) => q.PriceLevelGuid == item.PriceLevelGuid);
							if (productPriceRelation != null)
							{
								item.Guid = productPriceRelation.Guid;
								item.Price = productPriceRelation.Price;
								item.Profit = productPriceRelation.Profit;
								item.KasrRial = productPriceRelation.KasrRial;
								item.Quantity = productPriceRelation.Quantity;
							}
						}
					}
				}
				foreach (ProductPriceRelationModel item2 in list)
				{
					if (item2 != null && item2.Guid == Guid.Empty)
					{
						item2.Guid = Guid.NewGuid();
					}
				}
				result = list?.OrderBy((ProductPriceRelationModel q) => q.Ordinal);
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 92);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ProductPriceRelationBusiness>> GetAllAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 103));
		IEnumerable<ProductPriceRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductPriceRelation> queryable = (IQueryable<ProductPriceRelation>)c.ProductPriceRelation;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if ((filter?.ProductGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((ProductPriceRelation q) => q.ProductGuid == filter.ProductGuid);
					}
					if ((filter?.ProductGroupGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = ((IQueryable<ProductPriceRelation>)EntityFrameworkQueryableExtensions.ThenInclude<ProductPriceRelation, Product, ICollection<ProductGroupRelation>>(EntityFrameworkQueryableExtensions.Include<ProductPriceRelation, Product>(queryable, (Expression<Func<ProductPriceRelation, Product>>)((ProductPriceRelation q) => q.ProductItem)), (Expression<Func<Product, ICollection<ProductGroupRelation>>>)((Product q) => q.ProductGroupRelations)))?.Where((ProductPriceRelation q) => q.ProductItem.ProductGroupRelations.Select((ProductGroupRelation q) => q.GroupGuid).Contains(filter.ProductGroupGuid));
					}
					result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable.ToList(), null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 121);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ProductPriceRelationBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 137));
				ProductPriceRelation productPriceRelation = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(productPriceRelation, productPriceRelation.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 139);
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 145);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 146);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<ProductPriceRelationBusiness> list, Guid productGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (productGuid == Guid.Empty)
			{
				return res.AddError("ProductGuid is NotValid !!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 155);
			}
			if ((list?.Where((ProductPriceRelationBusiness q) => q.ProductGuid != productGuid))?.Any() ?? false)
			{
				return res.AddError("PriceList is NotValid !!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 157);
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 159));
			IQueryable<ProductPriceRelation> queryable = ((IQueryable<ProductPriceRelation>)c.ProductPriceRelation).Where((ProductPriceRelation q) => q.ProductGuid == productGuid);
			if (queryable != null && queryable.Any())
			{
				List<Guid> existGuidList = list?.Select((ProductPriceRelationBusiness q) => q.Guid)?.ToList();
				IQueryable<ProductPriceRelation> queryable2 = ((!(existGuidList?.Any() ?? false)) ? queryable : queryable.Where((ProductPriceRelation q) => !existGuidList.Contains(q.Guid)));
				if (queryable2 != null && queryable2.Any())
				{
					c.ProductPriceRelation.RemoveRange((IEnumerable<ProductPriceRelation>)queryable2);
				}
			}
			if (res.HasError || !(list?.Any() ?? false))
			{
				return res;
			}
			foreach (ProductPriceRelationBusiness item in list)
			{
				if (res.HasError)
				{
					return res;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				Acc_ModelContext acc_ModelContext = c;
				returnedSaveFuncInfo.AddReturnedValue(await acc_ModelContext.AddOrUpdateAsync(await Mapper.MapAsync(ContextGuid, item, null, tranName, token), item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 173);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 178);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 179);
		}
		return res;
	}

	public async Task<int> DbCountAsync(Guid? productGuid, Guid? priceLevelGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 185));
		int result;
		try
		{
			IQueryable<ProductPriceRelation> queryable = (IQueryable<ProductPriceRelation>)acc_ModelContext.ProductPriceRelation;
			if ((productGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ProductPriceRelation q) => q.ProductGuid == ((Guid?)productGuid).Value);
			}
			if ((priceLevelGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ProductPriceRelation q) => q.PriceLevelGuid == ((Guid?)priceLevelGuid).Value);
			}
			result = queryable?.Count() ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 195);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductPriceRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (ProductPriceRelationBusiness item in items)
			{
				if (item != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 213);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 218);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceRelationPersistenceRepository.cs", 219);
		}
		return res;
	}
}
