using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Crm.Calling;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Crm;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Crm;

internal class CallingLogDetailPersistenceRepository : PersistenceNoGuidRepository<CallingLogDetailBusiness, CallingLogDetail>, ICallingLogDetailRepository, INoGuidRepository<CallingLogDetailBusiness>, IDisposable
{
	public class mapper : IItemAsyncMapper<CallingLogDetailBusiness, CallingLogDetail>, IItemAsyncMapper<CallingLogDetail, CallingLogDetailBusiness>
	{
		public Task<CallingLogDetail> MapAsync(string contextGuid, CallingLogDetailBusiness source, CallingLogDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
				}
				if (destination == null)
				{
					destination = new CallingLogDetail();
				}
				if (source.CashierGuid == Guid.Empty)
				{
					destination.CashierGuid = null;
				}
				else
				{
					destination.CashierGuid = source.CashierGuid;
				}
				destination.DateInsert = source.DateInsert;
				destination.PickUpDateM = source.PickUpDateM;
				destination.HangUpDateM = source.HangUpDateM;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.CallFrom_Number = source.CallFrom_Number;
				destination.CallTo_Number = source.CallTo_Number;
				destination.CallFrom_Name = source.CallFrom_Name;
				destination.CallTo_Name = source.CallTo_Name;
				destination.Description = source.Description;
				destination.LinkedId = source.LinkedId;
				destination.Uuid = source.Uuid;
				destination.Rings = source.Rings;
				destination.Version = source.Version;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 170);
				throw;
			}
		}

		public Task<CallingLogDetailBusiness> MapAsync(string contextGuid, CallingLogDetail source, CallingLogDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CallingLogDetailBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.DateInsert = source.DateInsert;
				destination.PickUpDateM = source.PickUpDateM;
				destination.HangUpDateM = source.HangUpDateM;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.CallFrom_Number = source.CallFrom_Number;
				destination.CallTo_Number = source.CallTo_Number;
				destination.CallFrom_Name = source.CallFrom_Name;
				destination.CallTo_Name = source.CallTo_Name;
				destination.Description = source.Description;
				destination.LinkedId = source.LinkedId;
				destination.Uuid = source.Uuid;
				destination.Rings = source.Rings;
				destination.Version = source.Version;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 203);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<CallingLogDetail>> MapListAsync(string contextGuid, IEnumerable<CallingLogDetailBusiness> source, List<CallingLogDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CallingLogDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLogDetail> source, List<CallingLogDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CallingLogDetail, CallingLogDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CallingLogDetailBusiness, CallingLogDetail> MapToDatabase => Mapper;

	public CallingLogDetailPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 40);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CallingLogDetailBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(item.Uuid))
			{
				throw new ArgumentNullException("Uuid");
			}
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 49));
				CallingLog master = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CallingLog>((IQueryable<CallingLog>)dbcontext.CallingLog, (Expression<Func<CallingLog, bool>>)((CallingLog q) => q.Uuid == item.LinkedId), token);
				if (master == null)
				{
					return ret;
				}
				CallingLogDetail callingLogDetail = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				if (!master.PickUpDateM.HasValue)
				{
					callingLogDetail.PickUpDateM = master.PickUpDateM;
				}
				callingLogDetail.Version++;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(callingLogDetail, callingLogDetail.Uuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 58);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 60);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 63);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 64);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CallingLogDetailBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<CallingLogDetailBusiness> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 75));
			CallingLogDetailBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CallingLogDetail source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CallingLogDetail>((IQueryable<CallingLogDetail>)c.CallingLogDetail, (Expression<Func<CallingLogDetail, bool>>)((CallingLogDetail q) => q.Uuid == uuid), default(CancellationToken));
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<CallingLogDetailBusiness>> GetAllAsync(CallingLogDetailFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 96));
		Stopwatch sw = Stopwatch.StartNew();
		int recordCount = 0;
		IEnumerable<CallingLogDetailBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<CallingLogDetail> queryable = (IQueryable<CallingLogDetail>)c.CallingLogDetail;
				if (filter != null)
				{
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable.Where((CallingLogDetail q) => q.DateInsert >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable.Where((CallingLogDetail q) => q.DateInsert <= filter.DateTo);
					}
					if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable.Where((CallingLogDetail q) => q.CashierGuid == filter.CashierGuid.Value);
					}
					if (filter.LinkedId != null)
					{
						queryable = queryable.Where((CallingLogDetail q) => q.LinkedId == filter.LinkedId);
					}
					if (filter.Callfrom != null)
					{
						queryable = queryable.Where((CallingLogDetail q) => q.CallFrom_Number == filter.Callfrom || q.CallTo_Number == filter.Callfrom);
					}
					if (filter.NumberList?.Any() ?? false)
					{
						queryable = queryable?.Where((CallingLogDetail q) => filter.NumberList.Contains(q.CallFrom_Number) || filter.NumberList.Contains(q.CallTo_Number));
					}
				}
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					recordCount = queryable.Count();
					result = await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs", 120);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogDetailPersistenceRepository.cs");
			}
			if (sw.ElapsedMilliseconds > 20000)
			{
				throw new Exception($"Totlal elapsed time for getAll calling log is:{sw.ElapsedMilliseconds}(ms)\r\n" + $"RecordCount:{recordCount}\r\n" + "filter:" + Json.ToStringJson(filter) + " ");
			}
		}
		return result;
	}
}
