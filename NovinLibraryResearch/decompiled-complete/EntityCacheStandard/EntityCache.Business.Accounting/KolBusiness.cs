using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business.Accounting;

public class KolBusiness : BaseBusiness<KolBusiness>, IKol, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<KolBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _name = "";

	private string _description = "";

	private DateTime dateM = DateTime.Now;

	private Guid _guid;

	private Guid _groupGuid;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid
	{
		get
		{
			return _guid;
		}
		set
		{
			try
			{
				if (_guid == Guid.Empty || _guid == value)
				{
					_guid = value;
					return;
				}
				throw new InvalidOperationException($"change MoeinGuid from ({_guid}) to ({value}) is not valid!");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 52);
			}
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


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

	public Guid GroupGuid
	{
		get
		{
			return _groupGuid;
		}
		set
		{
			SetGroupGuid(EnSystem.None, value);
		}
	}

	public ENMahiat Mahiat { get; set; }

	public int Code { get; set; }

	public bool Status { get; set; }

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

	public decimal Account { get; set; }

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

	public byte[] ServerStatusImage
	{
		get
		{
			try
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
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServerStatusImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 105);
			}
			return null;
		}
	}

	public byte[]? GardeshHesab { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public void SetGroupGuid(EnSystem system, Guid value)
	{
		try
		{
			if (system == EnSystem.UserInterface && _groupGuid != Guid.Empty && _groupGuid != value)
			{
				throw new InvalidOperationException($"HesabGroupGuid change from ({_groupGuid}) to ({value})");
			}
			_groupGuid = value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetGroupGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 71);
		}
	}

	public KolBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<KolBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetAsync(guid, token, tranName);
	}

	public static KolBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IKol.Get(guid, token, tranName);
		}
		return null;
	}

	public static async Task<int> GetStartCodeAsync(string contextGuid, Guid groupGuid, CancellationToken token, string tranName = "")
	{
		string text;
		try
		{
			HesabGroupBusiness group = await HesabGroupBusiness.GetAsync(contextGuid, groupGuid, token, tranName);
			if (group == null)
			{
				throw new NullReferenceException($"HesabGroup({groupGuid}) is not exists");
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			text = group.Code.ToString();
			while (text.Length < accGlobalSettings.Sanads.KolLen)
			{
				text += "0";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStartCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 139);
			throw;
		}
		return text.ParseToInt();
	}

	public static async Task<int> GetEndCodeAsync(string contextGuid, Guid groupGuid, CancellationToken token, string tranName = "")
	{
		string text;
		try
		{
			HesabGroupBusiness group = await HesabGroupBusiness.GetAsync(contextGuid, groupGuid, token, tranName);
			if (group == null)
			{
				throw new NullReferenceException($"HesabGroup({groupGuid}) is not exists");
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			text = group.Code.ToString();
			while (text.Length < accGlobalSettings.Sanads.KolLen)
			{
				text += "9";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetEndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 158);
			throw;
		}
		return text.ParseToInt();
	}

	public static async Task<int> NextCodeInGroupAsync(string contextGuid, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			IKolRepository iKol = ClsCache.GetCache(contextGuid).CacheRepository.IKol;
			int num = await iKol.NextCodeInGroupAsync(groupGuid, await GetStartCodeAsync(contextGuid, groupGuid, token, tranName), await GetEndCodeAsync(contextGuid, groupGuid, token, tranName), token, tranName);
			if (num <= 0)
			{
				num = await GetStartCodeAsync(contextGuid, groupGuid, token, tranName);
			}
			return num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 174);
			throw;
		}
	}

	public async Task<bool> CheckCodeInRange(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			int code = Code;
			if (code > await GetEndCodeAsync(ContextGuid, GroupGuid, token))
			{
				return false;
			}
			code = Code;
			if (code < await GetStartCodeAsync(ContextGuid, GroupGuid, token))
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCodeInRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 190);
			throw;
		}
	}

	public static Task<KolBusiness> GetAsync(string contextGuid, int codeKol, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetAsync(codeKol, token, tranName);
	}

	public static Task<IEnumerable<KolBusiness>> GetAllAsync(string contextGuid, string search = "", CancellationToken token = default(CancellationToken), bool? status = true, Guid? groupHesab = null, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetAllAsync(search, token, status, groupHesab, tranName);
	}

	public static Task<IEnumerable<KolBusiness>> GetAllBetweenCodesAsync(string contextGuid, long? code1 = null, long? code2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetAllBetweenCodesAsync(code1, code2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool awaitOnSemaphore, bool updateAccount, short autoFixValidationErrorsCount, CancellationToken token, Priority priority, bool sendToServer, bool await4send = false, short tryCount = 10, string tranName = "")
	{
		int line = 0;
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "KolBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 211);
					}
					else
					{
						line = 200;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 213), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 213);
						}
						line = 201;
						transactionHasBeenCreated = true;
						line = 202;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName, autoFixValidationErrorsCount, system, !Status), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 217);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							line = 5;
							if (autoFixValidationErrorsCount > 0)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await FillGroupHesabAndTypeAsync(system, dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 221);
							}
							if (!ret.HasError)
							{
								line = 6;
								Console.WriteLine($"Kol HesabGroup Guid:{GroupGuid}");
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IKol.SaveAsync(this, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 226);
								goto end_IL_01ce;
							}
							result = ret;
						}
					}
					goto end_IL_00ed;
					end_IL_01ce:;
				}
				goto end_IL_00d9;
				end_IL_00ed:;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 228);
				goto end_IL_00d9;
			}
			catch (Exception ex2)
			{
				if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex2, $"IsDisposed:{base.IsDisposed}//Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null{ClsCache.GetCache(ContextGuid)?.DatabaseManager == null}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{base.IsDisposed}//Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null{ClsCache.GetCache(ContextGuid)?.DatabaseManager == null}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 234);
				}
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 235);
				goto end_IL_00d9;
			}
			return result;
			end_IL_00d9:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 240), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 240);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 243);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
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
				WebKol webEntity = await KolMapper.Instance.MapAsync(ContextGuid, this, new WebKol());
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
				cashiersLogBusiness.SanadType = EnLog.Kol;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره حساب دفتر کل".GetTranslate() + "(" + $"{webEntity.Code}".GetTranslate() + ")" + (" " + Name).GetTranslate() + " " + $"-{Guid}\r\n" + (ret.ErrorMessage ?? "").GetTranslate() + "\r\n" + (Json.ToStringJson(webEntity, indented: true) ?? "").GetTranslate();
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 280);
		}
	}

	private async Task<ReturnedSaveFuncInfo> FillGroupHesabAndTypeAsync(EnSystem system, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (GroupGuid == Guid.Empty)
			{
				line = 5;
				SetGroupGuid(EnSystem.None, ParentDefaults.HesabGroupGuids.DarayiJari);
				HesabGroupBusiness groupDarayiJari2 = await HesabGroupBusiness.GetAsync(ContextGuid, ParentDefaults.HesabGroupGuids.DarayiJari, token);
				line = 10;
				if (groupDarayiJari2 == null)
				{
					line = 11;
					groupDarayiJari2 = new HesabGroupBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						Guid = ParentDefaults.HesabGroupGuids.DarayiJari,
						Modified = DateTime.Now,
						Name = "دارایی های جاری-ثبت اتومات".GetTranslate(),
						Status = true,
						Code = 0,
						TypeGuid = ParentDefaults.HesabTypeGuids.Darayi
					};
					line = 15;
					HesabTypeBusiness hesabTypeBusiness = await HesabTypeBusiness.GetAsync(ContextGuid, ParentDefaults.HesabTypeGuids.Darayi, token);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "FillGroupHesabAndTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 310);
					}
					line = 20;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (hesabTypeBusiness == null)
					{
						line = 25;
						hesabTypeBusiness = new HesabTypeBusiness(ContextGuid)
						{
							ContextGuid = ContextGuid,
							Guid = ParentDefaults.HesabTypeGuids.Darayi,
							Name = "دارایی - ثبت اتوماتیک".GetTranslate()
						};
						line = 30;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await HesabTypeBusiness.SaveRangeAsync(token, new List<HesabTypeBusiness> { hesabTypeBusiness }, 10), ReturnedState.None, "FillGroupHesabAndTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 322);
					}
					line = 40;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await groupDarayiJari2.SaveAsync(system, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FillGroupHesabAndTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 325);
				}
				line = 45;
			}
			else if (await HesabGroupBusiness.GetAsync(ContextGuid, GroupGuid, token) == null)
			{
				line = 50;
				SetGroupGuid(EnSystem.None, ParentDefaults.HesabGroupGuids.DarayiJari);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "FillGroupHesabAndTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 337);
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "FillGroupHesabAndTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 338);
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfoWithValue<KolBusiness>> SaveAsnadDaryaftaniDefaultsync(string contextGuid, EnSystem system)
	{
		ReturnedSaveFuncInfoWithValue<KolBusiness> ret = new ReturnedSaveFuncInfoWithValue<KolBusiness>();
		CancellationToken token = default(CancellationToken);
		try
		{
			ReturnedSaveFuncInfoWithValue<KolBusiness> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = await GetAsync(contextGuid, ParentDefaults.KolCoding.ClsKol8, token);
			if (ret.value != null && ret.value.Guid == ParentDefaults.KolCoding.ClsKol8)
			{
				return ret;
			}
			ret.value = new KolBusiness(contextGuid)
			{
				Account = 0m,
				Code = 1,
				DateM = DateTime.Now,
				DeliveryDate = DateTime.Now,
				ContextGuid = contextGuid,
				DeliveryId = 0,
				DeliveryMessage = "",
				DeliveryShortMessage = "",
				Description = "",
				Guid = ParentDefaults.KolCoding.ClsKol8,
				Name = "اسناد دریافتنی".GetTranslate(),
				HostRefrenced = "",
				Mahiat = ENMahiat.Debit_Credit,
				Modified = DateTime.Now,
				ServerStatus = ServerStatusForClient.Delivered,
				Status = true,
				System = true
			};
			ret.value.SetGroupGuid(system, ParentDefaults.HesabGroupGuids.DarayiJari);
			returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await ret.value.SaveAsync(system, awaitOnSemaphore: true, updateAccount: false, 10, token, Priority.Low, sendToServer: false, await4send: false, 10), ReturnedState.None, "SaveAsnadDaryaftaniDefaultsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 371);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsnadDaryaftaniDefaultsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 375);
			ret.AddReturnedValue(ex, "", "SaveAsnadDaryaftaniDefaultsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 376);
		}
		return ret;
	}

	internal static Task<int> CountChilderenNotZeroAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IKol.CountChilderenNotZeroAsync(guid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<KolBusiness> lstHesabsListKol, bool sendToServer, Priority priority = Priority.Low, bool updateAccount = false, short autoFixValidationErrorsCount = 0, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), bool wait4Send = false, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CancellationTokenSource invokeToken = new CancellationTokenSource();
			int current = 0;
			if (lstHesabsListKol == null || !lstHesabsListKol.Any())
			{
				return ret;
			}
			foreach (KolBusiness kol in lstHesabsListKol)
			{
				current = checked(current + 1);
				Stopwatch.StartNew();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await kol.SaveAsync(system, awaitOnSemaphore: true, updateAccount, autoFixValidationErrorsCount, token, priority, sendToServer, wait4Send, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 394);
				if (percentCompleted != null)
				{
					invokeToken?.Cancel();
					invokeToken = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken.Token;
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsListKol.Count(), current, kol.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 408);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 409);
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<WebKol> items, CancellationToken token)
	{
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "KolBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 420);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 421), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 421);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IKol.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 423);
							goto end_IL_0104;
						}
						result = ret;
					}
					goto end_IL_006d;
					end_IL_0104:;
				}
				goto end_IL_005c;
				end_IL_006d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 425);
				goto end_IL_005c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 428);
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 429);
				goto end_IL_005c;
			}
			return result;
			end_IL_005c:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 433), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 433);
			if (ret.HasError && items.Count() == 1)
			{
				ret = await (await KolMapper.Instance.MapAsync(contextGuid, items.Single(), null, "", token)).SaveAsync(EnSystem.SignalR, awaitOnSemaphore: false, updateAccount: true, 10, token, Priority.High, sendToServer: false, await4send: false, 10);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebKol> list2 = new List<WebKol>();
				WebKol[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 448);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 461), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 461);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IKol.KeepRangeAsync(select, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 462);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 466);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 467);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 472), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 472);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName, short autoFixValidationErrorsCount, EnSystem system, bool isDeleteAction)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name.Trim()))
			{
				ret.AddError(system, "نام حساب کل معتبر نمی باشد".GetTranslate(), "GroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 482);
			}
			if (GroupGuid == Guid.Empty && autoFixValidationErrorsCount <= 0)
			{
				ret.AddError(system, "گروه حساب کل".GetTranslate() + "(" + $"{GroupGuid}".GetTranslate() + ")" + " معتبر نمی باشد".GetTranslate() + "\r\n" + $"{Guid}", "GroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 485);
			}
			if (system == EnSystem.UserInterface && isDeleteAction)
			{
				HesabGroupBusiness hesabGroup = await HesabGroupBusiness.GetAsync(ContextGuid, GroupGuid, token, tranName);
				int num = await CountChilderenNotZeroAsync(ContextGuid, Guid, token, tranName);
				if ((hesabGroup?.Guid ?? Guid.Empty) != GroupGuid)
				{
					ret.AddError(system, "گروه حساب".GetTranslate() + " (" + $"{GroupGuid}".GetTranslate() + ")" + " در جدول گروه حساب ها وجود ندارد.".GetTranslate(), "GroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 493);
				}
				if (Account != 0m)
				{
					ret.AddError("حساب انتخاب شده به علت داشتن موجودی قابل حذف نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 496);
				}
				if (num > 0)
				{
					ret.AddError(system, "حساب انتخاب شده به علت داشتن".GetTranslate() + $"{num}".GetTranslate() + "زیر حساب فعال  قابل حذف نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 499);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckCodeValidationAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 501);
			if (string.IsNullOrEmpty(Description))
			{
				Description = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 507);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 508);
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("KolCheckValidation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 513);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckCodeValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			HesabGroupBusiness hesabGroup = await HesabGroupBusiness.GetAsync(ContextGuid, GroupGuid, token, tranName);
			if (Code <= 0)
			{
				ret.AddError(system, "کد حساب کل معتبر نمی باشد".GetTranslate(), "Code", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 525);
			}
			KolBusiness kolBusiness = await GetAsync(ContextGuid, Code, token, tranName);
			if (kolBusiness != null && kolBusiness.Guid != Guid)
			{
				if (system == EnSystem.UserInterface)
				{
					ret.AddError(system, "کد حساب کل".GetTranslate() + "(" + (Name ?? "").GetTranslate() + $"- {Code}" + ")" + " تکراری می باشد".GetTranslate() + "\r\n" + "کد تخصیص داده شده جاری مربوط به حساب کلی با نام ذیل میباشد".GetTranslate() + ":\r\n" + kolBusiness.Name + $"CurrentKolGuid:{Guid}\r\n" + $"ExistsKolGuid:{kolBusiness.Guid}", "Code", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 531);
				}
				else
				{
					Code = await NextCodeInGroupAsync(ContextGuid, GroupGuid, token, tranName);
				}
			}
			short kolLen = setting.Sanads.KolLen;
			if (kolLen != Code.ToString().Length && system == EnSystem.UserInterface)
			{
				ret.AddError(system, "کد حساب کل".GetTranslate() + " (" + (Name ?? "").GetTranslate() + " " + $" - {Code}" + ")" + " و طول آن".GetTranslate() + $"{Code.ToString().Length}" + "می باشد که با تنظیمات کدینگ".GetTranslate() + $" {kolLen}".GetTranslate() + "همخوانی ندارد".GetTranslate(), "Code", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 541);
			}
			if (system == EnSystem.UserInterface && !Code.ToString().StartsWith(hesabGroup.Code.ToString()))
			{
				ret.AddError("کد حساب کل".GetTranslate() + "(" + (Name ?? "").GetTranslate() + ")-(" + $"{Code}".GetTranslate() + ")" + " با گروه حساب".GetTranslate() + "(" + $"{hesabGroup.Code}" + "-" + (hesabGroup.Name ?? "").GetTranslate() + ")" + " تطابق ندارد.".GetTranslate(), "", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 544);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 548);
			ret.AddReturnedValue(ex, "", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 549);
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("KolBusiness Validation Has Error:" + ret.GetErrorMessage(), "", "CheckCodeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 554);
			}
		}
		return ret;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IKol;
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 567), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 567);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IKol.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 569);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 571);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 574);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 575);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 580), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 580);
			}
		}
		return ret;
	}

	Task<ReturnedSaveFuncInfo> ISyncFromClient2Server<KolBusiness>.ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 5)
	{
		return ResendNotSents(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<KolBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<KolBusiness>(null);
	}

	internal static async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Kol;
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال حساب های کل".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 608);
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 610);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 612);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IKol == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IKol is null");
				}
				line = 52;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 53;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (list == null || !list.Any())
				{
					return ret.AddInformation("حساب های کل سینک هستند. تعداد حساب های سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 622);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 623);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 630);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 632);
						}
						KolBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "حساب کل جاری".GetTranslate() + ":" + (obj?.Name ?? "").GetTranslate();
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Kol \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 642);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 648);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 653);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 657);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 659);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"CacheIsNullOrdisposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}\r\n" + "ContextxGuid:" + contextGuid, "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 662);
				ret.AddReturnedValue(ex, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 665);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs"))
		{
			_ = 3;
			try
			{
				if (!ClsCache.GetCache(ContextGuid).BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "KolBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 697);
				}
				List<WebKol> list = new List<WebKol>();
				List<WebKol> list2 = list;
				list2.Add(await KolMapper.Instance.MapAsync(ContextGuid, this, new WebKol(), "", token));
				List<Packet> paks = WebKol.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 699);
				}
				using KolWaiter waiter = new KolWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("KolBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 703);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 705);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 707);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 710);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 711);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 723), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 723);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IKol.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 725);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 729);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 730);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 735), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 735);
			}
		}
		return ret;
	}

	public static async Task<KolBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? groupHesabGuid = null, string description = "", Guid? guid = null, bool isSystem = false, string tranName = "")
	{
		KolBusiness kolBusiness;
		try
		{
			kolBusiness = new KolBusiness(contextGuid);
			kolBusiness.DateM = DateTime.Now;
			kolBusiness.Modified = DateTime.Now;
			kolBusiness.ChangeStatus = EntityChangeSt.New;
			kolBusiness.System = isSystem;
			if ((groupHesabGuid ?? Guid.Empty) != Guid.Empty)
			{
				kolBusiness.SetGroupGuid(system, groupHesabGuid.Value);
			}
			if (!string.IsNullOrEmpty(description))
			{
				kolBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				kolBusiness.Guid = guid.Value;
			}
			else
			{
				kolBusiness.Guid = Guid.NewGuid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\KolBusiness.cs", 763);
			throw;
		}
		return kolBusiness;
	}

	public static async Task<byte[]> GetGardeshHesabAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IKol.GetGardeshHesabAsync(guid, token, tranName);
	}
}
