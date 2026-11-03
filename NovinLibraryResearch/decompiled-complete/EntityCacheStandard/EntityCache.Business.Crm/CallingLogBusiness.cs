using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Business.Crm.Report;
using EntityCache.Core;
using EntityCache.EventArguments;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Crm.Calling;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Crm;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.EventArgs;
using PacketParser.Filters.Crm;
using PacketParser.TransferLayer;

namespace EntityCache.Business.Crm;

public class CallingLogBusiness : BaseBusiness<CallingLogBusiness>, ICallingLog, IHasModified, IHasStatus, IHasContext, ISyncFromClient2Server<CallingLogBusiness>, IDeliverableEntity, IEditableDelivery, IHasJustGuid
{
	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private static SemaphoreSlim _databaseSemaphoreTotalConcurrent = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private short rings;

	private DateTime? deliveryDate;

	private Guid? _ticketGuid;

	private string callerIdNumber;

	private string dialedNumber;

	private DateTime dateM = DateTime.Now;

	private int version;

	private DateTime? _pickUpDateM;

	private DateTime? _hangUpDateM;

	private bool isIncoming;

	[Obsolete("Use UUID", true)]
	public Guid Guid { get; set; }

	public string? MasterUuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	bool IHasStatus.Status => Status == EnumNotification.Deleted;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public string CallerIdNumber
	{
		get
		{
			return callerIdNumber;
		}
		set
		{
			try
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 15)
				{
					value = new WebTafsilNumbers(value).CalcedNumber.ToString();
				}
				if (!string.IsNullOrEmpty(value) && value.Length > 15)
				{
					throw new ArgumentOutOfRangeException("value(" + value + ") is not valid for callerIdNumber max len is 15 characters");
				}
				if (value.ParseToLong(0L) <= 0)
				{
					return;
				}
				if (string.IsNullOrEmpty(callerIdNumber) || callerIdNumber.Length < value.Length)
				{
					callerIdNumber = value;
					return;
				}
				if (!string.IsNullOrEmpty(callerIdNumber) && value.Length >= 3)
				{
					callerIdNumber = value;
					return;
				}
				if (callerIdNumber.ParseToLong(0L) != value.ParseToLong(0L) && callerIdNumber.ParseToLong(0L) > 0)
				{
					throw new Exception("Number changed from(" + callerIdNumber + ") to (" + value + ")");
				}
				if (DialedNumber.ParseToLong(0L) > 0 && value.ParseToLong(0L) > 0)
				{
					throw new Exception("outgoing call in changing to incomming! dialedNumber is (" + DialedNumber + ") to (" + value + ")");
				}
				callerIdNumber = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CallerIdNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 86);
				throw;
			}
		}
	}

	public short Rings
	{
		get
		{
			return rings;
		}
		set
		{
			rings = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string NumberForDisplay
	{
		get
		{
			if (!string.IsNullOrEmpty(DialedNumber))
			{
				return DialedNumber;
			}
			return CallerIdNumber ?? "";
		}
	}

	public string DialedNumber
	{
		get
		{
			return dialedNumber;
		}
		set
		{
			try
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 15)
				{
					value = new WebTafsilNumbers(value).CalcedNumber.ToString();
				}
				if (!string.IsNullOrEmpty(value) && value.Length > 15)
				{
					throw new ArgumentOutOfRangeException("value(" + value + ") is not valid for callerIdNumber max len is 15 characters");
				}
				if (value.ParseToLong(0L) <= 0)
				{
					return;
				}
				if (string.IsNullOrEmpty(dialedNumber) || value.Length > dialedNumber.Length)
				{
					dialedNumber = value;
					return;
				}
				if (!string.IsNullOrEmpty(dialedNumber) && value.Length >= 3)
				{
					dialedNumber = value;
					return;
				}
				if (dialedNumber.ParseToLong(0L) != value.ParseToLong(0L) && dialedNumber.ParseToLong(0L) > 0)
				{
					throw new Exception("dialedNumber changed from(" + dialedNumber + ") to (" + value + ")");
				}
				if (callerIdNumber.ParseToLong(0L) > 0 && value.ParseToLong(0L) > 0)
				{
					throw new Exception("Incomming call in changing to Outgoing! CallerIdNumber is (" + callerIdNumber + ") to (" + value + ")");
				}
				dialedNumber = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DialedNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 133);
			}
		}
	}

	public DateTime? PickUpDateM
	{
		get
		{
			if (!_pickUpDateM.HasValue)
			{
				return null;
			}
			checked
			{
				if (_pickUpDateM.Value.Year > DateTime.Now.Year + 10)
				{
					return null;
				}
				if (_pickUpDateM.Value.Year < DateTime.Now.Year - 10)
				{
					return null;
				}
				return _pickUpDateM;
			}
		}
		set
		{
			_pickUpDateM = value;
		}
	}

	public DateTime? HangUpDateM
	{
		get
		{
			if (!_hangUpDateM.HasValue)
			{
				return null;
			}
			checked
			{
				if (_hangUpDateM.Value.Year > DateTime.Now.Year + 10)
				{
					return null;
				}
				if (_hangUpDateM.Value.Year < DateTime.Now.Year - 10)
				{
					return null;
				}
				return _hangUpDateM;
			}
		}
		set
		{
			_hangUpDateM = value;
		}
	}

	public DateTime DateM
	{
		get
		{
			if (dateM.Year >= 2000 && dateM.Year <= DateTime.Now.AddYears(10).Year)
			{
				return dateM;
			}
			return DateTime.Now;
		}
		set
		{
			dateM = value;
		}
	}

	public string Time => DateM.ToShortTimeString();

	public double Duration => (HangUpDateM ?? DateTime.Now).Subtract(PickUpDateM ?? DateTime.Now).TotalSeconds;

	public Guid? CoGuid { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }

	public Guid? ExGuid { get; set; }

	public EnumNotification Status { get; set; }

	public bool IsIncoming
	{
		get
		{
			return isIncoming;
		}
		set
		{
			if (isIncoming && !value)
			{
				throw new InvalidOperationException("چرا داری تماس ورودی رو نوع اش رو به خروجی تغییر میدی؟");
			}
			isIncoming = value;
		}
	}

	public bool IsCompleteCall { get; set; }

	public string Description { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string PickUpDateSh => PickUpDateM.MiladiToCalture();

	public string PickUpTime => PickUpDateM?.ToLongTimeString();

	public string PickUpDateByTimeSh => PickUpTime + " " + PickUpDateSh;

	public string HangUpDateSh => HangUpDateM.MiladiToCalture();

	public string HangUpTime => HangUpDateM?.ToLongTimeString();

	public string HangUpDateByTimeSh => HangUpTime + " " + HangUpDateSh;

	public int Version
	{
		get
		{
			return version;
		}
		set
		{
			if (version > value)
			{
				throw new Exception($"old version({version})=>new version({value})");
			}
			if (value > version)
			{
				_sendToken?.Cancel();
			}
			version = value;
		}
	}

	public long CalcedNumber
	{
		get
		{
			if (!IsIncoming)
			{
				return DialedNumber.ParseToLong(0L);
			}
			return CallerIdNumber.ParseToLong(0L);
		}
	}

	public string str_Duration
	{
		get
		{
			if (HangUpDateM.HasValue)
			{
				if (!(HangUpDateM == PickUpDateM))
				{
					return PacketParser.Time.IntToTime(Duration, "H:mm:ss");
				}
				return "00:00:00";
			}
			return "";
		}
	}

	public string StatusName => Status.GetDisplay().GetTranslate();

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate
	{
		get
		{
			if ((deliveryDate?.Date ?? DateTime.Now).Year >= 2000)
			{
				return deliveryDate;
			}
			return null;
		}
		set
		{
			deliveryDate = value;
		}
	}

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string Uuid { get; set; }

	public DateTime? DateUtc { get; set; }

	public CallingLogBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.CallingLogBusiness;

	public event Func<object, CallingLogEventArgs, Task> OnSavePersistence;

	public CallingLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<CallingLineBusiness> GetCoAsync(string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return CallingLineBusiness.GetAsync(ContextGuid, CoGuid ?? Guid.Empty, token, tranName);
	}

	public Task<CallingLineBusiness> GetExtentionAsync(CancellationToken token = default(CancellationToken))
	{
		return CallingLineBusiness.GetAsync(ContextGuid, ExGuid ?? Guid.Empty, token);
	}

	public async Task<IEnumerable<CusCompactBusiness>> GetTafsilNumberAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<TafsilNumberBusiness> list;
			if (isIncoming)
			{
				string contextGuid = ContextGuid;
				long number = CallerIdNumber.ParseToLong(0L);
				CancellationToken token2 = token;
				list = await TafsilNumberBusiness.GetInfoAsync(contextGuid, number, true, "", token2);
			}
			else
			{
				string contextGuid2 = ContextGuid;
				long number2 = dialedNumber.ParseToLong(0L);
				CancellationToken token2 = token;
				list = await TafsilNumberBusiness.GetInfoAsync(contextGuid2, number2, true, "", token2);
			}
			List<CusCompactBusiness> customers = new List<CusCompactBusiness>();
			if (list != null && list.Any())
			{
				foreach (TafsilNumberBusiness item in list)
				{
					List<CusCompactBusiness> list2 = customers;
					list2.Add(await CusCompactBusiness.GetAsync(ContextGuid, system, item.TafsilGuid, null, tranName, token));
				}
			}
			return customers;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 252);
			throw;
		}
	}

	public async Task<string> GetTafsilNameAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			IEnumerable<CusCompactBusiness> enumerable = await GetTafsilNumberAsync(system, tranName, token);
			string result;
			if (enumerable != null && enumerable.Any())
			{
				result = enumerable?.FirstOrDefault()?.FullName;
			}
			else
			{
				Guid guid = default(Guid);
				if (IsIncoming)
				{
					if (DialedNumber.ParseToInt() != 0)
					{
						guid = await CashierBusiness.GetCashierGuidByIssabelExtentionNumberAsync(ContextGuid, DialedNumber.ParseToInt(), null, token);
					}
					else if (CallerIdNumber.ParseToInt() != 0)
					{
						guid = await CashierBusiness.GetCashierGuidByIssabelExtentionNumberAsync(ContextGuid, CallerIdNumber.ParseToInt(), null, token);
					}
				}
				result = (await CashierBusiness.GetAsync(ContextGuid, guid, token, tranName))?.Name;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 279);
			throw;
		}
	}

	public static async Task<CallingLogBusiness> GetFromDatabaseAsync(string contextGuid, string uuid, CancellationToken token, string tranName)
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetFromDatabaseAsync(uuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, bool isFromSync, bool awaitOnSemaphore = true, Priority priority = Priority.Low, bool await4Send = false, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				try
				{
					using (await _databaseSemaphoreTotalConcurrent.GaurdAsync(awaitOnSemaphore, "CallingLogBusiness/SaveAsync", 60000, token))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 295), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 295);
						}
						transactionHasBeenCreated = true;
						if (string.IsNullOrEmpty(Uuid))
						{
							Uuid = Guid.NewGuid().ToString();
						}
						if (string.IsNullOrEmpty(DialedNumber) && DialedNumber == CallerIdNumber)
						{
							throw new ArgumentException();
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICallingLog.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 300);
					}
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 302);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 305);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 306);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 311), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 311);
				}
				if (!ret.HasError && !ret.HasWarning)
				{
					Task.Run(() => ClsCache.GetCache(ContextGuid).BusinessAggregator.CallingLogBusiness.OnSavePersistence?.Invoke(this, new CallingLogEventArgs
					{
						Call = this,
						System = system,
						IsFromSync = isFromSync
					}));
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCallingLog>>> GenrateAndSaveChildAsync(string contextGuid, long number, WebCallingLog call, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<List<WebCallingLog>> res = new ReturnedSaveFuncInfoWithValue<List<WebCallingLog>>
		{
			value = new List<WebCallingLog>()
		};
		using (new JaegerMonitor("GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 4;
				ReturnedSaveFuncInfoWithValue<List<WebCallingLog>> result;
				try
				{
					using (SemaphoreUtil sem = await _databaseSemaphoreTotalConcurrent.GaurdAsync(true, "CallingLogBusiness/GenrateAndSaveChildAsync", 60000, token))
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfoWithValue<List<WebCallingLog>> returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 327), ReturnedState.None, "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 327);
						}
						transactionHasBeenCreated = true;
						if (sem.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 330);
						}
						else
						{
							List<Guid> list = await GetTafsilGuidsAsync(contextGuid, number, sem.Token, tranName);
							if (list != null && list.Any() && call.TafsilNumber != null)
							{
								call.TafsilNumber.TafsilGuid = list.FirstOrDefault();
							}
							res.value.Add(call);
							if (!sem.Token.IsCancellationRequested)
							{
								List<Guid> list2 = list?.Where((Guid q) => q != (call?.TafsilNumber?.TafsilGuid ?? Guid.Empty))?.ToList();
								if (list2 != null && list2.Any())
								{
									foreach (Guid item in list2)
									{
										if (!(item == Guid.Empty))
										{
											CallingLogBusiness callChild = new CallingLogBusiness(contextGuid)
											{
												CallerIdNumber = call.CallerIdNumber,
												DialedNumber = call.DialedNumber,
												DateM = call.DateM,
												IsIncoming = call.IsIncoming,
												Uuid = Guid.NewGuid().ToString(),
												Message = call.Message,
												StatusId = call.StatusId,
												MasterUuid = call.Uuid
											};
											ReturnedSaveFuncInfoWithValue<List<WebCallingLog>> returnedSaveFuncInfoWithValue = res;
											returnedSaveFuncInfoWithValue.AddReturnedValue(await callChild.SaveAsync(EnSystem.UserInterface, sendToServer: false, isFromSync: false, awaitOnSemaphore: false, Priority.Low, await4Send: false, 1, sem.Token, tranName), ReturnedState.None, "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 354);
											WebCallingLog webCallingLog = await CallingLogMapper.Instance.MapAsync(contextGuid, callChild, new WebCallingLog(), tranName, sem.Token);
											if (call.TafsilNumber != null)
											{
												webCallingLog.TafsilNumber.TafsilGuid = item;
											}
											res.value.Add(webCallingLog);
										}
									}
								}
								goto end_IL_015f;
							}
							result = res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 336);
						}
						goto end_IL_00bc;
						end_IL_015f:;
					}
					goto end_IL_00ab;
					end_IL_00bc:;
				}
				catch (OperationCanceledException exception)
				{
					res.AddReturnedValue(exception, "", "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 360);
					goto end_IL_00ab;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 363);
					res.AddReturnedValue(ex, "", "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 364);
					goto end_IL_00ab;
				}
				return result;
				end_IL_00ab:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfoWithValue<List<WebCallingLog>> returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 369), ReturnedState.None, "GenrateAndSaveChildAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 369);
				}
			}
			return res;
		}
	}

	public static async Task<CallingLogBusiness> GetLast(string contextGuid, string computerName, short deviceNumber, short lineNumber, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			CallingLineBusiness callingLineBusiness = await CallingLineBusiness.GetAsync(contextGuid, computerName, deviceNumber, lineNumber, token);
			CallingLogBusiness callingLogBusiness = ((callingLineBusiness == null) ? null : (await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetLast(callingLineBusiness.Guid, token, tranName)));
			if (callingLogBusiness != null && !callingLogBusiness.IsCompleteCall)
			{
				return callingLogBusiness;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 388);
			throw;
		}
	}

	public static Task<IEnumerable<CallingLogBusiness>> GetAllAsync(string contextGuid, long number, string tafsilName, CancellationToken token, DateTime startDate, DateTime endDate, short maxCount, EnumNotification? type, string tranName = "", Guid? tafsilGuid = null)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetAllAsync(number, tafsilName, token, startDate, endDate, maxCount, type, tranName, tafsilGuid);
	}

	public static Task<CallingLogBusiness> GetAsync(string contextGuid, string uuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetAsync(uuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, List<Guid> eConnectionData, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.DeleteRangeAsync(eConnectionData, token, "");
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, string contextGuid, List<WebCallingLog> lst, bool isFromSync, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (lst != null && lst.Any())
				{
					List<WebCallingLine> lines = lst.Select((WebCallingLog x) => x.Co)?.Where((WebCallingLine p) => p != null && p.Guid != Guid.Empty).ToList();
					lines.AddRange(lst.Select((WebCallingLog p) => p.Extension)?.Where((WebCallingLine p) => p != null && p.Guid != Guid.Empty));
					IEnumerable<Guid> enumerable = (from p in lines
						where p != null
						select p.Guid).Distinct();
					foreach (Guid lineGuid in enumerable)
					{
						CallingLineBusiness existsLine2 = await CallingLineBusiness.GetAsync(contextGuid, lineGuid, token);
						if (existsLine2 == null)
						{
							WebCallingLine webCallingLine = lines.FirstOrDefault((WebCallingLine p) => p.Guid == lineGuid);
							existsLine2 = await CallingLineBusiness.GetAsync(contextGuid, webCallingLine.ComputerName, webCallingLine.DeviceNumber, webCallingLine.LineNumber, token, tranName);
							if (existsLine2 != null)
							{
								WebCallingLine webCallingLine2 = await CallingLineMapper.Instance.MapAsync(contextGuid, existsLine2, new WebCallingLine(), "", token);
								foreach (WebCallingLog item in lst)
								{
									if (item.CoGuid.HasValue && item.CoGuid == lineGuid)
									{
										item.Co = webCallingLine2;
										item.CoGuid = webCallingLine2.Guid;
									}
									else if (item.ExGuid.HasValue && item.ExGuid == lineGuid)
									{
										item.Extension = webCallingLine2;
										item.ExGuid = webCallingLine2.Guid;
									}
								}
							}
						}
						if (existsLine2 == null || existsLine2.Guid != lineGuid)
						{
							existsLine2 = await CallingLineMapper.Instance.MapAsync(contextGuid, lines.FirstOrDefault((WebCallingLine p) => p.Guid == lineGuid), null, "", token);
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await existsLine2.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 454);
						}
					}
					foreach (WebCallingLog item2 in lst)
					{
						bool send2Server = system != EnSystem.SignalR;
						await (await CallingLogMapper.Instance.MapAsync(contextGuid, item2, null, "", token)).SaveAsync(system, send2Server, isFromSync, awaitOnSemaphore: true, Priority.Low, await4Send: false, 0, token, tranName);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 469);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 470);
			}
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, List<CallingLogBusiness> lst, Priority priority, bool isFromSync, bool sendToServer = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		checked
		{
			using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
			{
				ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
				try
				{
					var enumerable = lst?.GroupBy((CallingLogBusiness p) => p.Uuid)?.Select((IGrouping<string, CallingLogBusiness> group) => new
					{
						Guid = group.Key,
						count = group.Count()
					})?.Where(p => p.count > 1);
					if (enumerable != null && enumerable.Any())
					{
						foreach (var item in enumerable)
						{
							for (int i = 0; i < item.count - 1; i++)
							{
								lst.Remove(lst.FirstOrDefault((CallingLogBusiness p) => p.Uuid == item.Guid));
							}
						}
					}
					if (lst?.Any() ?? false)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(lst.Select((CallingLogBusiness p) => p.SaveAsync(system, sendToServer, isFromSync, awaitOnSemaphore: true, priority, await4Send: false, tryCount, token, tranName))), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 499);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 503);
					ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 504);
				}
				return ret;
			}
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (item == null)
		{
			throw new NullReferenceException("item");
		}
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		bool semaphoreHasBeenTaken = false;
		try
		{
			_ = 2;
			try
			{
				await _connectionPoolSemaphore.WaitAsync();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 520), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 520);
				}
				transactionHasBeenCreated = true;
				ret.throwExceptionIfError();
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICallingLog.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 523);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 525);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 528);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 529);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 534), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 534);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				return returnedSaveFuncInfo;
			}
			catch (OperationCanceledException exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 562);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 565);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 566);
			}
			return returnedSaveFuncInfo;
		}
	}

	public static async Task<List<CallingLogBusiness>> SearchAsync(string contextGuid, Expression<Func<ICallingLog, bool>> predicate, Expression<Func<ICallingLog, object>> orderby = null, int count = 0, bool descending = false, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		List<CallingLogBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.SearchAsync(predicate, orderby, count, descending, token, tranName);
		if (list == null)
		{
			list = new List<CallingLogBusiness>();
		}
		return list;
	}

	public Task<WebCallingLog> ToWebItemAsync(CancellationToken token)
	{
		return CallingLogMapper.Instance.MapAsync(ContextGuid, this, new WebCallingLog(), "", token);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.CallingLog;
			try
			{
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 644);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 645);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<List<string>>> SendRange(string contextGuid, IEnumerable<CallingLogBusiness> callingLogBusinesses, int timeout, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<string>> ret = new ReturnedSaveFuncInfoWithValue<List<string>>
		{
			value = new List<string>()
		};
		using (new JaegerMonitor("SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 672);
				}
				List<WebCallingLog> list = await CallingLogMapper.Instance.MapListAsync(contextGuid, callingLogBusinesses.ToList(), new List<WebCallingLog>(), "", token);
				List<Packet> paks = WebCallingLog.GenerateSaveRequest(list, 0);
				ret.value = list.Select((WebCallingLog s) => s.Uuid)?.ToList();
				using CallingLogWaiter waiter = new CallingLogWaiter(timeout, token);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 677);
				}
				if ((ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
				{
					return ret.AddError(EnSystem.SignalR, "Buffer is null or not connected!", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 679);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 681);
				}
				Task.Run(() => ClsCache.GetCache(contextGuid).SendAndWaitForResponse(paks, token, 20, 2, 10, 10));
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 683);
				}
				await waiter.WaitForSaveResponse(ret?.value, ClsCache.GetCache(contextGuid).Context);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 685);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The ClsCache.GetCache(contextGuid) :{ClsCache.GetCache(contextGuid) == null} ", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 689);
				ret.AddReturnedValue(ex, "", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 690);
			}
			return ret;
		}
	}

	internal static EnumNotification GetStatus(string issabelChanelState, EnumNotification currentState, DateTime? pickupDate, DateTime? hangUpDate)
	{
		try
		{
			switch (issabelChanelState.ToLower())
			{
			case "down":
				if (pickupDate.HasValue || currentState == EnumNotification.ConversationStarted)
				{
					return EnumNotification.ConversationFinished;
				}
				return EnumNotification.IncommingCall;
			case "up":
				if (hangUpDate.HasValue)
				{
					return EnumNotification.ConversationFinished;
				}
				return EnumNotification.ConversationStarted;
			case "ring":
				if (hangUpDate.HasValue && pickupDate.HasValue)
				{
					return currentState;
				}
				return EnumNotification.IncommingCall;
			case "ringing":
				if (hangUpDate.HasValue && pickupDate.HasValue)
				{
					return currentState;
				}
				return EnumNotification.IncommingCall;
			case "busy":
				return EnumNotification.Busy;
			default:
				if (currentState == EnumNotification.None)
				{
					return EnumNotification.IncommingCall;
				}
				return currentState;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 729);
			return EnumNotification.None;
		}
	}

	public static async Task<IEnumerable<CallingLogReportBusiness>> GetAllAsync(string ContextGuid, CallingLogFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			List<Guid> cashierList = filter.CashierList;
			if (cashierList != null && cashierList.Count > 0)
			{
				filter.Status.Add(EnumNotification.ConversationStarted);
			}
			if (filter.HasTicket.HasValue)
			{
				filter.StatusId = checked((short)(await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token)).TicketAccess.DefaultSanadStatusAccess.End);
			}
			return await ClsCache.GetCache(ContextGuid).CacheRepository.ICallingLog.GetAllAsync(filter, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 749);
			throw;
		}
	}

	public static async Task<CallingLogReportBusiness> GetLastAsync(string contextGuid, List<string> numbers, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<CallingLogReportBusiness> enumerable = await GetAllAsync(contextGuid, new CallingLogFilter
			{
				NumberString = numbers
			}, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return enumerable?.Where((CallingLogReportBusiness q) => q.Status != EnumNotification.MissedCall)?.OrderByDescending((CallingLogReportBusiness q) => q.DateM)?.FirstOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 764);
			throw;
		}
	}

	public static Task<CallingLogReportBusiness> GetReportAsync(string contextGuid, string uuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetReportAsync(uuid, token, tranName);
	}

	public static async Task<Guid> GetTafsilGuidAsync(string contextGuid, long number, CancellationToken token, string tranName = "")
	{
		_ = 2;
		try
		{
			using SemaphoreUtil dis = await _semaphore.GaurdAsync(true, "CallingLineBusiness/GetTafsilGuidAsync", 60000, token);
			if (number <= 0)
			{
				return Guid.Empty;
			}
			Guid tafsilGuid = Guid.Empty;
			if (dis.Token.IsCancellationRequested)
			{
				return tafsilGuid;
			}
			List<TafsilNumberBusiness> list = await TafsilNumberBusiness.GetInfoAsync(contextGuid, number, true, tranName, dis.Token);
			if (dis.Token.IsCancellationRequested)
			{
				return tafsilGuid;
			}
			TafsilNumberBusiness tafsilNumberBusiness = list?.FirstOrDefault();
			if (tafsilNumberBusiness != null)
			{
				tafsilGuid = tafsilNumberBusiness.TafsilGuid;
			}
			if (tafsilGuid == Guid.Empty && number <= 99999)
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetByInternalNumberAsync(contextGuid, checked((int)number), null, dis.Token, tranName);
				if (dis.Token.IsCancellationRequested)
				{
					return tafsilGuid;
				}
				if ((cashierBusiness?.VisitorGuid ?? Guid.Empty) != Guid.Empty)
				{
					tafsilGuid = cashierBusiness.VisitorGuid.Value;
				}
			}
			return tafsilGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 798);
			throw;
		}
	}

	public static async Task<List<Guid>> GetTafsilGuidsAsync(string contextGuid, long number, CancellationToken token, string tranName = "")
	{
		try
		{
			List<Guid> res = new List<Guid>();
			if (number <= 0)
			{
				return res;
			}
			List<TafsilNumberBusiness> list = await TafsilNumberBusiness.GetInfoAsync(contextGuid, number, null, tranName, token);
			if (token.IsCancellationRequested)
			{
				return res;
			}
			if (list != null && list.Any())
			{
				foreach (TafsilNumberBusiness item in list)
				{
					_ = item.TafsilGuid;
					res.Add(item.TafsilGuid);
				}
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilGuidsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 820);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateMessageStatusAsync(string contextGuid, string uuid, string message, short statusId, CancellationToken token, string tranName = "")
	{
		using (new JaegerMonitor("UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				try
				{
					using (await _databaseSemaphoreTotalConcurrent.GaurdAsync(true, "CallingLogBusiness/UpdateMessageStatusAsync", 60000))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 834), ReturnedState.None, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 834);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.UpdateMessageStatusAsync(uuid, message, statusId, token, tranName), ReturnedState.None, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 836);
					}
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 838);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 841);
					ret.AddReturnedValue(ex, "", "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 842);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 847), ReturnedState.None, "UpdateMessageStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 847);
				}
				CallingLogBusiness source = await GetAsync(contextGuid, uuid, token);
				WebCallingLog arg = await CallingLogMapper.Instance.MapAsync(contextGuid, source, new WebCallingLog());
				await (CallingLogReportBusiness.OnSetMessage?.Invoke(arg));
			}
			return ret;
		}
	}

	public static async Task<List<CallTitelSuggest>> GetCallTitelsSuggestAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			List<CallTitelSuggest> list = await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLog.GetCallTitelsSuggestAsync(token, tranName);
			if (list == null || list.Count == 0)
			{
				return new List<CallTitelSuggest>();
			}
			return (from t in (from x in list
					where !string.IsNullOrWhiteSpace(x.Titel)
					select x.Titel.Trim() into t
					where t.Length >= 3
					where Regex.IsMatch(t, "[a-zA-Zآ-ی]")
					where !Regex.IsMatch(t, "^[\\s\\.\\!\\?\\-]+$")
					select t).Distinct<string>(StringComparer.OrdinalIgnoreCase)
				orderby t
				select new CallTitelSuggest
				{
					Titel = t
				}).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCallTitelsSuggestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLogBusiness.cs", 878);
			return new List<CallTitelSuggest>();
		}
	}
}
