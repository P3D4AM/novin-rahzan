using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm.Report;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.Filters.Crm;

namespace EntityCache.Business.Crm.Calling;

public class CallingLogDetailBusiness : ICallingLogDetail, IHasContext
{
	private static SemaphoreSlim _databaseSemaphor = new SemaphoreSlim(1, 1);

	private string callFrom_Number;

	private string callTo_Number;

	private EnumNotification status;

	private Guid? _ticketGuid;

	public string ContextGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public string PickUpDateSh => PickUpDateM.MiladiToCalture();

	public string PickUpTime => PickUpDateM?.ToLongTimeString();

	public DateTime? HangUpDateM { get; set; }

	public DateTime? PickUpDateM { get; set; }

	public DateTime DateInsert { get; set; }

	public DateTime Modified { get; set; }

	public string HangUpDateSh => HangUpDateM.MiladiToCalture();

	public string HangUpDateTime => HangUpDateM?.ToLongTimeString();

	public string ModifiedSh => Modified.ToLongTimeString();

	public EnumNotification Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
		}
	}

	public string StatusForDisplay => Status.GetDisplay().GetTranslate();

	public string Uuid { get; set; }

	public string LinkedId { get; set; }

	public string CallFrom_Number
	{
		get
		{
			return callFrom_Number;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && value.Length > 15)
			{
				value = new WebTafsilNumbers(value).CalcedNumber.ToString();
			}
			if (!string.IsNullOrEmpty(value) && value.Length > 15)
			{
				throw new ArgumentOutOfRangeException("value(" + value + ") is not valid for CallFrom_Number max len is 15 characters");
			}
			callFrom_Number = value;
		}
	}

	public string CallFrom_Name { get; set; }

	public string CallTo_Number
	{
		get
		{
			return callTo_Number;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && value.Length > 15)
			{
				value = new WebTafsilNumbers(value).CalcedNumber.ToString();
			}
			if (!string.IsNullOrEmpty(value) && value.Length > 15)
			{
				throw new ArgumentOutOfRangeException("value(" + value + ") is not valid for callTo_Number max len is 15 characters");
			}
			callTo_Number = value;
		}
	}

	public TimeSpan? Duration
	{
		get
		{
			if (PickUpDateM.HasValue && HangUpDateM.HasValue)
			{
				return HangUpDateM.Value.Subtract(PickUpDateM.Value);
			}
			return null;
		}
	}

	public string CallTo_Name { get; set; }

	public string Description { get; set; }

	public short Rings { get; set; }

	public int Version { get; set; }

	public bool IsChecked { get; set; }

	public CallingLogDetailBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.CallingLogDetailBusiness;

	public event Func<object, EventArgs, Task> OnSave;

	public CallingLogDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transationHasBeenCreate = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphor.GaurdAsync(true, "CallingLogDetailBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 92);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 93), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 93);
					}
					transationHasBeenCreate = true;
					if (!res.HasError)
					{
						Modified = DateTime.Now;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICallingLogDetail.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 97);
						if (EventManager != null)
						{
							_ = MasterGuid;
							PacketParser.Utilities.RaiseEvent(EventManager.OnSave, this, new EventArgs(), default(CancellationToken), 60, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs");
						}
						goto end_IL_0116;
					}
					result = res;
					goto end_IL_007d;
					end_IL_0116:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 101);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 104);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 105);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autotran && transationHasBeenCreate)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 110), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 110);
			}
		}
		return res;
	}

	public static Task<CallingLogDetailBusiness> GetAsync(string contextGuid, string uuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLogDetail.GetAsync(uuid, token, tranName);
	}

	public static Task<IEnumerable<CallingLogDetailBusiness>> GetAllAsync(string contextGuid, CallingLogDetailFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLogDetail.GetAllAsync(filter, token, tranName);
	}

	public static async Task<CallingLogDetailCountReport> GetCountCallCashierAsync(string contextGuid, Guid? cashierGuid, List<string> numbers, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<CallingLogDetailBusiness> enumerable = await GetAllAsync(contextGuid, new CallingLogDetailFilter(), token, tranName);
			int num = 0;
			if (numbers?.Any() ?? false)
			{
				num = (enumerable?.Where((CallingLogDetailBusiness q) => numbers.Contains(q.CallFrom_Number) || numbers.Contains(q.CallTo_Number))?.Count()).GetValueOrDefault();
			}
			if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
			{
				enumerable = enumerable?.Where((CallingLogDetailBusiness q) => q.CashierGuid == cashierGuid.Value).ToList();
			}
			int valueOrDefault = (enumerable?.Where((CallingLogDetailBusiness q) => q.DateInsert >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) && q.DateInsert <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))?.Count()).GetValueOrDefault();
			int valueOrDefault2 = (enumerable?.Where((CallingLogDetailBusiness q) => q.DateInsert >= Calendar.OneWeekAgo() && q.DateInsert <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))?.Count()).GetValueOrDefault();
			int valueOrDefault3 = (enumerable?.Where((CallingLogDetailBusiness q) => q.DateInsert >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 23, 59, 59) && q.DateInsert <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))?.Count()).GetValueOrDefault();
			return new CallingLogDetailCountReport
			{
				TodayCount = valueOrDefault,
				WeekCount = valueOrDefault2,
				MonthCount = valueOrDefault3,
				ListCount = enumerable.Count(),
				CallNumberCountCall = num
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCountCallCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogDetailBusiness.cs", 142);
			throw;
		}
	}
}
