using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Selection;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Selection;

internal class ProductGroupRelationPersistenceRepositoryBase : PersistenceNoGuidRepository<ProductGroupRelationBusiness, ProductGroupRelation>, IProductGroupRelationRepository, INoGuidRepository<ProductGroupRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductGroupRelationBusiness, ProductGroupRelation>, IItemAsyncMapper<ProductGroupRelation, ProductGroupRelationBusiness>
	{
		public Task<ProductGroupRelation> MapAsync(string contextGuid, ProductGroupRelationBusiness source, ProductGroupRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductGroupRelation();
				}
				destination.ProductGuid = source.ProductGuid;
				destination.GroupGuid = source.GroupGuid;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 213);
				throw;
			}
		}

		public Task<ProductGroupRelationBusiness> MapAsync(string contextGuid, ProductGroupRelation source, ProductGroupRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductGroupRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.GroupGuid = source.GroupGuid;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 237);
				throw;
			}
		}

		public Task<List<ProductGroupRelation>> MapListAsync(string contextGuid, IEnumerable<ProductGroupRelationBusiness> source, List<ProductGroupRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductGroupRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductGroupRelation> source, List<ProductGroupRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public override IItemAsyncMapper<ProductGroupRelation, ProductGroupRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductGroupRelationBusiness, ProductGroupRelation> MapToDatabase => Mapper;

	public ProductGroupRelationPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 42);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ProductGroupRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 50));
			ProductGroupRelation productGroupRelation = await MapToDatabase.MapAsync(item.ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(productGroupRelation, productGroupRelation.ProductGuid, productGroupRelation.GroupGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 52);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 56);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 57);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<ProductGroupRelationBusiness> list, Guid productGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (productGuid == Guid.Empty)
			{
				return res.AddError("ProductGuid is NotValid !!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 66);
			}
			if ((list?.Where((ProductGroupRelationBusiness q) => q.ProductGuid != productGuid))?.Any() ?? false)
			{
				return res.AddError("GroupList is NotValid !!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 68);
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 70));
			IQueryable<ProductGroupRelation> queryable = ((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation q) => q.ProductGuid == productGuid);
			if (queryable != null && queryable.Any())
			{
				List<Guid> existGuidList = list?.Select((ProductGroupRelationBusiness q) => q.GroupGuid)?.ToList();
				if (existGuidList?.Any() ?? false)
				{
					IQueryable<ProductGroupRelation> queryable2 = queryable.Where((ProductGroupRelation q) => !existGuidList.Contains(q.GroupGuid));
					if (queryable2 != null && queryable2.Any())
					{
						c.PrdGroupRelation.RemoveRange((IEnumerable<ProductGroupRelation>)queryable2);
					}
				}
			}
			if (res.HasError || !(list?.Any() ?? false))
			{
				return res;
			}
			if (list.Max((ProductGroupRelationBusiness q) => q.Ordinal) != list.Count)
			{
				int num = 1;
				foreach (ProductGroupRelationBusiness item2 in list)
				{
					item2.Ordinal = (short)num++;
				}
			}
			foreach (ProductGroupRelationBusiness item in list)
			{
				if (res.HasError)
				{
					return res;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				Acc_ModelContext acc_ModelContext = c;
				returnedSaveFuncInfo.AddReturnedValue(await acc_ModelContext.AddOrUpdateAsync(await Mapper.MapAsync(base.ContextGuid, item, null, tranName, token), item.ProductGuid, item.GroupGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 93);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 98);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 99);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductGroupRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		try
		{
			foreach (ProductGroupRelationBusiness item in items)
			{
				if (item != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 112);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 117);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 118);
		}
		return ret;
	}

	public async Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(Guid? productGuid, Guid? groupGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 124));
		IEnumerable<ProductGroupRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ProductGroupRelation> queryable = (IQueryable<ProductGroupRelation>)c.PrdGroupRelation;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if ((productGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((ProductGroupRelation q) => q.ProductGuid == ((Guid?)productGuid).Value);
					}
					if ((groupGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((ProductGroupRelation q) => q.GroupGuid == ((Guid?)groupGuid).Value);
					}
					result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 138);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 153));
			List<ProductGroupRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroupRelation>(((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation q) => q.ProductGuid == productGuid), token);
			if (list == null || !list.Any())
			{
				return res;
			}
			c.PrdGroupRelation.RemoveRange((IEnumerable<ProductGroupRelation>)list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 160);
			res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 161);
		}
		return res;
	}

	public virtual Task<ConcurrentDictionary<Guid, List<Guid>>> GetDictionaryAsync(List<Guid> productGuidList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(List<Guid> groupGuidList, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 168));
		IEnumerable<ProductGroupRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				if (!(groupGuidList?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					IQueryable<ProductGroupRelation> prdGroupRelation = (IQueryable<ProductGroupRelation>)c.PrdGroupRelation;
					if (prdGroupRelation == null || !prdGroupRelation.Any())
					{
						result = null;
					}
					else
					{
						prdGroupRelation = prdGroupRelation?.Where((ProductGroupRelation q) => groupGuidList.Contains(q.GroupGuid));
						result = ((prdGroupRelation != null && prdGroupRelation.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, prdGroupRelation, null, tranName, token)) : null);
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs", 182);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroupRelationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}
}
