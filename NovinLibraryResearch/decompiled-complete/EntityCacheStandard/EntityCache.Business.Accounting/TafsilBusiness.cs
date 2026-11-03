using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Events;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Records.TafsilRecords;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business.Accounting;

public class TafsilBusiness : BaseBusiness<TafsilBusiness>, ITafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<TafsilBusiness>, IDeliverableEntity, IEditableDelivery, INotifyPropertyChanged
{
	private DateTime dateM = DateTime.Now;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 5);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 5);

	private SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _name = "";

	private string _description = "";

	private long code;

	private Guid _guid;

	private decimal _account;

	[JsonIgnore]
	[XmlIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public decimal SumAghsat { get; set; }

	public bool IsSpecial { get; set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value?.Trim().FixString();
		}
	}

	public long Code
	{
		get
		{
			return code;
		}
		set
		{
			code = value;
		}
	}

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.Trim().FixString();
		}
	}

	public bool System { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public HesabType HType { get; set; }

	public decimal Account
	{
		get
		{
			return _account;
		}
		set
		{
			_account = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Account", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 80);
				}
			}, "Account", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs");
		}
	}

	public DateTime DateM
	{
		get
		{
			return dateM;
		}
		set
		{
			if (value < DateTime.Now.AddYears(100) && value > DateTime.Now.AddYears(-100))
			{
				dateM = value;
			}
			else
			{
				dateM = DateTime.Now;
			}
		}
	}

	public string SerializedData { get; set; }

	public Guid Guid
	{
		get
		{
			return _guid;
		}
		set
		{
			if (_guid == Guid.Empty || _guid == value)
			{
				_guid = value;
				return;
			}
			throw new InvalidOperationException($"change TafsilGuid from ({_guid}) to ({value}) is not valid!");
		}
	}

	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public int Version { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public Task<TafsilGroupBusiness> GetTafsilGroupAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilGroupBusiness.GetAsync(ContextGuid, TafsilGroupGuid, system, token, tranName);
	}

	public TafsilBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<TafsilBusiness>> GetAllAsync(string contextGuid, EnSystem system, TafsilFilter filter, CancellationToken token = default(CancellationToken), string tranName = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				semaphoreHasBeenTaken = await _databaseSemaphore.WaitAsync(60000);
				IEnumerable<TafsilBusiness> items = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAllAsync(system, filter, token, tranName);
				return await ApplyAccessAsync(contextGuid, sourceFilePath + "/" + memberName, items, system, token, tranName);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 133);
				throw;
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_databaseSemaphore.Release();
				}
			}
		}
	}

	public static async Task<TafsilBusiness> GetFromDatabaseAsync(string contextGuid, EnSystem system, Guid tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		_ = 1;
		try
		{
			if (tafsilGuid == Guid.Empty)
			{
				return null;
			}
			string calleName = "GetFromDatabaseAsync=>" + sourceFilePath + "/" + memberName;
			return await ApplyAccessAsync(contextGuid, calleName, await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetFromDatabaseAsync(tafsilGuid, token, tranName), system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 151);
			throw;
		}
	}

	public static async Task<TafsilBusiness> GetAsync(string contextGuid, EnSystem system, Guid tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		_ = 1;
		try
		{
			if (tafsilGuid == Guid.Empty)
			{
				return null;
			}
			ClsCache cache = ClsCache.GetCache(contextGuid);
			if (cache == null)
			{
				return null;
			}
			TafsilBusiness tafsilBusiness = await cache.CacheRepository.ITafsil.GetAsync(tafsilGuid, token, tranName);
			if (system == EnSystem.UserInterface)
			{
				return await ApplyAccessAsync(contextGuid, "GetAsync_2=>" + sourceFilePath + "/" + memberName, tafsilBusiness, system, token, tranName);
			}
			return tafsilBusiness;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 169);
		}
		return null;
	}

	public static async Task<TafsilBusiness> GetAsync(string contextGuid, EnSystem system, long tafsilCode, string tranName = "", CancellationToken token = default(CancellationToken), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		_ = 2;
		try
		{
			if (system == EnSystem.UserInterface)
			{
				string calleName = "GetAsync_1=>" + sourceFilePath + "/" + memberName;
				return await ApplyAccessAsync(contextGuid, calleName, await (ClsCache.GetCache(contextGuid).CacheRepository.ITafsil?.GetAsync(tafsilCode, token, tranName)), system, token, tranName);
			}
			return await (ClsCache.GetCache(contextGuid).CacheRepository.ITafsil?.GetAsync(tafsilCode, token, tranName));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 181);
		}
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, CancellationToken token, bool updateAccount, bool awaitOnSemaphore, short autoFixValidationErrorsCount, bool send2Server, bool await4sent = false, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		int line = 0;
		Stopwatch stopwatch = new Stopwatch();
		Stopwatch swSemaphoreTime = new Stopwatch();
		Stopwatch swDbSemaphoreTime = new Stopwatch();
		stopwatch.Start();
		swSemaphoreTime.Start();
		swDbSemaphoreTime.Start();
		bool semaphoreHasBeenTaken = false;
		try
		{
			_ = 5;
			checked
			{
				ReturnedSaveFuncInfo result;
				try
				{
					semaphoreHasBeenTaken = await _saveSemaphore.WaitAsync(60000, token);
					if (!semaphoreHasBeenTaken)
					{
						result = ret.AddError("Tafsil Save TimeOut !!!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 202);
						goto end_IL_0198;
					}
					swSemaphoreTime.Stop();
					line = 1;
					send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
					line = 2;
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilBusiness/SaveAsync", 60000, token))
					{
						swDbSemaphoreTime.Stop();
						line = 3;
						if (!dis.Token.IsCancellationRequested)
						{
							line = 4;
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 212), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 212);
							}
							line = 5;
							transactionHasBeenCreated = true;
							line = 6;
							if (Code <= 0)
							{
								line = 7;
								ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = await NextCodeInGroupAsync(ContextGuid, system, TafsilGroupGuid, token, tranName);
								line = 8;
								ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 222);
								line = 9;
								Code = returnedSaveFuncInfoWithValue.value;
								line = 10;
							}
							line = 11;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, autoFixValidationErrorsCount, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 228);
							line = 12;
							if (system != EnSystem.UserInterface)
							{
								ret.throwExceptionIfError();
							}
							line = 13;
							_ = ClsCache.GetCache(ContextGuid).Source;
							if (system == EnSystem.UserInterface)
							{
								Version++;
								ServerStatus = ServerStatusForClient.None;
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsil.SaveAsync(this, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 238);
							goto end_IL_02c8;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 210);
						goto end_IL_0198;
						end_IL_02c8:;
					}
					goto end_IL_0109;
					end_IL_0198:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 240);
					goto end_IL_0109;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 243);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 244);
					goto end_IL_0109;
				}
				return result;
			}
			end_IL_0109:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				string debugInfo = $"{Guid}-{Code}-{Name}-{system}";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 251, debugInfo), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 251);
				if (!ret.HasError)
				{
					RaiseAccountChanged(updateAccount);
				}
			}
			if (semaphoreHasBeenTaken)
			{
				_saveSemaphore.Release();
			}
			if (!ret.HasError && send2Server)
			{
				if (await4sent)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, Priority.High), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 258);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, Priority.High));
				}
			}
			_ = ret.HasError;
			SaveLogAsync(system, ret);
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebTafsil webEntity = await TafsilMapper.Instance.MapAsync(ContextGuid, this, new WebTafsil());
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.Tafsil;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره حساب تفصیلی".GetTranslate() + "  (" + $"{webEntity.Code}".GetTranslate() + ") " + (Name ?? "").GetTranslate() + "-" + $"{Guid}" + "\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 300);
		}
	}

	public static Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, TafsilBusiness tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.SaveAsync(tafsil, updateAccount, tryCount, token, tranName);
	}

	private void RaiseAccountChanged(bool updateAccount)
	{
		try
		{
			TafsilEventAggregator aggregator = ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.Tafsil;
			if (!aggregator.HasEventHandler)
			{
				return;
			}
			Task.Factory.StartNew(delegate
			{
				try
				{
					if (updateAccount)
					{
						aggregator.RaiseAccountChanged(this, Guid, "");
					}
					if (HType == HesabType.Sandogh || HType == HesabType.Bank)
					{
						aggregator.RaiseAccountSaved(this, EventArgs.Empty);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "RaiseAccountChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 319);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseAccountChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 323);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationCodeAsync(EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, string tranName, short autoFixValidationErrorsCount, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				TafsilGroupBusiness tafsilGroup = await GetTafsilGroupAsync(system, token, tranName);
				TafsilBusiness tafsilBusiness = await GetAsync(ContextGuid, system, Code, tranName, token, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs");
				line = 1;
				if (tafsilBusiness != null && tafsilBusiness.Guid != Guid)
				{
					if (autoFixValidationErrorsCount > 0 && keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate)
					{
						line = 2;
						TafsilBusiness tafsilBusiness2 = new TafsilBusiness(ContextGuid);
						if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
						{
							throw new InvalidOperationException("در حسابداری بوجود آمد خطای کانفلیکت روی کد حساب تفصیلی غیر مجاز است!".GetTranslate() + "\r\n" + $"System:{system}\r\n" + $"keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate:{keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate}\r\n" + $"Code:{code}\r\n" + "Nane:" + Name + "\r\n" + $"Guid:{Guid}\r\n" + $"ExistsGuid:{tafsilBusiness.Guid}");
						}
						line = 3;
						tafsilBusiness2.TafsilGroupGuid = TafsilGroupGuid;
						line = 4;
						tafsilBusiness.Code = tafsilBusiness2.Code;
						line = 5;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount: false, awaitOnSemaphore: false, 10, send2Server: false, await4sent: false, 10), ReturnedState.None, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 355);
						line = 6;
					}
					else
					{
						if (autoFixValidationErrorsCount > 0 && !keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate)
						{
							line = 7;
							long currentCode = Code;
							line = 8;
							ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = await NextCodeInGroupAsync(ContextGuid, system, TafsilGroupGuid, token, tranName);
							line = 9;
							ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 365);
							line = 10;
							Code = returnedSaveFuncInfoWithValue.value;
							line = 11;
							ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeTafsilCode From:({currentCode}) To:({Code}) Because {currentCode} was Dupplicate", "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 369);
							line = 12;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							TafsilBusiness tafsilBusiness3 = this;
							short num = checked((short)(autoFixValidationErrorsCount - 1));
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness3.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, num, token), ReturnedState.None, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 371);
							line = 13;
							return ret;
						}
						line = 14;
						ret.AddError(system, Name + " -" + " کد تفصیلی به حساب دیگری".GetTranslate() + "(" + (tafsilBusiness.Name ?? "").GetTranslate() + ")" + " اختصاص داده شده است".GetTranslate() + $" {Code}".GetTranslate(), "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 378);
					}
				}
				if (!(await IsCodeTafsilInRangeAsync(ContextGuid, Code, TafsilGroupGuid, token)))
				{
					if (autoFixValidationErrorsCount > 0)
					{
						line = 15;
						long currentCode = Code;
						line = 16;
						ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue2 = await NextCodeInGroupAsync(ContextGuid, system, TafsilGroupGuid, token, tranName);
						line = 17;
						ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 389);
						line = 18;
						Code = returnedSaveFuncInfoWithValue2.value;
						line = 190;
						token.ThrowIfCancellationRequested();
						line = 1910;
						if (tafsilGroup == null)
						{
							throw new NullReferenceException("tafsilGroup is null!!");
						}
						line = 1912;
						ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n " + $"ChangeTafsilCode From:({currentCode}) To:({Code})\r\n" + $"Because {currentCode} Not in Range ({tafsilGroup?.CodeMin ?? 0}-{tafsilGroup?.CodeMax ?? 0})", "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 397);
						line = 20;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						TafsilBusiness tafsilBusiness4 = this;
						short num = checked((short)(autoFixValidationErrorsCount - 1));
						autoFixValidationErrorsCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness4.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, num, token), ReturnedState.None, "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 402);
						line = 21;
						return ret;
					}
					line = 22;
					if (system != EnSystem.Transfer)
					{
						ret.AddError(system, "کد  وارد شده".GetTranslate() + "(" + $"{Code}".GetTranslate() + ")" + "در بازه کد گروه تفصیلی".GetTranslate() + "(" + $"{tafsilGroup?.CodeMin}".GetTranslate() + "-" + $"{tafsilGroup?.CodeMax}".GetTranslate() + ")" + "نمی باشد".GetTranslate() + " \r\n" + "نام حساب".GetTranslate() + ":" + (Name ?? "").GetTranslate() + "\r\n" + "گروه تفصیلی".GetTranslate() + ":" + ((tafsilGroup?.Name ?? "") ?? "").GetTranslate() + "\r\n", "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 410);
					}
				}
				if (system != EnSystem.UserInterface && system != EnSystem.BazsaziData)
				{
					ret.throwExceptionIfError();
				}
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 421);
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}/System:{system}/Guid:{Guid}/Name:{Name}/Code:{code}/TafsilGroup:{TafsilGroupGuid}/CacheIsNull:{ClsCache.GetCache(ContextGuid) == null}", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 422);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, string tranName, short autoFixValidationErrorsCount, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		checked
		{
			using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs"))
			{
				_ = 5;
				try
				{
					if (system == EnSystem.SignalR && ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
					{
						return ret;
					}
					line = 1;
					TafsilGroupBusiness tafsilGroupBusiness = await GetTafsilGroupAsync(system, token, tranName);
					line = 2;
					if (Guid == Guid.Empty)
					{
						ret.AddError(system, "شناسه تهی".GetTranslate(), "Guid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 439);
					}
					line = 5;
					if (DeliveryDate.HasValue && DeliveryDate.Value.Year < 2000)
					{
						DeliveryDate = null;
					}
					line = 10;
					if (dateM.Year < 2000)
					{
						dateM = DateTime.Now;
					}
					line = 15;
					if (string.IsNullOrEmpty(Name))
					{
						ret.AddError(system, "حساب تفصیلی فاقد نام قابل ثبت نمی باشد".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 447);
					}
					line = 200;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (tafsilGroupBusiness != null && !tafsilGroupBusiness.Status)
					{
						line = 201;
						if (autoFixValidationErrorsCount > 0 && system != EnSystem.UserInterface)
						{
							line = 2020;
							_ = Guid.Empty;
							line = 2021;
							Guid guid;
							switch (HType)
							{
							case HesabType.Hazine:
								line = 2021;
								guid = ParentDefaults.TafsilLevelGuids.Hazineha;
								line = 2022;
								break;
							case HesabType.Bardasht:
								line = 2023;
								guid = ParentDefaults.TafsilLevelGuids.Bardashts;
								line = 2024;
								break;
							case HesabType.Sandogh:
								line = 2025;
								guid = ParentDefaults.TafsilLevelGuids.Sandooghs;
								line = 2026;
								break;
							case HesabType.Bank:
								line = 2027;
								guid = ParentDefaults.TafsilLevelGuids.Banks;
								line = 2028;
								break;
							case HesabType.Customers:
								line = 2029;
								guid = ParentDefaults.TafsilLevelGuids.PersonsAndCompany;
								line = 2030;
								break;
							case HesabType.Hesab:
								line = 2029;
								guid = ParentDefaults.TafsilLevelGuids.Others;
								line = 2030;
								break;
							default:
								throw new ArgumentOutOfRangeException($"hesabType({HType}) not defined!");
							}
							if (guid == Guid.Empty)
							{
								string contextGuid = ContextGuid;
								CancellationToken token2 = token;
								guid = (await TafsilGroupBusiness.GetAllAsync(contextGuid, system, true, token2))?.FirstOrDefault()?.Guid ?? TafsilGroupGuid;
							}
							line = 203;
							ret.AddInformation("گروه تفصیلی حساب".GetTranslate() + "(" + (Name ?? "").GetTranslate() + ")(" + $"{Guid}" + ")" + " به علت غیر فعال بودن".GetTranslate() + "\r\n" + "به".GetTranslate() + " " + $"{guid}".GetTranslate() + " " + "تغییر یافت".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 496);
							line = 204;
							TafsilGroupGuid = guid;
							line = 205;
							returnedSaveFuncInfo = ret;
							TafsilBusiness tafsilBusiness = this;
							short num = (short)(autoFixValidationErrorsCount - 1);
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, num, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 501);
							line = 205;
							return ret;
						}
						line = 206;
						ret.AddError(system, "گروه تفصیلی".GetTranslate() + "(" + (tafsilGroupBusiness.Name ?? "").GetTranslate() + ")" + " مسدود شده".GetTranslate() + "\r\n" + "لطفا گروه تفصیلی مربوط به حساب".GetTranslate() + " (" + (Name ?? "").GetTranslate() + ")" + "را تغییر دهید.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 508);
						line = 207;
					}
					line = 208;
					if (tafsilGroupBusiness == null)
					{
						if (autoFixValidationErrorsCount > 0)
						{
							line = 400;
							Guid currentTafsilGroupGuid = TafsilGroupGuid;
							line = 401;
							string contextGuid2 = ContextGuid;
							CancellationToken token2 = token;
							IEnumerable<TafsilGroupBusiness> enumerable = await TafsilGroupBusiness.GetAllAsync(contextGuid2, system, true, token2, tranName);
							TafsilGroupGuid = enumerable?.SingleOrDefault((TafsilGroupBusiness p) => p.Tbl == "CUS")?.Guid ?? Guid.Empty;
							if (TafsilGroupGuid == Guid.Empty)
							{
								TafsilGroupGuid = enumerable?.FirstOrDefault()?.Guid ?? Guid.Empty;
							}
							line = 402;
							ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeTafsilGroupGuid From:({currentTafsilGroupGuid}) To:({TafsilGroupGuid})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 526);
							line = 403;
							returnedSaveFuncInfo = ret;
							TafsilBusiness tafsilBusiness2 = this;
							short num = (short)(autoFixValidationErrorsCount - 1);
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness2.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, num, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 528);
							line = 404;
							return ret;
						}
						ret.AddError(system, "گروه تفصیلی انتخاب نشده است".GetTranslate() + "\r\n", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 533);
					}
					line = 50;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationCodeAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, autoFixValidationErrorsCount, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 546);
					line = 80;
					if (system != EnSystem.UserInterface && system != EnSystem.BazsaziData)
					{
						ret.throwExceptionIfError();
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 554);
					ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 555);
				}
				finally
				{
					if (ret.HasError && system != EnSystem.UserInterface)
					{
						WebErrorLog.Instance.StartLog("❌TafsilValidationHas Error:\r\n❌Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 560);
					}
				}
				return ret;
			}
		}
	}

	public static Task<string> NextTafsilSayerAsync(string contextGuid, Guid ParentGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.NextTafsilSayerAsync(ParentGuid, token, tranName);
	}

	private static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<TafsilBusiness> lstHesabsListTafsil, CancellationToken token, short maxParalelTasks = 20)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		string tranName = Guid.NewGuid().ToString();
		bool transactionHasBeenCreated = false;
		int line = 0;
		Stopwatch.StartNew();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				line = 1;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "TafsilBusiness/SaveAsync", 60000, token))
				{
					line = 3;
					if (!dis.Token.IsCancellationRequested)
					{
						line = 4;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 581), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 581);
						line = 5;
						transactionHasBeenCreated = true;
						line = 6;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.SaveRangeAsync(lstHesabsListTafsil, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 585);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 579);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 587);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 590);
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 591);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (transactionHasBeenCreated)
			{
				string debugInfo = "SaveRangeForHesabyarAsync";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 598, debugInfo), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 598);
			}
			_ = ret.HasError;
		}
		if (ret.HasError && lstHesabsListTafsil.Count() == 1)
		{
			ret = await lstHesabsListTafsil.Single().SaveAsync(EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: true, token, updateAccount: true, awaitOnSemaphore: false, 10, send2Server: false, await4sent: false, 10);
		}
		else if (ret.HasError && lstHesabsListTafsil.Count() > 1 && !token.IsCancellationRequested)
		{
			ret = new ReturnedSaveFuncInfo();
			List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
			List<TafsilBusiness> list2 = new List<TafsilBusiness>();
			TafsilBusiness[] array = lstHesabsListTafsil.ToArray();
			for (int i = 0; i < lstHesabsListTafsil.Count() / 2; i = checked(i + 1))
			{
				list2.Add(array[i]);
			}
			list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token, maxParalelTasks));
			list.Add(SaveRangeForHesabyarAsync(contextGuid, lstHesabsListTafsil.Except(list2), token, maxParalelTasks));
			ReturnedSaveFuncInfo result = ret;
			result.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 616);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, IEnumerable<TafsilBusiness> lstHesabsListTafsil, bool updateAccount, short autoFixValidationErrorsCount, CancellationToken token, short maxParalelTasks = 20, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar && system == EnSystem.SignalR)
			{
				return await SaveRangeForHesabyarAsync(contextGuid, lstHesabsListTafsil, token, maxParalelTasks);
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			CancellationTokenSource invokeToken2 = new CancellationTokenSource();
			int current = 0;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (TafsilBusiness item in lstHesabsListTafsil)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 634);
				}
				tasks.Add(Task.Run(() => item.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, token, updateAccount, awaitOnSemaphore: true, autoFixValidationErrorsCount, send2Server: false, await4sent: false, 10)));
				if (tasks.Count >= maxParalelTasks)
				{
					Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
					tasks.Remove(task);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 640);
					if (percentCompleted != null)
					{
						invokeToken2?.Cancel();
						invokeToken2 = new CancellationTokenSource(60000);
						CancellationToken token2 = invokeToken2.Token;
						percentCompleted(new PercenteCompletedEventArgs(lstHesabsListTafsil.Count(), current, item.Name, token2));
					}
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 650);
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsListTafsil.Count(), lstHesabsListTafsil.Count(), "", invokeTkn));
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 661);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 662);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(string contextGuid, Guid guid, ServerStatusForClient status, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 674), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 674);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.UpdateServerStatusAsync(guid, status, token, tranName), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 676);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 680);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 681);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 686), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 686);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(string contextGuid, Guid guid, decimal p0, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				if (guid == Guid.Empty)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 699), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 699);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.UpdateAccountAsync(guid, p0, tryCount, token, tranName), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 701);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 705);
				ret.AddReturnedValue(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 706);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 711), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 711);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateSumAghsatAsync(string contextGuid, Guid guid, decimal price, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				if (guid == Guid.Empty)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 724), ReturnedState.None, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 724);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.UpdateSumAghsatAsync(guid, price, tryCount, token, tranName), ReturnedState.None, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 726);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 730);
				ret.AddReturnedValue(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 731);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 736), ReturnedState.None, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 736);
			}
		}
		return ret;
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> select, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 748), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 748);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.KeepRangeAsync(select, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 749);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 753);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 754);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 759), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 759);
			}
		}
		return ret;
	}

	public static async Task<TafsilBusiness> GetByTypeAndCodeAsync(string contextGuid, EnSystem system, HesabType hesabType, long code, string tranName = "", CancellationToken token = default(CancellationToken), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		string calleName = "GetByTypeAndCodeAsync=>" + sourceFilePath + "/" + memberName;
		return await ApplyAccessAsync(contextGuid, calleName, await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetByTypeAndCodeAsync(hesabType, code, token, tranName), system, token, tranName);
	}

	public static async Task<decimal> GetSandoghsBalanceAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<SandoghBusiness> enumerable = await SandoghBusiness.GetAllAsync(contextGuid, system, "", true, "", token, new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein7 });
			if (enumerable == null || !enumerable.Any())
			{
				return default(decimal);
			}
			decimal ret = default(decimal);
			foreach (SandoghBusiness item in enumerable)
			{
				ret += await AccountBusiness.GetAccountSumAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein7, item.Guid, true, tranName, token);
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSandoghsBalanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 782);
			throw;
		}
	}

	public static async Task<decimal> GetBanksBalanceAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<BankBusiness> enumerable = await BankBusiness.GetAllAsync(contextGuid, system, "", token, true, "", new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein4 });
			if (enumerable == null || !enumerable.Any())
			{
				return default(decimal);
			}
			decimal ret = default(decimal);
			foreach (BankBusiness item in enumerable)
			{
				ret += await AccountBusiness.GetAccountSumAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein4, item.Guid, true, tranName, token);
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBanksBalanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 802);
			throw;
		}
	}

	public static async Task<bool> IsCodeTafsilInRangeAsync(string contextGuid, long code, Guid tfGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			TafsilGroupBusiness tafsilGroupBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.GetAsync(tfGuid, token, tranName);
			line = 1;
			if (tafsilGroupBusiness == null)
			{
				return false;
			}
			line = 2;
			if (tafsilGroupBusiness.CodeMin <= code && tafsilGroupBusiness.CodeMax >= code)
			{
				return true;
			}
			line = 3;
			return false;
		}
		catch (Exception ex)
		{
			if (ClsCache.GetCache(contextGuid) != null)
			{
				Log.Warning(ex, $"#Line_{line}/CahceIsNull!!!");
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}/CahceIsNull:false", "IsCodeTafsilInRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 824);
			}
			return false;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(string contextGuid, EnSystem system, Guid tfLevelGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<long> ret = new ReturnedSaveFuncInfoWithValue<long>();
		int line = 0;
		try
		{
			if (tfLevelGuid == Guid.Empty)
			{
				line = 1;
				ret.AddError(system, "گروه تفصیلی انتخاب نشده است".GetTranslate(), "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 837);
				line = 2;
				return ret;
			}
			line = 3;
			EnSuggestionType suggestionMode = EnSuggestionType.FillGaps;
			if (system != EnSystem.Transfer)
			{
				suggestionMode = ((await LocalSettingBusiness.GetSettingAsync(contextGuid, token))?.Customers?.SuggestionType).GetValueOrDefault(EnSuggestionType.FillGaps);
			}
			ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.NextCodeInGroupAsync(tfLevelGuid, suggestionMode, token, tranName), ReturnedState.None, "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 848);
		}
		catch (Exception ex)
		{
			if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ITafsil == null)
			{
				Log.Warning(ex, $"#Line_{line}/tfLevelGuid:{tfLevelGuid}/System:{system}/ClsCache.GetCache(contextGuid).CacheRepository.ITafsil is null:true");
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}/tfLevelGuid:{tfLevelGuid}/System:{system}/ClsCache.GetCache(contextGuid).CacheRepository.ITafsil is null:false", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 855);
			}
			ret.AddReturnedValue(ex, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 856);
		}
		return ret;
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.DbCountAsync(status, token, tranName);
	}

	public static TafsilBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.Get(guid, token, tranName);
		}
		return null;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ITafsil;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
				using (await _databaseSemaphore.GaurdAsync(true, "tafsil/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 876), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 876);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsil.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 878);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 880);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 883);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 884);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 889), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 889);
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		EnSystem system = EnSystem.SignalR;
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Tafsil;
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال حساب های تفصیلی".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 914);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(system, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 915);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 917);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ITafsil == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ITafsil is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 924);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("حساب های تفصیلی سینک هستند تعداد حساب تفصیلی سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 925);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 926);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 934);
						}
						TafsilBusiness tafsilBusiness = await GetFromDatabaseAsync(contextGuid, EnSystem.SignalR, item, tranName, token, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs");
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 936);
						}
						if (tafsilBusiness == null)
						{
							throw new NullReferenceException($"obj is null!! contextGuid({contextGuid}) ItemGuid({item})");
						}
						if (string.IsNullOrEmpty(tafsilBusiness?.ContextGuid))
						{
							throw new NullReferenceException("obj?.ContextGuid is null or empty!!");
						}
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "حساب تفصیلی جاری".GetTranslate() + ":" + (" " + tafsilBusiness?.Name).GetTranslate();
							uiUpdater.Count = current;
						}
						if (tafsilBusiness.ServerStatus == ServerStatusForClient.Delivered || tafsilBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Tafsil \r\n" + $"EntityGuid:{tafsilBusiness?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + tafsilBusiness?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 947);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 953);
						}
						tasks.Add(tafsilBusiness.Send2ServerAsync(token, EnSystem.SignalR));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 958);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 962);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 964);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 967);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 968);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_resendToServerSemaphore.Release();
				}
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs"))
		{
			Stopwatch sw = Stopwatch.StartNew();
			int line = 0;
			try
			{
				line = 1;
				WebErrorLog.Instance.Trace($"Sending tafsil... Name:{Name}/Code:{code}/Guid:{Guid}", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 998);
				line = 2;
				_sendToken?.Cancel();
				line = 3;
				_sendToken = new CancellationTokenSource();
				line = 4;
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "TafsilBusiness/Send2ServerAsync", 60000, token, token2);
				line = 5;
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1007);
				}
				line = 6;
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache == null || !cache.BufferEnable || cache.BufferLoginState.Status != EnLoginResult.Success)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1010);
				}
				List<Guid> relatedMoeinGuidList = null;
				if (cache.Source == ENSource.Accounting)
				{
					bool semaphoreHasBeenTaken = await _connectionPoolSemaphore.WaitAsync(60000, token);
					try
					{
						relatedMoeinGuidList = (await AccountBusiness.GetAllAsync(ContextGuid, Guid, isLoadDefMoein: false, token: token, status: true))?.Select((AccountTableViewModel q) => q.MoeinGuid)?.Distinct()?.ToList();
					}
					finally
					{
						if (semaphoreHasBeenTaken)
						{
							_connectionPoolSemaphore.Release();
						}
					}
				}
				WebTafsil webTafsil = await TafsilMapper.Instance.MapAsync(ContextGuid, this, new WebTafsil(), "", token);
				webTafsil.RelatedMoeinGuidList = relatedMoeinGuidList;
				List<Packet> lstPacket = WebTafsil.GenerateSaveRequest(new List<WebTafsil> { webTafsil }, 0);
				line = 7;
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1027);
				}
				line = 8;
				using TafsilWaiter waiter = new TafsilWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				line = 9;
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				line = 10;
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("TafsilBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1034);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1036);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1038);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1041);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1042);
			}
			finally
			{
				WebErrorLog.Instance.Trace($"Tafsil Sent time:{sw.ElapsedMilliseconds}(ms) result has Error:{ret.HasError} {ret.GetErrorMessage()}//TokenCancelled:{token.IsCancellationRequested}// =>Name:{Name}/Code:{code}/Guid:{Guid}", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1046);
			}
			return ret;
		}
	}

	public static async Task<TafsilBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? guid = null, Guid? tafsilGroupGuid = null, string description = "", bool isSystem = false, HesabType? hType = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		TafsilBusiness ret;
		try
		{
			ret = new TafsilBusiness(contextGuid)
			{
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				System = isSystem
			};
			if ((tafsilGroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				ret.TafsilGroupGuid = tafsilGroupGuid.Value;
				ret.code = (await NextCodeInGroupAsync(contextGuid, system, tafsilGroupGuid.Value, token))?.value ?? 0;
			}
			if (!string.IsNullOrEmpty(description))
			{
				ret.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				ret.Guid = guid.Value;
			}
			else
			{
				ret.Guid = Guid.NewGuid();
			}
			if (hType.HasValue)
			{
				ret.HType = hType.Value;
			}
			else
			{
				ret.HType = HesabType.Hesab;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1081);
			throw;
		}
		return ret;
	}

	private static void ApplyTafsilGroupAccess(List<Guid> tafsilGroupGuidList, IEnumerable<TafsilBusiness> lst)
	{
		try
		{
			if (tafsilGroupGuidList == null)
			{
				return;
			}
			List<TafsilBusiness> list = lst?.ToList();
			tafsilGroupGuidList = tafsilGroupGuidList?.Where((Guid q) => q != Guid.Empty)?.ToList();
			List<Guid> list2 = tafsilGroupGuidList;
			if (list2 == null || !list2.Any())
			{
				if (list == null || !list.Any())
				{
					return;
				}
				{
					foreach (TafsilBusiness item in list)
					{
						item.Account = 0m;
					}
					return;
				}
			}
			tafsilGroupGuidList = tafsilGroupGuidList?.Where((Guid q) => q != Guid.Empty)?.ToList();
			List<Guid> list3 = tafsilGroupGuidList;
			if (list3 == null || !list3.Any())
			{
				return;
			}
			List<Guid> validGuids = lst.Where((TafsilBusiness q) => tafsilGroupGuidList.Contains(q.TafsilGroupGuid))?.Select((TafsilBusiness q) => q.Guid)?.ToList();
			List<Guid> list4 = validGuids;
			if (list4 == null || !list4.Any())
			{
				return;
			}
			IEnumerable<TafsilBusiness> enumerable = list?.Where((TafsilBusiness q) => !validGuids.Contains(q.Guid));
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (TafsilBusiness item2 in enumerable)
			{
				item2.Account = 0m;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyTafsilGroupAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1114);
			throw;
		}
	}

	private static async Task<IEnumerable<TafsilBusiness>> ApplyAccessAsync(string contextGuid, string calleName, IEnumerable<TafsilBusiness> items, EnSystem system, CancellationToken token, string tranName = "")
	{
		int line = 0;
		StringBuilder sb = new StringBuilder();
		try
		{
			IEnumerable<TafsilBusiness> originalData = items;
			sb.Append($"Total Items Count:{items?.Count() ?? 0}");
			line = 1;
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (system != EnSystem.UserInterface)
			{
				return items;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			line = 2;
			if (currentCashier == null)
			{
				return items;
			}
			line = 3;
			if (token.IsCancellationRequested)
			{
				return null;
			}
			UserAccBusiness user = await currentCashier.GetUserAsync(token, tranName);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (user == null)
			{
				throw new NullReferenceException("UserIs Null!!");
			}
			line = 4;
			if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
			{
				items = items?.Where((TafsilBusiness p) => p != null && user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid));
				if ((currentCashier?.VisitorGuid ?? Guid.Empty) != Guid.Empty)
				{
					TafsilBusiness tafsilBusiness = originalData?.SingleOrDefault((TafsilBusiness q) => q != null && q.Guid == currentCashier.VisitorGuid.Value);
					if (tafsilBusiness != null && !(items?.Select((TafsilBusiness q) => q.Guid)?.Contains(tafsilBusiness.Guid)).GetValueOrDefault())
					{
						items = items?.ToList();
						((List<TafsilBusiness>)items).Add(tafsilBusiness);
					}
				}
				sb.AppendLine($"after  TafsilGroupJustListGuid apply access=> Items Count:{items?.Count() ?? 0}");
			}
			line = 5;
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if ((currentCashier.VisitorGuid ?? Guid.Empty) != Guid.Empty && !currentCashier.AllVisitorAccess)
			{
				IEnumerable<CusCompactBusiness> enumerable = await CusCompactBusiness.GetAllAsync(contextGuid, EnSystem.None, token, null, null, null, tranName);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				line = 6;
				List<Guid> accessedCus = enumerable?.Where(delegate(CusCompactBusiness p)
				{
					Guid visitorGuid = p.VisitorGuid;
					Guid? visitorGuid2 = currentCashier.VisitorGuid;
					if (!(visitorGuid == visitorGuid2))
					{
						visitorGuid = p.Guid;
						visitorGuid2 = currentCashier.VisitorGuid;
						return visitorGuid == visitorGuid2;
					}
					return true;
				})?.Select((CusCompactBusiness p) => p.Guid)?.ToList();
				line = 7;
				if (!(accessedCus?.Any() ?? false))
				{
					items = items?.Where((TafsilBusiness p) => p != null && p.HType != HesabType.Customers);
					sb.AppendLine($"after cod1 apply access=> Items Count:{items?.Count() ?? 0}");
				}
				else
				{
					items = items?.Where((TafsilBusiness p) => p != null && (p.HType != HesabType.Customers || accessedCus.Contains(p.Guid)));
					sb.AppendLine($"after cod2 apply access=> Items Count:{items?.Count() ?? 0}");
				}
				line = 8;
			}
			if (!currentCashier.AllSandoghsAccess)
			{
				items = items?.Where((TafsilBusiness p) => p != null && (p.HType != HesabType.Sandogh || p.Guid == (currentCashier.SandoghGuid ?? Guid.Empty)));
				sb.AppendLine($"after cod3 apply access=> Items Count:{items?.Count() ?? 0}");
			}
			if (!currentCashier.AllBanksAccess)
			{
				items = items?.Where((TafsilBusiness p) => p != null && (p.HType != HesabType.Bank || p.Guid == (currentCashier.BankGuid1 ?? Guid.Empty) || p.Guid == (currentCashier.BankGuid2 ?? Guid.Empty) || p.Guid == (currentCashier.BankGuid3 ?? Guid.Empty) || p.Guid == (currentCashier.BankGuid4 ?? Guid.Empty)));
				sb.AppendLine($"after cod4 apply access=> Items Count:{items?.Count() ?? 0}");
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (!user.UserAccess.Customers.Customer_DastrasiBeAshkhaseKhas)
			{
				items = items?.Where((TafsilBusiness q) => q != null && (q.HType != HesabType.Customers || !q.IsSpecial));
				sb.AppendLine($"after cod4 apply access=> Items Count:{items?.Count() ?? 0}");
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			ApplyTafsilGroupAccess(await CashierBusiness.GetAllTafsilGroupGuidsForTurnoverAsync(contextGuid, system, token, tranName), items);
			line = 9;
			items = items?.ToList();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"The Error Line IS : {line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1206);
			throw;
		}
		finally
		{
			_ = system;
		}
		if (token.IsCancellationRequested)
		{
			return null;
		}
		return items;
	}

	private static async Task<TafsilBusiness> ApplyAccessAsync(string contextGuid, string calleName, TafsilBusiness item, EnSystem system, CancellationToken token, string tranName)
	{
		if (system == EnSystem.UserInterface)
		{
			return (await ApplyAccessAsync(contextGuid, calleName, new List<TafsilBusiness> { item }, system, token, tranName))?.SingleOrDefault();
		}
		return item;
	}

	internal static Task<List<Guid>> CheckExistAsync(string contextGuid, List<Guid> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.CheckExistAsync(lst, token, tranName);
	}

	public static async Task<TafsilInstallmentsRecord> GetInstallmentReportAsync(string contextGuid, Guid tafsilGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			if (tafsilGuid == Guid.Empty)
			{
				return null;
			}
			TafsilBusiness tafsilBusiness = await GetAsync(contextGuid, system, tafsilGuid, tranName, token, "GetInstallmentReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs");
			if (tafsilBusiness == null)
			{
				return null;
			}
			TafsilInstallmentsRecord data = new TafsilInstallmentsRecord
			{
				Account = tafsilBusiness.Account,
				TafsilGuid = tafsilGuid,
				TafsilName = tafsilBusiness.Name
			};
			AghsatMFilter filter = new AghsatMFilter
			{
				Status = EnAghsatMStatus.NotRecieved,
				TafsilGuid = tafsilGuid
			};
			IEnumerable<AghsatMBusiness> enumerable = await AghsatMBusiness.GetAllAsync(contextGuid, filter, token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				data.AghsatMCount = enumerable.Count();
				data.AghsatMSum = enumerable.Sum((AghsatMBusiness q) => q.TotalPrice);
			}
			AghsatShFilter filter2 = new AghsatShFilter
			{
				Status = EnAghsatShStatus.NotPayed,
				TafsilGuid = tafsilGuid
			};
			List<AghsatShBusiness> list = await AghsatShBusiness.GetAllAsync(contextGuid, filter2, token, tranName);
			if (list != null && list.Any())
			{
				data.AghsatShCount = list.Count();
				data.AghsatShSum = list.Sum((AghsatShBusiness q) => q.Price);
			}
			return data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstallmentReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1263);
			throw;
		}
	}

	public async Task<bool> CanShowTurnoverAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return false;
			}
			if (Guid == Guid.Empty || TafsilGroupGuid == Guid.Empty)
			{
				return false;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
			if (cashierBusiness == null)
			{
				return false;
			}
			if ((cashierBusiness?.VisitorGuid ?? Guid.Empty) != Guid.Empty && Guid == cashierBusiness.VisitorGuid.Value)
			{
				return true;
			}
			AccessLevel accessLevel = await cashierBusiness.GetUserAccessAsync(token, tranName);
			if (accessLevel == null)
			{
				return false;
			}
			if (!(accessLevel.TafsilGroupGuid?.Any() ?? false) && !(accessLevel.TafsilGroupJustListGuid?.Any() ?? false))
			{
				return true;
			}
			if (!(accessLevel.TafsilGroupGuid?.Any() ?? false) && (accessLevel.TafsilGroupJustListGuid?.Any() ?? false))
			{
				return false;
			}
			return accessLevel.TafsilGroupGuid.Any((Guid q) => q == TafsilGroupGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanShowTurnoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilBusiness.cs", 1286);
			throw;
		}
	}
}
