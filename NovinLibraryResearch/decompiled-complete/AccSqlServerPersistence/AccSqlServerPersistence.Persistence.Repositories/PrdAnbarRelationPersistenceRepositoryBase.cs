using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Models.Accounting.Prd;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class PrdAnbarRelationPersistenceRepositoryBase : PersistenceNoGuidRepository<PrdAnbarRelationBusiness, PrdAnbarRelation>, IPrdAnbarRelationRepository, INoGuidRepository<PrdAnbarRelationBusiness>, IDisposable
{
	protected class mapper : IItemAsyncMapper<PrdAnbarRelationBusiness, PrdAnbarRelation>, IItemAsyncMapper<PrdAnbarRelation, PrdAnbarRelationBusiness>
	{
		public Task<PrdAnbarRelation> MapAsync(string contextGuid, PrdAnbarRelationBusiness source, PrdAnbarRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdAnbarRelation();
				}
				destination.PrdGuid = source.PrdGuid;
				destination.AnbarGuid = source.AnbarGuid;
				destination.Min = source.Min;
				destination.Max = source.Max;
				destination.Amount = source.Amount;
				destination.Modified = source.Modified;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 178);
				throw;
			}
		}

		public Task<PrdAnbarRelationBusiness> MapAsync(string contextGuid, PrdAnbarRelation source, PrdAnbarRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdAnbarRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.PrdGuid = source.PrdGuid;
				destination.AnbarGuid = source.AnbarGuid;
				destination.Min = source.Min;
				destination.Max = source.Max;
				destination.Amount = source.Amount;
				destination.Modified = source.Modified;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 203);
				throw;
			}
		}

		public Task<List<PrdAnbarRelation>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarRelationBusiness> source, List<PrdAnbarRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdAnbarRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarRelation> source, List<PrdAnbarRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	protected mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<PrdAnbarRelation, PrdAnbarRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdAnbarRelationBusiness, PrdAnbarRelation> MapToDatabase => Mapper;

	public PrdAnbarRelationPersistenceRepositoryBase(ContextHolder context)
		: base(context)
	{
	}

	public abstract Task<ReturnedSaveFuncInfo> UpdateAccountAsync(ProductsChanges productsChanges, bool rollback, short tryCount, CancellationToken token, string tranName);

	public override Task<ReturnedSaveFuncInfo> RemoveAllAsync(short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("Should Not RemoveAll Data!!!");
	}

	public abstract Task<ReturnedSaveFuncInfo> RemoveAnbarRelationsAsync(IEnumerable<Guid> prdGuids, short tryCount, CancellationToken token, string tranName = null);

	public async Task<List<PrdAnbarRelationBusiness>> GetAnbarRelationsAsync(Guid? prdGuid, Guid? dptGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 33));
		using (new JaegerMonitor("GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs"))
		{
			List<PrdAnbarRelationBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<PrdAnbarRelation> queryable = c.PrdAnbarRelation.AsQueryable();
					if (prdGuid.HasValue)
					{
						queryable = queryable.Where((PrdAnbarRelation x) => x.PrdGuid == prdGuid);
					}
					if (dptGuid.HasValue)
					{
						queryable = queryable.Where((PrdAnbarRelation x) => x.AnbarGuid == dptGuid);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<PrdAnbarRelation, PrdAnbarRelationBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = base.ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarRelation>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 47);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdAnbarRelationBusiness> items, bool updateAmount, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs"))
		{
			try
			{
				foreach (PrdAnbarRelationBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveAsync(item, updateAmount, tryCount, token, tranName);
					if (returnedSaveFuncInfo.HasError)
					{
						return returnedSaveFuncInfo;
					}
				}
				return new ReturnedSaveFuncInfo();
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 74);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 75);
			}
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdAnbarRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Use Method With UpdateAmount Parameter");
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(PrdAnbarRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Use Method With UpdateAmount Parameter");
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(PrdAnbarRelationBusiness item, bool updateAmount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbContext1 = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 88));
				PrdAnbarRelation newObject = await MapToDatabase.MapAsync(dbContext.Context.Guid, item, null, tranName, token);
				PrdAnbarRelation prdAnbarRelation = await ((DbContext)dbContext1).FindAsync<PrdAnbarRelation>(new object[2] { item.PrdGuid, item.AnbarGuid });
				if (prdAnbarRelation == null)
				{
					prdAnbarRelation = newObject;
					if (!updateAmount)
					{
						prdAnbarRelation.Amount = 0m;
					}
					((DbContext)dbContext1).Add<PrdAnbarRelation>(newObject);
				}
				else
				{
					if (!updateAmount)
					{
						newObject.Amount = prdAnbarRelation.Amount;
					}
					((EntityEntry)((DbContext)dbContext1).Entry<PrdAnbarRelation>(prdAnbarRelation)).CurrentValues.SetValues((object)newObject);
					((DbContext)dbContext1).Update<PrdAnbarRelation>(prdAnbarRelation);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 106);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 107);
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
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 120);
		}
	}

	public abstract Task<IEnumerable<ProductOrderPointModel>> GetAllProductOrderpointsAsync(CancellationToken token, string tranName);

	public virtual Task<decimal> GetNegativeAmountAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<decimal> GetPositiveAmountAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<PrdAnbarRelationBusiness>> GetAllAsync(List<ProductsChanges> list, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 127));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs"))
		{
			IEnumerable<PrdAnbarRelationBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (!(list?.Any() ?? false))
					{
						result = null;
					}
					else
					{
						List<Guid> prdGuidList = list.Select((ProductsChanges q) => q.PrdGuid).ToList();
						List<Guid> depotGuidList = list.Select((ProductsChanges q) => q.depGuid).ToList();
						IQueryable<PrdAnbarRelation> prdAnbarRelation = (IQueryable<PrdAnbarRelation>)c.PrdAnbarRelation;
						prdAnbarRelation = prdAnbarRelation.Where((PrdAnbarRelation x) => prdGuidList.Contains(x.PrdGuid) && depotGuidList.Contains(x.AnbarGuid));
						if (prdAnbarRelation == null || !prdAnbarRelation.Any())
						{
							result = null;
						}
						else
						{
							IItemAsyncMapper<PrdAnbarRelation, PrdAnbarRelationBusiness> mapFromDatabase = MapFromDatabase;
							string contextGuid = base.ContextGuid;
							result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarRelation>(prdAnbarRelation, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs", 144);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}
}
