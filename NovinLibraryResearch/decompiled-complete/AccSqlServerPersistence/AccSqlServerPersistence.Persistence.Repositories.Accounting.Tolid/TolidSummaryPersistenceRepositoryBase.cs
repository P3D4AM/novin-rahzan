using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Tolid.Summary;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal abstract class TolidSummaryPersistenceRepositoryBase : PersistenceRepository<TolidSummaryBusiness, TolidSummary>, ITolidSummaryRepository, IRepository<TolidSummaryBusiness>, INoGuidRepository<TolidSummaryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidSummaryBusiness, TolidSummary>, IItemAsyncMapper<TolidSummary, TolidSummaryBusiness>
	{
		public Task<TolidSummary> MapAsync(string contextGuid, TolidSummaryBusiness source, TolidSummary destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidSummary();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description ?? "";
				destination.Id2 = source.Id2;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 210);
				throw;
			}
		}

		public Task<TolidSummaryBusiness> MapAsync(string contextGuid, TolidSummary source, TolidSummaryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidSummaryBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.Id2 = source.Id2;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 237);
				throw;
			}
		}

		public Task<List<TolidSummary>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryBusiness> source, List<TolidSummary> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidSummaryBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidSummary> source, List<TolidSummaryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private TolidSummaryDetailPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<TolidSummary, TolidSummaryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<TolidSummary, TolidSummaryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidSummaryBusiness, TolidSummary> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public TolidSummaryPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
		_detRepo = new TolidSummaryDetailPersistenceRepository(dbContext);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_detRepo?.Dispose();
				_detRepo = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 44);
		}
	}

	public async Task<IEnumerable<TolidSummaryReportBusiness>> GetAllAsync(TolidSummaryFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 48));
		IEnumerable<TolidSummaryReportBusiness> result;
		try
		{
			IQueryable<TolidSummary> tolidSummaries = (IQueryable<TolidSummary>)acc_ModelContext.TolidSummaries;
			if (tolidSummaries == null || !tolidSummaries.Any())
			{
				result = null;
			}
			else
			{
				IQueryable<TolidSummaryReportBusiness> queryable = tolidSummaries.Select((TolidSummary q) => new TolidSummaryReportBusiness
				{
					CashierGuid = q.CashierGuid,
					CashierName = q.Cashier.Name,
					DateM = q.DateM,
					Description = q.Description,
					Guid = q.Guid,
					Id2 = q.Id2
				});
				if (filter != null)
				{
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((TolidSummaryReportBusiness q) => q.DateM >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((TolidSummaryReportBusiness q) => q.DateM <= filter.DateTo);
					}
					if (filter.Id2From != 0)
					{
						queryable = queryable?.Where((TolidSummaryReportBusiness q) => q.Id2 >= filter.Id2From);
					}
					if (filter.Id2To != 0)
					{
						queryable = queryable?.Where((TolidSummaryReportBusiness q) => q.Id2 <= filter.Id2To);
					}
					if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
					{
						queryable = queryable?.Where((TolidSummaryReportBusiness q) => q.CashierGuid == filter.CashierGuid);
					}
				}
				result = queryable?.ToList();
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 77);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public override async Task<TolidSummaryBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 88));
		TolidSummaryBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				TolidSummary tolidSummary = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TolidSummary>((IQueryable<TolidSummary>)c.TolidSummaries, (Expression<Func<TolidSummary, bool>>)((TolidSummary q) => q.Guid == guid), token);
				if (tolidSummary == null)
				{
					result = null;
				}
				else
				{
					TolidSummaryBusiness mappedItem = await MapFromDatabase.MapAsync(ContextGuid, tolidSummary, new TolidSummaryBusiness(ContextGuid), tranName, token);
					List<TolidSummaryDetailBusiness> list = await _detRepo.GetAllAsync(guid, token, tranName);
					if (list != null && list.Any())
					{
						mappedItem.AddRangeToDetList(list, clearList: true);
					}
					result = mappedItem;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 103);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 115));
			int result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TolidSummary> queryable = ((IQueryable<TolidSummary>)c.TolidSummaries).OrderByDescending((TolidSummary x) => x.Id2).Take(1);
					TolidSummary tolidSummary = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TolidSummary>(queryable, token) : null);
					result = ((tolidSummary == null) ? 1 : (tolidSummary.Id2 + 1));
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 127);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<List<TolidSummaryDetailBusiness>> GetAllAsync(TolidSummaryBusiness master, DateTime dateM, CancellationToken token, string tranName);

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TolidSummaryBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 145));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				TolidSummary tolidSummary = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(tolidSummary, tolidSummary.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 148);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 150);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.ListTolidDet?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 151);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 156);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 157);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 169));
				TolidSummary tolidSummary = ((IQueryable<TolidSummary>)acc_ModelContext.TolidSummaries).Single((TolidSummary p) => p.Guid == guid);
				if (tolidSummary == null)
				{
					return ret;
				}
				acc_ModelContext.TolidSummaries.Remove(tolidSummary);
				IQueryable<TolidSummaryDetail> queryable = ((IQueryable<TolidSummaryDetail>)((DbContext)acc_ModelContext).Set<TolidSummaryDetail>()).Where((TolidSummaryDetail x) => x.MasterGuid == guid);
				((DbContext)acc_ModelContext).Set<TolidSummaryDetail>().RemoveRange((IEnumerable<TolidSummaryDetail>)queryable);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 179);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepositoryBase.cs", 180);
			}
			return ret;
		}
	}
}
