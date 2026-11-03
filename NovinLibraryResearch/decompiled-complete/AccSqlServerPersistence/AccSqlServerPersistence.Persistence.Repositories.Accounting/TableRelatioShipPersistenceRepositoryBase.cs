using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class TableRelatioShipPersistenceRepositoryBase : PersistenceRepository<TableRelationShipBusiness, TableRelationShip>, ITableRelationShipRepository, IRepository<TableRelationShipBusiness>, INoGuidRepository<TableRelationShipBusiness>, IDisposable
{
	private class mapper : IItemMapper<TableRelationShip, TableRelationShipBusiness>, IItemAsyncMapper<TableRelationShip, TableRelationShipBusiness>, IItemAsyncMapper<TableRelationShipBusiness, TableRelationShip>
	{
		public TableRelationShipBusiness Map(string contextGuid, TableRelationShip source, TableRelationShipBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new TableRelationShipBusiness(contextGuid);
					}
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Guid = source.Guid;
					destination.Price = source.Price;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 193);
					throw;
				}
				return destination;
			}
		}

		public Task<TableRelationShipBusiness> MapAsync(string contextGuid, TableRelationShip source, TableRelationShipBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<List<TableRelationShipBusiness>> MapListAsync(string contextGuid, IEnumerable<TableRelationShip> source, List<TableRelationShipBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<TableRelationShip> MapAsync(string contextGuid, TableRelationShipBusiness source, TableRelationShip destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
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
						destination = new TableRelationShip();
					}
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Guid = source.Guid;
					destination.Price = source.Price;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 222);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<TableRelationShip>> MapListAsync(string contextGuid, IEnumerable<TableRelationShipBusiness> source, List<TableRelationShip> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class TableRelationShipBusinessWithEmptyConstructor : TableRelationShipBusiness
	{
		public TableRelationShipBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<TableRelationShip, TableRelationShipBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override IItemAsyncMapper<TableRelationShip, TableRelationShipBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TableRelationShipBusiness, TableRelationShip> MapToDatabase => Mapper;

	public TableRelatioShipPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<TableRelationShipBusiness>> GetAllRelatedSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllRelatedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllRelatedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 34));
			IEnumerable<TableRelationShipBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TableRelationShip> queryable = ((IQueryable<TableRelationShip>)c.TableRelationShip).Where((TableRelationShip x) => x.SanadGuid == sanadGuid && x.SanadType == sanadType);
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<TableRelationShipBusinessWithEmptyConstructor> queryable2 = queryable.Select((TableRelationShip q) => new TableRelationShipBusinessWithEmptyConstructor
						{
							Guid = q.Guid,
							IsInRelationSanad = false,
							Price = q.Price,
							SanadGuid = q.RelatedSanadGuid,
							SanadType = q.RelatedSanadType,
							ContextGuid = ContextGuid
						});
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TableRelationShipBusinessWithEmptyConstructor>(queryable2, token) : null);
					}
					result = await (Task<List<TableRelationShipBusinessWithEmptyConstructor>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllRelatedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 52);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllRelatedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<TableRelationShipBusiness>> GetAllInRelationSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllInRelationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllInRelationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 64));
			IEnumerable<TableRelationShipBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TableRelationShip> queryable = ((IQueryable<TableRelationShip>)c.TableRelationShip).Where((TableRelationShip x) => x.RelatedSanadGuid == sanadGuid && x.RelatedSanadType == sanadType);
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<TableRelationShipBusinessWithEmptyConstructor> queryable2 = queryable.Select((TableRelationShip q) => new TableRelationShipBusinessWithEmptyConstructor
						{
							Guid = q.Guid,
							IsInRelationSanad = true,
							Price = q.Price,
							SanadGuid = q.SanadGuid,
							SanadType = q.SanadType,
							ContextGuid = ContextGuid
						});
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TableRelationShipBusinessWithEmptyConstructor>(queryable2, token) : null);
					}
					result = await (Task<List<TableRelationShipBusinessWithEmptyConstructor>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllInRelationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 82);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllInRelationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (sanadGuid == Guid.Empty)
				{
					return ret;
				}
				BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 99);
				IQueryable<TableRelationShip> queryable = ((IQueryable<TableRelationShip>)((DbContext)baseModelContext).Set<TableRelationShip>()).Where((TableRelationShip x) => x.SanadGuid == sanadGuid && x.SanadType == sanadType);
				((DbContext)baseModelContext).Set<TableRelationShip>().RemoveRange((IEnumerable<TableRelationShip>)queryable);
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 106);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 107);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TableRelationShipBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 118));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await RemoveRangeAsync(item.SanadGuid, item.SanadType, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 120);
				foreach (TableRelationShipBusiness child in item.ChildList ?? new List<TableRelationShipBusiness>())
				{
					if (res.HasError)
					{
						return res;
					}
					TableRelationShip tableRelationShip = await MapToDatabase.MapAsync(ContextGuid, child, null, tranName, token);
					tableRelationShip.RelatedSanadGuid = child.SanadGuid;
					tableRelationShip.RelatedSanadType = child.SanadType;
					tableRelationShip.SanadGuid = item.SanadGuid;
					tableRelationShip.SanadType = item.SanadType;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(tableRelationShip, tableRelationShip.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 129);
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 135);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 136);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TableRelationShipBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (items != null && items.Any())
				{
					foreach (TableRelationShipBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 150);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 154);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 155);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TableRelatioShipPersistenceRepositoryBase.cs", 168);
		}
	}
}
