using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class MoeinTafsilGroupRelationsPersistanceRepository : PersistenceNoGuidRepository<MoeinTafsilGroupRelationsBusiness, MoeinTafsilGroupRelations>, IMoeinTafsilGroupRelationsRepository, INoGuidRepository<MoeinTafsilGroupRelationsBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<MoeinTafsilGroupRelationsBusiness, MoeinTafsilGroupRelations>, IItemAsyncMapper<MoeinTafsilGroupRelations, MoeinTafsilGroupRelationsBusiness>
	{
		public Task<MoeinTafsilGroupRelations> MapAsync(string contextGuid, MoeinTafsilGroupRelationsBusiness source, MoeinTafsilGroupRelations destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
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
						destination = new MoeinTafsilGroupRelations();
					}
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGroupGuid = source.TafsilGroupGuid;
					destination.Level = source.Level;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 170);
					throw;
				}
			}
		}

		public Task<MoeinTafsilGroupRelationsBusiness> MapAsync(string contextGuid, MoeinTafsilGroupRelations source, MoeinTafsilGroupRelationsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
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
						destination = new MoeinTafsilGroupRelationsBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGroupGuid = source.TafsilGroupGuid;
					destination.Level = source.Level;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 195);
					throw;
				}
			}
		}

		public Task<List<MoeinTafsilGroupRelations>> MapListAsync(string contextGuid, IEnumerable<MoeinTafsilGroupRelationsBusiness> source, List<MoeinTafsilGroupRelations> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<MoeinTafsilGroupRelationsBusiness>> MapListAsync(string contextGuid, IEnumerable<MoeinTafsilGroupRelations> source, List<MoeinTafsilGroupRelationsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<MoeinTafsilGroupRelations, MoeinTafsilGroupRelationsBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<MoeinTafsilGroupRelationsBusiness, MoeinTafsilGroupRelations> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public MoeinTafsilGroupRelationsPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<MoeinTafsilGroupRelationsBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilLevelGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 31));
			IEnumerable<MoeinTafsilGroupRelationsBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<MoeinTafsilGroupRelations> source = (IQueryable<MoeinTafsilGroupRelations>)c.MoeinTafsilLevelRelation;
					if (tafsilLevelGuid.HasValue)
					{
						source = source.Where((MoeinTafsilGroupRelations p) => p.TafsilGroupGuid == tafsilLevelGuid);
					}
					if (moeinGuid.HasValue)
					{
						source = source.Where((MoeinTafsilGroupRelations p) => p.MoeinGuid == moeinGuid);
					}
					result = await MapFromDatabase.MapListAsync(base.ContextGuid, source.ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 45);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid? moeinGuid, Guid? tafsilGroupGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!moeinGuid.HasValue && !tafsilGroupGuid.HasValue)
				{
					return res.AddError("parameter is null", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 61);
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 62));
				IQueryable<MoeinTafsilGroupRelations> source = dbcontext.MoeinTafsilLevelRelation.AsQueryable();
				source = ((!moeinGuid.HasValue) ? source.Where((MoeinTafsilGroupRelations p) => p.TafsilGroupGuid == tafsilGroupGuid) : source.Where((MoeinTafsilGroupRelations p) => p.MoeinGuid == moeinGuid));
				if (source == null || !source.Any())
				{
					return res;
				}
				List<MoeinTafsilGroupRelations> list = await EntityFrameworkQueryableExtensions.ToListAsync<MoeinTafsilGroupRelations>(source, token);
				if (list.Count > 0)
				{
					((DbContext)dbcontext).RemoveRange((IEnumerable<object>)list);
				}
				if (res.HasError || (moeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					return res;
				}
				Moein moein = await dbcontext.Moein.FindAsync(new object[1] { moeinGuid.Value });
				if (moein != null && ClsCache.GetCache(base.ContextGuid).Source == ENSource.Accounting)
				{
					moein.ServerStatus = ServerStatusForClient.None;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(moein, moein.Guid), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 77);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 83);
				res.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 84);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(MoeinTafsilGroupRelationsBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 95));
				MoeinTafsilGroupRelations moeinTafsilGroupRelations = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(moeinTafsilGroupRelations, moeinTafsilGroupRelations.MoeinGuid, moeinTafsilGroupRelations.TafsilGroupGuid, moeinTafsilGroupRelations.Level), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 97);
				if (ret.HasError)
				{
					return ret;
				}
				if (ClsCache.GetCache(base.ContextGuid).Source == ENSource.Accounting)
				{
					Moein moein = await c.Moein.FindAsync(new object[1] { item.MoeinGuid });
					if (moein != null)
					{
						moein.ServerStatus = ServerStatusForClient.None;
						((DbContext)c).Update<Moein>(moein);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 112);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 113);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinTafsilGroupRelationsBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs"))
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
					foreach (MoeinTafsilGroupRelationsBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 126);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 130);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 131);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinTafsilGroupRelationsPersistanceRepository.cs", 145);
		}
	}
}
