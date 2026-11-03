using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global.Tasks;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tasks;

public class TaskCashierRelationPersistenceRepository : PersistenceNoGuidRepository<TaskCashierRelationBusiness, TaskCashierRelation>, ITaskCashierRelationRepository, INoGuidRepository<TaskCashierRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TaskCashierRelation, TaskCashierRelationBusiness>, IItemAsyncMapper<TaskCashierRelationBusiness, TaskCashierRelation>
	{
		public Task<TaskCashierRelationBusiness> MapAsync(string contextGuid, TaskCashierRelation source, TaskCashierRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs"))
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
						destination = new TaskCashierRelationBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.CashierGuid = source.CashierGuid;
					destination.TaskGuid = source.TaskGuid;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 100);
					throw;
				}
			}
		}

		public Task<TaskCashierRelation> MapAsync(string contextGuid, TaskCashierRelationBusiness source, TaskCashierRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs"))
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
						destination = new TaskCashierRelation();
					}
					destination.CashierGuid = source.CashierGuid;
					destination.TaskGuid = source.TaskGuid;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 124);
					throw;
				}
			}
		}

		public Task<List<TaskCashierRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<TaskCashierRelation> source, List<TaskCashierRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaskCashierRelation>> MapListAsync(string contextGuid, IEnumerable<TaskCashierRelationBusiness> source, List<TaskCashierRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<TaskCashierRelation, TaskCashierRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TaskCashierRelationBusiness, TaskCashierRelation> MapToDatabase => Mapper;

	public TaskCashierRelationPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 37);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TaskCashierRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (item == null)
			{
				return ret;
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 47));
				TaskCashierRelation taskCashierRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(taskCashierRelation, taskCashierRelation.TaskGuid, taskCashierRelation.CashierGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 49);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 53);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 54);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TaskCashierRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items == null)
			{
				return ret;
			}
			try
			{
				if (items != null && items.Any())
				{
					foreach (TaskCashierRelationBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 67);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 71);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskCashierRelationPersistenceRepository.cs", 72);
			}
			return ret;
		}
	}
}
