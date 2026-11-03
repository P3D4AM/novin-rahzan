using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Utilities;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

public class ProductAttributeRelationPersistenceRepository : PersistenceNoGuidRepository<ProductAttributeRelationBusiness, ProductAttributeRelation>, IProductAttributeRelationRepository, INoGuidRepository<ProductAttributeRelationBusiness>, IDisposable
{
	private class Mapper : IItemAsyncMapper<ProductAttributeRelationBusiness, ProductAttributeRelation>, IItemAsyncMapper<ProductAttributeRelation, ProductAttributeRelationBusiness>
	{
		public Task<ProductAttributeRelation> MapAsync(string contextGuid, ProductAttributeRelationBusiness source, ProductAttributeRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs"))
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
						destination = new ProductAttributeRelation();
					}
					destination.AttributeValueGuid = source.AttributeValueGuid;
					destination.ProductGuid = source.ProductGuid;
					destination.AttributeGuid = source.AttributeGuid;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 221);
					throw;
				}
			}
		}

		public Task<ProductAttributeRelationBusiness> MapAsync(string contextGuid, ProductAttributeRelation source, ProductAttributeRelationBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs"))
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
						destination = new ProductAttributeRelationBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.AttributeValueGuid = source.AttributeValueGuid;
					destination.ProductGuid = source.ProductGuid;
					destination.AttributeGuid = source.AttributeGuid;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 247);
					throw;
				}
			}
		}

		public Task<List<ProductAttributeRelation>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeRelationBusiness> source, List<ProductAttributeRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductAttributeRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeRelation> source, List<ProductAttributeRelationBusiness> destination, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private Mapper mapper { get; set; } = new Mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ProductAttributeRelation, ProductAttributeRelationBusiness> MapFromDatabase => mapper;

	public override IItemAsyncMapper<ProductAttributeRelationBusiness, ProductAttributeRelation> MapToDatabase => mapper;

	public ProductAttributeRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ProductAttributeRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if ((item?.ProductGuid ?? Guid.Empty) == Guid.Empty || (item?.AttributeGuid ?? Guid.Empty) == Guid.Empty || (item?.AttributeValueGuid ?? Guid.Empty) == Guid.Empty)
				{
					return ret;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 40));
				ProductAttributeRelation productAttributeRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(productAttributeRelation, productAttributeRelation.ProductGuid, productAttributeRelation.AttributeValueGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 42);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 46);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 47);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductAttributeRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs"))
		{
			try
			{
				foreach (ProductAttributeRelationBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 59);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 63);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 64);
			}
			return ret;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 77);
		}
	}

	public async Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 81));
		IEnumerable<ProductAttributeRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductAttributeRelation> queryable = ((IQueryable<ProductAttributeRelation>)c.ProductAttributeRelations).Where((ProductAttributeRelation q) => q.ProductGuid == productGuid);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 92);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllWithMasterProductGuidAsync(Guid masterProductGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllWithMasterProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 103));
		IEnumerable<ProductAttributeRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductAttributeRelation> queryable = ((IQueryable<ProductAttributeRelation>)c.ProductAttributeRelations).Where((ProductAttributeRelation q) => q.Product.MasterGuid == masterProductGuid);
				if (queryable != null && queryable.Any())
				{
					goto IL_032e;
				}
				IQueryable<Product> queryable2 = ((IQueryable<Product>)c.Products).Where((Product q) => q.MasterGuid == masterProductGuid);
				if (queryable2 != null && queryable2.Any())
				{
					queryable = queryable2.Select((Product q) => new ProductAttributeRelation
					{
						ProductGuid = q.Guid
					});
					goto IL_032e;
				}
				result = null;
				goto end_IL_00de;
				IL_032e:
				result = await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token);
				end_IL_00de:;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllWithMasterProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 119);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllWithMasterProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 135));
				IQueryable<ProductAttributeRelation> queryable = ((IQueryable<ProductAttributeRelation>)acc_ModelContext.ProductAttributeRelations).Where((ProductAttributeRelation q) => q.Product.MasterGuid == productGuid);
				if (queryable == null || !queryable.Any())
				{
					return ret;
				}
				acc_ModelContext.ProductAttributeRelations.RemoveRange((IEnumerable<ProductAttributeRelation>)queryable);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 142);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 143);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<ProductAttributeRelationModel>> GetAllModelAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 149));
		IEnumerable<ProductAttributeRelationModel> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductAttributeRelation> queryable = ((IQueryable<ProductAttributeRelation>)c.ProductAttributeRelations).Where((ProductAttributeRelation q) => q.ProductGuid == productGuid);
				result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<ProductAttributeRelationModel>(queryable.Select((ProductAttributeRelation q) => new ProductAttributeRelationModel
				{
					ProductGuid = q.ProductGuid,
					AttributeValueGuid = q.AttributeValueGuid,
					AttributeValue = $"{q.ProductAttributeValue.MasterItem.Name} {q.ProductAttributeValue.Name}",
					DeleteImage = ImageResourceManager.OrderDeleteImage,
					AttributeGuid = q.AttributeGuid
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 167);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllBtAttributeValueAsync(Guid attributeValueGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllBtAttributeValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 178));
		IEnumerable<ProductAttributeRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductAttributeRelation> queryable = ((IQueryable<ProductAttributeRelation>)c.ProductAttributeRelations).Where((ProductAttributeRelation q) => q.AttributeValueGuid == attributeValueGuid);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllBtAttributeValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs", 189);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllBtAttributeValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeRelationPersistenceRepository.cs");
			}
		}
		return result;
	}
}
