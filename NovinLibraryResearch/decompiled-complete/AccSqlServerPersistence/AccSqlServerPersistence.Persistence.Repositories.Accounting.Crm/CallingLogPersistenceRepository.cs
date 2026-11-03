using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Report;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Crm;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.EventArgs;
using PacketParser.Filters.Crm;
using PacketParser.Issabel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Crm;

internal class CallingLogPersistenceRepository : PersistenceNoGuidRepository<CallingLogBusiness, CallingLog>, ICallingLogRepository, INoGuidRepository<CallingLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CallingLogBusiness, CallingLog>, IItemAsyncMapper<CallingLog, CallingLogBusiness>, IItemAsyncMapper<CallingLog, CallingLogReportBusiness>
	{
		public Task<CallingLog> MapAsync(string contextGuid, CallingLogBusiness source, CallingLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
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
						destination = new CallingLog();
					}
					destination.Modified = source.Modified;
					destination.CallerIdNumber = source.CallerIdNumber ?? "";
					destination.MasterUuid = source.MasterUuid;
					destination.Rings = source.Rings;
					destination.DialedNumber = source.DialedNumber ?? "";
					destination.PickUpDateM = source.PickUpDateM;
					destination.HangUpDateM = source.HangUpDateM;
					destination.DateM = source.DateM;
					destination.CoGuid = source.CoGuid ?? null;
					destination.ExGuid = source.ExGuid ?? null;
					destination.Status = source.Status;
					destination.IsIncoming = source.IsIncoming;
					destination.IsCompleteCall = source.IsCompleteCall;
					destination.Description = source.Description ?? "";
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage ?? "";
					destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
					destination.HostRefrenced = source.HostRefrenced ?? "";
					destination.Version = source.Version;
					destination.Message = source.Message;
					destination.StatusId = source.StatusId;
					if (string.IsNullOrEmpty(source.Uuid))
					{
						destination.Uuid = null;
					}
					else
					{
						destination.Uuid = source.Uuid;
					}
					destination.CalcedNumber = source.CalcedNumber;
					if (destination.CalcedNumber <= 0 && source.DialedNumber.ParseToLong(0L) > 0)
					{
						destination.CalcedNumber = source.DialedNumber.ParseToLong(0L);
					}
					if (destination.CalcedNumber <= 0 && source.CallerIdNumber.ParseToLong(0L) > 0)
					{
						destination.CalcedNumber = source.CallerIdNumber.ParseToLong(0L);
					}
					destination.DateUtc = destination.DateM.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 568);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CallingLogBusiness> MapAsync(string contextGuid, CallingLog source, CallingLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
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
						destination = new CallingLogBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Modified = source.Modified;
					destination.MasterUuid = source.MasterUuid;
					destination.CallerIdNumber = source.CallerIdNumber;
					destination.Rings = source.Rings;
					destination.DialedNumber = source.DialedNumber;
					destination.PickUpDateM = source.PickUpDateM;
					destination.HangUpDateM = source.HangUpDateM;
					destination.DateM = source.DateM;
					destination.CoGuid = (((source.CoGuid ?? Guid.Empty) == Guid.Empty) ? null : source.CoGuid);
					destination.ExGuid = (((source.ExGuid ?? Guid.Empty) == Guid.Empty) ? null : source.ExGuid);
					destination.Status = source.Status;
					destination.IsIncoming = source.IsIncoming;
					destination.IsCompleteCall = source.IsCompleteCall;
					destination.Description = source.Description;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.Version = source.Version;
					destination.Uuid = source.Uuid;
					destination.DateUtc = source.DateUtc;
					destination.Message = source.Message;
					destination.StatusId = source.StatusId;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 615);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CallingLogReportBusiness> MapAsync(string contextGuid, CallingLog source, CallingLogReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
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
						destination = new CallingLogReportBusiness();
					}
					destination.Status = source.Status;
					destination.IsIncoming = source.IsIncoming;
					destination.MasterUuid = source.MasterUuid;
					destination.IsCompleteCall = source.IsCompleteCall;
					destination.Uuid = source.Uuid;
					destination.CalcedNumber = source.CalcedNumber;
					destination.CallerIdNumber = source.CallerIdNumber;
					destination.DateM = source.DateM;
					destination.DialedNumber = source.DialedNumber;
					destination.PickUpDateM = source.PickUpDateM;
					destination.HangUpDateM = source.HangUpDateM;
					destination.HangUpTime = ((!source.HangUpDateM.HasValue) ? null : new TimeSpan?(source.HangUpDateM.Value.TimeOfDay));
					destination.PickUpTime = ((!source.PickUpDateM.HasValue) ? null : new TimeSpan?(source.PickUpDateM.Value.TimeOfDay));
					destination.Version = source.Version;
					destination.Rings = source.Rings;
					destination.Message = source.Message;
					destination.StatusId = source.StatusId;
					destination.CallFromName = "ناشناس".GetTranslate();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 655);
					throw;
				}
			}
		}

		public Task<List<CallingLog>> MapListAsync(string contextGuid, IEnumerable<CallingLogBusiness> source, List<CallingLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CallingLogBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLog> source, List<CallingLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CallingLogReportBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLog> source, List<CallingLogReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<CallingLog, CallingLogBusiness> MapFromDatabase => Mapper;

	public IItemAsyncMapper<CallingLog, CallingLogReportBusiness> MapFromDatabaseToReport => Mapper;

	public override IItemAsyncMapper<CallingLogBusiness, CallingLog> MapToDatabase => Mapper;

	internal CallingLogPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 50);
		}
	}

	public async Task<List<CallingLogBusiness>> SearchAsync<TKey>(Expression<Func<ICallingLog, bool>> predicate, Expression<Func<ICallingLog, TKey>> orderby, int takeCount, bool descending, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			List<CallingLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 58));
					IQueryable<ICallingLog> source = ((IQueryable<ICallingLog>)((DbContext)c).Set<CallingLog>()).Where(predicate);
					if (orderby != null)
					{
						source = (descending ? source.OrderByDescending(orderby) : source.OrderBy(orderby));
					}
					if (takeCount != 0)
					{
						source = source.Take(takeCount);
					}
					List<CallingLog> list = source.Cast<CallingLog>().ToList();
					List<CallingLogBusiness> list2 = ((list == null || list.Count > 0) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
					result = list2;
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
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 74);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CallingLogBusiness> GetLast(Guid coGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 86));
			CallingLogBusiness result;
			try
			{
				_ = 1;
				try
				{
					CallingLog source = (from q in (IQueryable<CallingLog>)c.CallingLog
						where q.CoGuid == coGuid || q.ExGuid == coGuid
						orderby q.DateM descending
						select q)?.FirstOrDefault();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 96);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<List<CallingLogBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName = "")
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			List<CallingLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 111));
					List<CallingLog> list = ((IQueryable<CallingLog>)((DbContext)c).Set<CallingLog>()).Where((CallingLog x) => x.ServerStatus != ServerStatusForClient.Delivered && x.ServerStatus != ServerStatusForClient.DirectDelivery && x.ServerStatus != ServerStatusForClient.Sent).Take(count)?.ToList();
					List<CallingLogBusiness> list2 = ((list == null || list.Count > 0) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
					result = list2;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 122);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<CallingLogBusiness>> GetLastRecordsByTime(int count, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetLastRecordsByTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			List<CallingLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastRecordsByTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 137));
					List<CallingLog> list = ((IQueryable<CallingLog>)((DbContext)c).Set<CallingLog>()).OrderByDescending((CallingLog x) => x.DateM).Take(count)?.ToList();
					List<CallingLogBusiness> list2 = ((list == null || list.Count > 0) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
					result = list2;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastRecordsByTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 145);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastRecordsByTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CallingLogBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 160));
				CallingLog dbObject = await c.CallingLog.FindAsync(new object[1] { item.Uuid });
				CallingLog callingLog = await Mapper.MapAsync(base.ContextGuid, item, null, tranName, token);
				callingLog.Version++;
				if (dbObject != null)
				{
					callingLog.Uuid = dbObject.Uuid;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				await returnedSaveFuncInfo.AddReturnedValueAsync(await c.AddOrUpdateAsync(callingLog, callingLog.Uuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 165);
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 167);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 170);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 171);
			}
			return res;
		}
	}

	public async Task<IEnumerable<CallingLogBusiness>> GetAllAsync(long number, string tafsilName, CancellationToken token, DateTime startDate, DateTime endDate, short maxCount, EnumNotification? type, string tranName, Guid? tafsilGuid)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			Acc_ModelContext c = null;
			IEnumerable<CallingLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<long> numbers = new List<long>();
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 182));
					IQueryable<CallingLog> source = ((IQueryable<CallingLog>)((DbContext)c).Set<CallingLog>()).Where((CallingLog w) => w.DateM >= startDate && w.DateM <= endDate);
					if (tafsilGuid.HasValue)
					{
						numbers.AddRange(from q in (IQueryable<TafsilNumber>)EntityFrameworkQueryableExtensions.Include<TafsilNumber, Tafsil>((IQueryable<TafsilNumber>)((DbContext)c).Set<TafsilNumber>(), (Expression<Func<TafsilNumber, Tafsil>>)((TafsilNumber d) => d.TafsilItem))
							where q.TafsilGuid == tafsilGuid
							select q into p
							select p.CalcedNumber);
					}
					if (!string.IsNullOrEmpty(tafsilName))
					{
						List<string> list = tafsilName.SplitString();
						if (list != null && list.Count > 0)
						{
							var source2 = from call in (IQueryable<CallingLog>)c.CallingLog
								join taf in (IEnumerable<TafsilNumber>)c.TafsilNumber on call.CalcedNumber equals taf.CalcedNumber
								select new
								{
									calling = call,
									tasils = taf
								};
							foreach (string item in list)
							{
								if (!string.IsNullOrEmpty(item) && item.Trim() != "")
								{
									source2 = ((number == 0L) ? source2.Where(x => x.tasils.TafsilItem.Name.Contains(item)) : source2.Where(x => x.calling.CalcedNumber == number || x.tasils.TafsilItem.Name.Contains(item)));
								}
							}
							source = source2.Select(s => s.calling);
						}
					}
					if (number > 0 && string.IsNullOrEmpty(tafsilName))
					{
						source = source.Where((CallingLog w) => w.CalcedNumber == number);
					}
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						if (type.HasValue)
						{
							source = source.Where((CallingLog w) => (int?)w.Status == (int?)type);
						}
						if (numbers?.Any() ?? false)
						{
							source = source.Where((CallingLog w) => numbers.Contains(w.CalcedNumber));
						}
						source = source.OrderByDescending((CallingLog o) => o.DateM).Take(maxCount);
						IEnumerable<CallingLog> enumerable = source.AsEnumerable();
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else
						{
							enumerable.Reverse();
							if (token.IsCancellationRequested)
							{
								result = null;
							}
							else
							{
								List<CallingLogBusiness> list2 = ((enumerable == null || enumerable.Count() > 0) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, enumerable, null, tranName, token)) : null);
								result = list2;
							}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 225);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<CallingLogBusiness> GetFromDatabaseAsync(string uuid, CancellationToken token, string tranName)
	{
		return GetAsync(uuid, token, tranName);
	}

	public async Task<CallingLogBusiness> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 238));
			CallingLogBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CallingLog source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CallingLog>((IQueryable<CallingLog>)c.CallingLog, (Expression<Func<CallingLog, bool>>)((CallingLog q) => q.Uuid == uuid), default(CancellationToken));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 248);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<CallingLogReportBusiness>> GetAllAsync(CallingLogFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 259));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			IEnumerable<CallingLogReportBusiness> result2;
			try
			{
				_ = 1;
				try
				{
					IQueryable<CallingLog> queryable = (IQueryable<CallingLog>)EntityFrameworkQueryableExtensions.Include<CallingLog, ICollection<CallingLogDetail>>((IQueryable<CallingLog>)c.CallingLog, (Expression<Func<CallingLog, ICollection<CallingLogDetail>>>)((CallingLog q) => q.CallingLogDetail));
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable.Where((CallingLog q) => q.DateM >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable.Where((CallingLog q) => q.DateM <= filter.DateTo);
					}
					if (filter.ExtentionNumberList != null && filter.ExtentionNumberList.Any())
					{
						List<string> extNumbersAsStrings = filter.ExtentionNumberList.Select((int x) => x.ToString()).ToList();
						queryable = queryable.Where((CallingLog q) => extNumbersAsStrings.Contains(q.CallerIdNumber) || extNumbersAsStrings.Contains(q.DialedNumber));
					}
					if (filter.NumberString?.Any() ?? false)
					{
						List<string> numbers = filter.NumberString;
						queryable = queryable?.Where((CallingLog q) => numbers.Contains(q.DialedNumber) || numbers.Contains(q.CallerIdNumber));
					}
					if (filter.HasTicket.HasValue)
					{
						queryable = ((!filter.HasTicket.GetValueOrDefault()) ? queryable.Where((CallingLog q) => q.StatusId != filter.StatusId) : queryable.Where((CallingLog q) => q.StatusId == filter.StatusId));
					}
					if (filter.IsNullMessage.HasValue)
					{
						queryable = ((!filter.IsNullMessage.GetValueOrDefault()) ? queryable.Where((CallingLog q) => q.Message != null) : queryable.Where((CallingLog q) => q.Message == null));
					}
					if (filter.IsNullMasterUuid.HasValue)
					{
						queryable = ((!filter.IsNullMasterUuid.GetValueOrDefault()) ? queryable.Where((CallingLog q) => q.MasterUuid != null) : queryable.Where((CallingLog q) => q.MasterUuid == null));
					}
					if (!string.IsNullOrEmpty(filter.Message))
					{
						queryable = queryable.Where((CallingLog q) => q.Message == filter.Message);
					}
					if (!string.IsNullOrEmpty(filter.MasterUuid))
					{
						queryable = queryable?.Where((CallingLog q) => q.MasterUuid == filter.MasterUuid);
					}
					if (filter.IsCompleteCall.HasValue)
					{
						queryable = queryable?.Where((CallingLog q) => (bool?)q.IsCompleteCall == filter.IsCompleteCall);
					}
					if (filter.IsIncoming.HasValue)
					{
						queryable = queryable?.Where((CallingLog q) => (bool?)q.IsIncoming == filter.IsIncoming);
					}
					if (filter.Status?.Any() ?? false)
					{
						queryable = queryable?.Where((CallingLog q) => q.CallingLogDetail.Any((CallingLogDetail q) => filter.Status.Contains(q.Status)));
					}
					List<CallingLogReportBusiness> result = queryable.Select((CallingLog q) => new CallingLogReportBusiness
					{
						Status = q.Status,
						IsIncoming = q.IsIncoming,
						IsCompleteCall = q.IsCompleteCall,
						Uuid = q.Uuid,
						CalcedNumber = q.CalcedNumber,
						CallerIdNumber = q.CallerIdNumber,
						DateM = q.DateM,
						DialedNumber = q.DialedNumber,
						HangUpTime = ((q.HangUpDateM == null) ? null : ((TimeSpan?)q.HangUpDateM.Value.TimeOfDay)),
						PickUpTime = ((q.PickUpDateM == null) ? null : ((TimeSpan?)q.PickUpDateM.Value.TimeOfDay)),
						CallToName = (q.CallingLogDetail.Where((CallingLogDetail q) => q.Status == EnumNotification.ConversationStarted).Any() ? (from q in q.CallingLogDetail
							where q.Status == EnumNotification.ConversationStarted
							orderby q.Modified
							select q).FirstOrDefault().CallTo_Name : ""),
						CallFromName = "ناشناس".GetTranslate()
					})?.ToList();
					if (filter?.DurationFrom.HasValue ?? false)
					{
						result = result?.Where((CallingLogReportBusiness q) => q.Duration >= filter.DurationFrom).ToList();
					}
					if (filter?.DurationTo.HasValue ?? false)
					{
						result = result?.Where((CallingLogReportBusiness q) => q.Duration <= filter.DurationTo).ToList();
					}
					if (filter?.WaitingTime.HasValue ?? false)
					{
						result = result?.Where((CallingLogReportBusiness q) => q.WaitingTime >= filter.WaitingTime).ToList();
					}
					if ((filter?.TakeCount).GetValueOrDefault() > 0)
					{
						result = result?.OrderByDescending((CallingLogReportBusiness q) => q.DateM).Take(filter.TakeCount.Value)?.ToList();
					}
					if (result?.Any() ?? false)
					{
						await UpdateNameAsync(c, result, token);
					}
					result2 = result?.OrderByDescending((CallingLogReportBusiness q) => q.DateM);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 332);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result2;
		}
	}

	private async Task UpdateNameAsync(Acc_ModelContext c, List<CallingLogReportBusiness> list, CancellationToken token)
	{
		_ = 2;
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			List<long> numberList = list.Select((CallingLogReportBusiness q) => new WebTafsilNumbers(q.DialedNumber)).Union(list.Select((CallingLogReportBusiness q) => new WebTafsilNumbers(q.CallerIdNumber)))?.Where((WebTafsilNumbers q) => q.CalcedNumber != 0)?.Select((WebTafsilNumbers q) => q.CalcedNumber)?.Distinct()?.ToList();
			List<TafsilNumber> tafsilNumbers = await EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumber>(((IQueryable<TafsilNumber>)EntityFrameworkQueryableExtensions.Include<TafsilNumber, Tafsil>((IQueryable<TafsilNumber>)c.TafsilNumber, (Expression<Func<TafsilNumber, Tafsil>>)((TafsilNumber q) => q.TafsilItem))).Where((TafsilNumber q) => numberList.Contains(q.CalcedNumber)).Distinct(), token);
			List<Cashier> source = await EntityFrameworkQueryableExtensions.ToListAsync<Cashier>(((IQueryable<Cashier>)c.Cashiers).Where((Cashier q) => q.Status), token);
			IEnumerable<Tuple<string, List<int>>> enumerable = ((await new CashierPersistanceRepository(dbContext).MapFromDatabase.MapListAsync(base.ContextGuid, source, null, "", token))?.Select((CashierBusiness q) => new Tuple<string, List<int>>(q.Name, q.Settings?.NumberList?.Select((IsabelleNumber q) => q.Number)?.ToList())))?.Where((Tuple<string, List<int>> q) => q.Item2?.Any() ?? false);
			IEnumerable<CallingLogReportBusiness> obj = list?.Where((CallingLogReportBusiness q) => q.IsIncoming);
			IEnumerable<CallingLogReportBusiness> enumerable2 = list?.Where((CallingLogReportBusiness q) => !q.IsIncoming);
			if (obj?.Any() ?? false)
			{
				if (enumerable != null && enumerable.Any())
				{
					foreach (Tuple<string, List<int>> item in enumerable)
					{
						foreach (int j in item.Item2)
						{
							IEnumerable<CallingLogReportBusiness> enumerable3 = list?.Where((CallingLogReportBusiness q) => new WebTafsilNumbers(q.DialedNumber).CalcedNumber == j);
							if (enumerable3 == null || !enumerable3.Any())
							{
								continue;
							}
							foreach (CallingLogReportBusiness item2 in enumerable3)
							{
								item2.CallToName = item.Item1;
							}
						}
					}
				}
				if (tafsilNumbers?.Any() ?? false)
				{
					foreach (TafsilNumber number2 in tafsilNumbers)
					{
						if (number2?.TafsilItem == null)
						{
							continue;
						}
						IEnumerable<CallingLogReportBusiness> enumerable4 = list?.Where((CallingLogReportBusiness q) => new WebTafsilNumbers(q.CallerIdNumber).CalcedNumber == number2.CalcedNumber);
						if (enumerable4 == null || !enumerable4.Any())
						{
							continue;
						}
						foreach (CallingLogReportBusiness item3 in enumerable4)
						{
							item3.CallFromName = number2.TafsilItem.Name;
						}
					}
				}
			}
			if (enumerable2 == null || !enumerable2.Any())
			{
				return;
			}
			if (enumerable != null && enumerable.Any())
			{
				foreach (Tuple<string, List<int>> item4 in enumerable)
				{
					foreach (int i in item4.Item2)
					{
						IEnumerable<CallingLogReportBusiness> enumerable5 = list?.Where((CallingLogReportBusiness q) => new WebTafsilNumbers(q.CallerIdNumber).CalcedNumber == i);
						IEnumerable<CallingLogReportBusiness> enumerable6 = list?.Where((CallingLogReportBusiness q) => new WebTafsilNumbers(q.DialedNumber).CalcedNumber == i);
						if (enumerable5 != null && enumerable5.Any())
						{
							foreach (CallingLogReportBusiness item5 in enumerable5)
							{
								item5.CallFromName = item4.Item1;
							}
						}
						if (enumerable6 == null || !enumerable6.Any())
						{
							continue;
						}
						foreach (CallingLogReportBusiness item6 in enumerable6)
						{
							item6.CallToName = item4.Item1;
						}
					}
				}
			}
			if (!(tafsilNumbers?.Any() ?? false))
			{
				return;
			}
			foreach (TafsilNumber number in tafsilNumbers)
			{
				if (number?.TafsilItem == null)
				{
					continue;
				}
				IEnumerable<CallingLogReportBusiness> enumerable7 = list?.Where((CallingLogReportBusiness q) => new WebTafsilNumbers(q.DialedNumber).CalcedNumber == number.CalcedNumber);
				if (enumerable7 == null || !enumerable7.Any())
				{
					continue;
				}
				foreach (CallingLogReportBusiness item7 in enumerable7)
				{
					item7.CallToName = number.TafsilItem.Name;
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
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 422);
			throw;
		}
	}

	public async Task<CallingLogReportBusiness> GetReportAsync(string uuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 429));
			CallingLogReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CallingLog callingLog = (await EntityFrameworkQueryableExtensions.ToListAsync<CallingLog>(RelationalQueryableExtensions.FromSqlRaw<CallingLog>(c.CallingLog, "select * from Acc_CallingLog with (updlock, holdlock) where Uuid='" + uuid + "'", Array.Empty<object>()), token)).SingleOrDefault();
					CallingLogReportBusiness callingLogReportBusiness = ((callingLog == null) ? null : (await MapFromDatabaseToReport.MapAsync(base.ContextGuid, callingLog, new CallingLogReportBusiness(), tranName, token)));
					result = callingLogReportBusiness;
				}
				catch (InvalidCastException)
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 442);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CallingLogBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	Task<ReturnedSaveFuncInfo> ICallingLogRepository.DeleteRangeAsync(List<Guid> eConnectionData, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	Task<ReturnedSaveFuncInfo> ICallingLogRepository.SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateMessageStatusAsync(string uuid, string message, short statusId, CancellationToken toke, string tranName)
	{
		using (new JaegerMonitor("UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (string.IsNullOrEmpty(uuid))
				{
					throw new ArgumentNullException("uuid");
				}
				if (string.IsNullOrEmpty(message) && statusId != 22)
				{
					throw new ArgumentNullException("message");
				}
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 471));
				CallingLog callingLog = await c.CallingLog.FindAsync(new object[1] { uuid });
				if (callingLog == null)
				{
					throw new ArgumentNullException("call");
				}
				callingLog.Message = callingLog.Message + "***" + message;
				callingLog.StatusId = statusId;
				callingLog.Modified = DateTime.Now;
				((DbContext)c).Update<CallingLog>(callingLog);
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 480);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 483);
				res.AddReturnedValue(ex2, "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 484);
			}
			return res;
		}
	}

	public async Task<List<CallTitelSuggest>> GetCallTitelsSuggestAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetCallTitelsSuggestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetCallTitelsSuggestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 492));
			List<CallTitelSuggest> result;
			try
			{
				List<string> list = ((IQueryable<CallingLog>)acc_ModelContext.CallingLog).Select((CallingLog q) => q.Message).Distinct().ToList();
				result = ((list != null && list.Any()) ? list.Select((string q) => new CallTitelSuggest
				{
					Titel = q
				})?.ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetCallTitelsSuggestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs", 506);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetCallTitelsSuggestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Crm\\CallingLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
