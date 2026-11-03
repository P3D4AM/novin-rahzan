using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Attendance.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Calendar;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CalenderPersistenceRepository : PersistenceRepository<CalenderBusiness, Calender>, ICalenderRepository, IRepository<CalenderBusiness>, INoGuidRepository<CalenderBusiness>, IDisposable
{
	public class mapper : IItemAsyncMapper<CalenderBusiness, Calender>, IItemAsyncMapper<Calender, CalenderBusiness>
	{
		public Task<Calender> MapAsync(string contextGuid, CalenderBusiness source, Calender destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs"))
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
						destination = new Calender();
					}
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Monasebat = source.Monasebat;
					destination.STRasmi = source.STRasmi;
					destination.Status = source.Status;
					if (source.CalendarSmsMetaData != null)
					{
						destination.MetaData = Json.ToStringJson(source.CalendarSmsMetaData);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 178);
					throw;
				}
			}
		}

		public Task<CalenderBusiness> MapAsync(string contextGuid, Calender source, CalenderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs"))
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
						destination = new CalenderBusiness(contextGuid);
					}
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Monasebat = source.Monasebat;
					destination.STRasmi = source.STRasmi;
					destination.Status = source.Status;
					if (!string.IsNullOrEmpty(source.MetaData))
					{
						destination.CalendarSmsMetaData = source.MetaData.FromJson<CalendarSmsMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 201);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 208);
					throw;
				}
			}
		}

		public Task<List<Calender>> MapListAsync(string contextGuid, IEnumerable<CalenderBusiness> source, List<Calender> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CalenderBusiness>> MapListAsync(string contextGuid, IEnumerable<Calender> source, List<CalenderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Calender, CalenderBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CalenderBusiness, Calender> MapToDatabase => Mapper;

	public override IItemMapper<Calender, CalenderBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public CalenderPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CalenderBusiness calender, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 40));
				Calender item = await MapToDatabase.MapAsync(ContextGuid, calender, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, calender.Guid, calender.DateM), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 42);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 46);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 47);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CalenderBusiness> lstTaghvim, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (!(lstTaghvim?.Any() ?? false))
				{
					return ret;
				}
				foreach (CalenderBusiness item in lstTaghvim)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 60);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 64);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 65);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<CalenderBusiness>> GetAllAsync(DateTime? d1, DateTime? d2, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 71));
		IEnumerable<CalenderBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IQueryable<Calender> queryable = (IQueryable<Calender>)c.Calender;
				if (d1.HasValue)
				{
					queryable = queryable?.Where((Calender q) => q.DateM >= ((DateTime?)d1).Value);
				}
				if (d2.HasValue)
				{
					queryable = queryable?.Where((Calender q) => q.DateM <= ((DateTime?)d2).Value);
				}
				if (status.HasValue)
				{
					queryable = queryable?.Where((Calender q) => q.Status == ((bool?)status).Value);
				}
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					IItemAsyncMapper<Calender, CalenderBusiness> mapFromDatabase = MapFromDatabase;
					string contextGuid = ContextGuid;
					result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Calender>(queryable, token) : null), null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 85);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs");
			}
		}
		return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 103);
		}
	}

	public async Task<CalenderBusiness> GetAsync(Guid guid, DateTime dateM, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 107));
		CalenderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Calender calender = await c.Calender.FindAsync(new object[2] { guid, dateM });
				result = ((calender != null) ? (await MapFromDatabase.MapAsync(ContextGuid, calender, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 118);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<CalenderBusiness> GetAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 129));
		CalenderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Calender calender = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Calender>((IQueryable<Calender>)c.Calender, (Expression<Func<Calender, bool>>)((Calender q) => q.DateM.Date == ((DateTime)dateM).Date && q.Status), token);
				result = ((calender != null) ? (await MapFromDatabase.MapAsync(ContextGuid, calender, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs", 140);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\CalenderPersistenceRepository.cs");
			}
		}
		return result;
	}
}
