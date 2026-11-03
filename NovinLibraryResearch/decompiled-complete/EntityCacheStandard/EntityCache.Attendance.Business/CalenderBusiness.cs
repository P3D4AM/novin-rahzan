using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.Entities.MetaData.Calendar;
using PacketParser.Records.Calendar;

namespace EntityCache.Attendance.Business;

public class CalenderBusiness : Serializable<CalenderBusiness>, ICalender, IHasJustGuid, IHasStatus, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Monasebat { get; set; }

	public string Day => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateM);

	public string Description { get; set; }

	public bool STRasmi { get; set; }

	public DateTime Modified { get; set; }

	public string ContextGuid { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public CalendarSmsMetaData CalendarSmsMetaData { get; set; }

	public byte[] IsSendSms
	{
		get
		{
			if (!(CalendarSmsMetaData?.CalendarSmsList?.Any()).GetValueOrDefault())
			{
				return ImageResourceManager.IsNotSend;
			}
			return ImageResourceManager.IsSend;
		}
	}

	public CalenderBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<CalenderBusiness> GetAsync(string contextGuid, Guid guid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Calender.GetAsync(guid, dateM, token, tranName);
	}

	public static Task<IEnumerable<CalenderBusiness>> GetAllAsync(string contextGuid, DateTime? d1, DateTime? d2, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Calender.GetAllAsync(d1, d2, status, token, tranName);
	}

	internal static Task<CalenderBusiness> GetAsync(string contextGuid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Calender.GetAsync(dateM, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<CalenderBusiness> lstBussiness, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lstBussiness?.Any() ?? false))
			{
				return ret;
			}
			DateTime value = lstBussiness.Min((CalenderBusiness q) => q.DateM);
			DateTime value2 = lstBussiness.Max((CalenderBusiness q) => q.DateM);
			IEnumerable<CalenderBusiness> dbList = await GetAllAsync(contextGuid, value, value2, true, token, tranName);
			foreach (CalenderBusiness item in lstBussiness)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 57);
				}
				IEnumerable<CalenderBusiness> enumerable = dbList?.Where((CalenderBusiness q) => q.DateM.Date == item.DateM);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (enumerable != null && enumerable.Any())
				{
					CalendarSmsMetaData firstDbItemMetaData = enumerable.First().CalendarSmsMetaData;
					foreach (CalenderBusiness item2 in enumerable)
					{
						item2.Status = false;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 65);
					}
					item.CalendarSmsMetaData = firstDbItemMetaData;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 69);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 74);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 75);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "CalenderBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 89), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 89);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Calender.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 91);
						goto end_IL_0116;
					}
					result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 88);
					goto end_IL_007d;
					end_IL_0116:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 93);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 96);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 97);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 102), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 102);
			}
		}
		return res;
	}

	public static async Task<List<CalenderSmsRecord>> SendSmsAsync(string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			if ((await LocalSettingBusiness.GetAsync(contextGuid, "CalndarSms_" + DateTime.Now.MiladiToCalture()))?.ParseToBoolean() ?? false)
			{
				return null;
			}
			List<CalenderSmsRecord> result = new List<CalenderSmsRecord>();
			ClsCache cache = ClsCache.GetCache(contextGuid);
			if (cache == null || cache.Source != ENSource.Accounting)
			{
				return null;
			}
			CalenderBusiness calendar = await GetAsync(contextGuid, DateTime.Now, token);
			if (!(calendar?.CalendarSmsMetaData?.CalendarSmsList?.Any()).GetValueOrDefault())
			{
				return null;
			}
			List<TafsilNumberReportBusiness> list = await TafsilNumberBusiness.GetAllReportAsync(contextGuid, token);
			if (list == null || !list.Any())
			{
				return null;
			}
			foreach (CalendarSms item in calendar.CalendarSmsMetaData.CalendarSmsList)
			{
				DateTime dateTime = new DateTime(calendar.DateM.Year, calendar.DateM.Month, calendar.DateM.Day, item.TimeFrom.Hour, item.TimeFrom.Minute, 0);
				DateTime dateTime2 = new DateTime(calendar.DateM.Year, calendar.DateM.Month, calendar.DateM.Day, item.TimeTo.Hour, item.TimeTo.Minute, 0);
				if (DateTime.Now < dateTime || DateTime.Now > dateTime2)
				{
					continue;
				}
				new List<long>();
				List<long> list2 = ((!(item.CustomerGuidList?.Any() ?? false)) ? list.Select((TafsilNumberReportBusiness q) => q.OriginalNumber)?.ToList() : list.Where((TafsilNumberReportBusiness q) => item.CustomerGuidList.Contains(q.TafsilGuid))?.Select((TafsilNumberReportBusiness q) => q.OriginalNumber)?.ToList());
				if (list2 == null || !list2.Any())
				{
					continue;
				}
				foreach (long item2 in list2)
				{
					result.Add(new CalenderSmsRecord
					{
						Reciever = item2,
						SmsText = item.SmsPattern,
						RefrenceGuid = calendar.Guid
					});
				}
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\CalenderBusiness.cs", 147);
			throw;
		}
	}
}
