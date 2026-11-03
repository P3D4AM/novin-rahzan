using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

internal abstract class CheckedSavePersistenceRepositoryBase : PersistenceRepository<CheckedSaveBusiness, CheckedSave>, ICheckedSaveRepository, IRepository<CheckedSaveBusiness>, INoGuidRepository<CheckedSaveBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CheckedSaveBusiness, CheckedSave>, IItemAsyncMapper<CheckedSave, CheckedSaveBusiness>
	{
		public Task<CheckedSave> MapAsync(string contextGuid, CheckedSaveBusiness source, CheckedSave destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs"))
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
						destination = new CheckedSave();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.EntityGuid = source.EntityGuid;
					destination.IsDebit = source.IsDebit;
					destination.ReportType = source.ReportType;
					destination.DateM = source.DateM;
					destination.DetailGuid = source.DetailGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 137);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CheckedSaveBusiness> MapAsync(string contextGuid, CheckedSave source, CheckedSaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs"))
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
						destination = new CheckedSaveBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.EntityGuid = source.EntityGuid;
					destination.IsDebit = source.IsDebit;
					destination.ReportType = source.ReportType;
					destination.DateM = source.DateM;
					destination.DetailGuid = source.DetailGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 167);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<CheckedSave>> MapListAsync(string contextGuid, IEnumerable<CheckedSaveBusiness> source, List<CheckedSave> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CheckedSaveBusiness>> MapListAsync(string contextGuid, IEnumerable<CheckedSave> source, List<CheckedSaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CheckedSave, CheckedSaveBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CheckedSaveBusiness, CheckedSave> MapToDatabase => Mapper;

	public override IItemMapper<CheckedSave, CheckedSaveBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CheckedSavePersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<CheckedSaveBusiness>> GetAllAsync(checkedTablReportType reportType, Guid hesabGuid, bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 27));
			IEnumerable<CheckedSaveBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<CheckedSave> queryable = ((IQueryable<CheckedSave>)c.CheckedSaves).Where((CheckedSave q) => q.ReportType == reportType && q.EntityGuid == hesabGuid);
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid tafsilGuid, DateTime d1, DateTime d2, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 49));
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CheckedSave checkedSave = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CheckedSave>((IQueryable<CheckedSave>)c.CheckedSaves, (Expression<Func<CheckedSave, bool>>)((CheckedSave q) => q.ReportType == reportType && q.EntityGuid == tafsilGuid && q.DateM >= d1 && q.DateM <= d2), token);
			if (checkedSave != null)
			{
				c.CheckedSaves.Remove(checkedSave);
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
			WebErrorLog.Instance.StartLog(ex2, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 63);
			throw;
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 73));
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			try
			{
				List<CheckedSave> list = ((IQueryable<CheckedSave>)global_ModelContext.CheckedSaves).Where((CheckedSave q) => q.ReportType == reportType && q.EntityGuid == hesabGuid)?.ToList();
				if (list != null && list.Count >= 0)
				{
					global_ModelContext.CheckedSaves.RemoveRange((IEnumerable<CheckedSave>)list);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 82);
				throw;
			}
			return result;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, Guid detailGuid, short tryCount, CancellationToken token, string tranName);

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepositoryBase.cs", 101);
		}
	}
}
