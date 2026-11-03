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
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tasks;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.tasks;

internal class TaskPersistenceRepository : PersistenceRepository<TaskBusiness, TaskClass>, ITaskRepository, IRepository<TaskBusiness>, INoGuidRepository<TaskBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TaskBusiness, TaskClass>, IItemAsyncMapper<TaskClass, TaskBusiness>
	{
		public Task<TaskClass> MapAsync(string contextGuid, TaskBusiness source, TaskClass destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs"))
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
						destination = new TaskClass();
					}
					destination.Guid = source.Guid;
					destination.Title = source.Title;
					destination.DateM = source.DateM;
					destination.CashierGuid = source.CashierGuid;
					destination.StartDate = source.StartDate;
					destination.EndDate = source.EndDate;
					destination.StatusId = source.StatusId;
					destination.Description = source.Description;
					destination.MasterGuid = source.MasterGuid;
					destination.Status = source.Status;
					destination.Modified = source.Modified;
					destination.Id2 = source.Id2;
					destination.MetaData = Json.ToStringJson(source.MetaData);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 131);
					throw;
				}
			}
		}

		public Task<TaskBusiness> MapAsync(string contextGuid, TaskClass source, TaskBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs"))
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
						destination = new TaskBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.CashierGuid = source.CashierGuid;
					destination.DateM = source.DateM;
					destination.StartDate = source.StartDate;
					destination.EndDate = source.EndDate;
					destination.Title = source.Title;
					destination.StatusId = source.StatusId;
					destination.Description = source.Description;
					destination.MasterGuid = source.MasterGuid;
					destination.Id2 = source.Id2;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					if (!string.IsNullOrEmpty(source.MetaData))
					{
						destination.MetaData = source.MetaData.FromJson<TaskMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 160);
					}
					destination.ChangeStatus = EntityChangeSt.Edited;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 168);
					throw;
				}
			}
		}

		public Task<List<TaskClass>> MapListAsync(string contextGuid, IEnumerable<TaskBusiness> source, List<TaskClass> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return MapListAsync(contextGuid, source, destination, tranName);
		}

		public Task<List<TaskBusiness>> MapListAsync(string contextGuid, IEnumerable<TaskClass> source, List<TaskBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return MapListAsync(contextGuid, source, destination, tranName);
		}
	}

	private TaskCashierRelationPersistenceRepository _cashierTask;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<TaskClass, TaskBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<TaskClass, TaskBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TaskBusiness, TaskClass> MapToDatabase => Mapper;

	public TaskPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_cashierTask = new TaskCashierRelationPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				_cashierTask?.Dispose();
				_cashierTask = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 50);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TaskBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext _dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 58));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				TaskClass taskClass = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _dbcontext.AddOrUpdateAsync(taskClass, taskClass.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 61);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _cashierTask.SaveRangeAsync(item.CashierList.ToList(), 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 63);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 67);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 68);
			}
			return ret;
		}
	}

	public async Task<int> GetNextId2Async(CancellationToken token, string tranName)
	{
		new JaegerMonitor("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs");
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 75));
		int result;
		try
		{
			int num = 0;
			IQueryable<TaskClass> queryable = ((IQueryable<TaskClass>)global_ModelContext.Tasks).Where((TaskClass q) => q.Id2 > 0);
			if (queryable != null && queryable.Any())
			{
				num = queryable.Max((TaskClass q) => q.Id2);
			}
			num++;
			result = num;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs", 89);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await global_ModelContext.DisposeAsync("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\Tasks\\TaskPersistenceRepository.cs");
			}
		}
		return result;
	}
}
