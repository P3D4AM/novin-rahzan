using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Crm;
using PacketParser.Connection.Login;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Issabel;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Accounting.UserSettings;
using Serilog;

namespace EntityCache.Business.Accounting;

public class CashierBusiness : BaseBusiness<CashierBusiness>, ICashier, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<CashierBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static ConcurrentDictionary<string, Guid> _dic = new ConcurrentDictionary<string, Guid>();

	private UserSettings _settings;

	private Guid? cusGuid;

	private Guid? depotGuid;

	private Guid? sandoghGuid;

	private Guid? visitorGuid;

	private Guid? bankGuid1;

	private Guid? bankGuid2;

	private Guid? bankGuid3;

	private Guid? bankGuid4;

	private AccessLevel _accessLevel;

	private UserAccBusiness _user;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid UserGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public string UserName { get; set; }

	public byte[]? HashedPassword { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; } = true;


	public List<CashierTime> CashierTime { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public UserSettings Settings => _settings;

	public Guid? DepotGuid
	{
		get
		{
			return depotGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				depotGuid = null;
			}
			else
			{
				depotGuid = value;
			}
		}
	}

	public Guid? SandoghGuid
	{
		get
		{
			return sandoghGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				sandoghGuid = null;
			}
			else
			{
				sandoghGuid = value;
			}
		}
	}

	public Guid? VisitorGuid
	{
		get
		{
			return visitorGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				visitorGuid = null;
			}
			else
			{
				visitorGuid = value;
			}
		}
	}

	public Guid? BankGuid1
	{
		get
		{
			return bankGuid1;
		}
		set
		{
			bankGuid1 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? BankGuid2
	{
		get
		{
			return bankGuid2;
		}
		set
		{
			bankGuid2 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? BankGuid3
	{
		get
		{
			return bankGuid3;
		}
		set
		{
			bankGuid3 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? BankGuid4
	{
		get
		{
			return bankGuid4;
		}
		set
		{
			bankGuid4 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	[Obsolete("Use Method to Get or Set", true)]
	public Guid? CusGuid { get; set; }

	public Guid? DefaultCusGroup { get; set; }

	[DefaultValue(true)]
	public bool AllVisitorAccess { get; set; } = true;


	public bool AllSandoghsAccess { get; set; }

	public bool AllDepotsAccess { get; set; }

	public bool AllBanksAccess { get; set; }

	public short LoginAccess { get; set; } = 7;


	public bool IsAndroid => HasLevel(UserLevel.Android);

	public bool IsAccounting => HasLevel(UserLevel.Hesabdari);

	public bool IsBaje => HasLevel(UserLevel.Baje);

	public Guid? ReceptionSmsCustomerGuid { get; set; }

	public Guid? PardakhtSmsCustomerGuid { get; set; }

	public Guid FareMoeinGuid { get; set; }

	public Guid? FareTafsilGuid { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public void SetSetting(string settings)
	{
		try
		{
			_settings = settings.FromJson<UserSettings>("SetSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 59);
			if (string.IsNullOrEmpty(settings))
			{
				throw new NullReferenceException("setting value is null or empty!!");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 63);
		}
	}

	public void SetSetting(UserSettings settings)
	{
		try
		{
			if (settings == null)
			{
				throw new NullReferenceException("value is null!!");
			}
			if (_settings != settings)
			{
				_settings = Json.ToStringJson(settings).FromJson<UserSettings>("SetSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 71);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 73);
		}
	}

	public async Task<Guid?> GetCusGuidAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return cusGuid;
			}
			if (!cusGuid.HasValue || cusGuid == Guid.Empty)
			{
				AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				if (!(accGlobalSettings.Customers.CustomerMotefaregheList?.Any() ?? false))
				{
					return cusGuid;
				}
				Guid tempCusGuid = accGlobalSettings.Customers.CustomerMotefaregheList.FirstOrDefault();
				if (await TafsilBusiness.GetAsync(ContextGuid, system, tempCusGuid, "", token, "GetCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs") != null)
				{
					cusGuid = tempCusGuid;
				}
			}
			return cusGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 99);
		}
		return null;
	}

	public void SetCusGuid(Guid? value)
	{
		cusGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
	}

	public Guid? GetCusGuid()
	{
		return cusGuid;
	}

	public CashierBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<AccessLevel> GetUserAccessAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_accessLevel == null)
			{
				_accessLevel = (await GetUserAsync(token, tranName))?.UserAccess ?? new AccessLevel();
			}
			return _accessLevel;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetUserAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 136);
			throw;
		}
	}

	public static async Task<bool> SetCurrentCashierAsync(string contextGuid, Guid cashierGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ClsCache.GetCache(contextGuid).SetCashierGuid(cashierGuid);
			CashierBusiness obj = await GetAsync(contextGuid, cashierGuid, token, tranName);
			_dic.TryRemove(contextGuid, out var _);
			if (obj == null)
			{
				WebErrorLog.Instance.StartLog($"Cashier is Null {cashierGuid}", "", "SetCurrentCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 150);
				return false;
			}
			_dic.TryAdd(contextGuid, cashierGuid);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCurrentCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 158);
			return false;
		}
	}

	public static Guid GetCurrentCashierGuid(string contextGuid)
	{
		int num = 0;
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new ArgumentException("contextGuid is null or empty!");
		}
		try
		{
			num = 1;
			if (_dic.TryGetValue(contextGuid, out var value))
			{
				return value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//ContextGuid:{contextGuid}", "GetCurrentCashierGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 172);
		}
		return Guid.Empty;
	}

	public static async Task<CashierBusiness> GetCurrentCashierAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		CashierBusiness res = null;
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				return null;
			}
			line = 1;
			if (_dic.TryGetValue(contextGuid, out var value))
			{
				res = await GetAsync(contextGuid, value, token, tranName);
			}
			if (res == null)
			{
				line = 2;
				UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(contextGuid, tranName);
				if (userInformationBusiness != null && userInformationBusiness.CashierGuid != Guid.Empty && ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
				{
					await SetCurrentCashierAsync(contextGuid, userInformationBusiness.CashierGuid, token, tranName);
				}
				else
				{
					Guid guid = userInformationBusiness?.UserAccGuid ?? Guid.Empty;
					line = 3;
					if (guid == Guid.Empty)
					{
						return null;
					}
					res = await GetAsync(contextGuid, guid, token, tranName);
					line = 4;
					if (res != null)
					{
						await SetCurrentCashierAsync(contextGuid, res.Guid, token, tranName);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCurrentCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 205);
			throw;
		}
		return res;
	}

	public async Task<UserAccBusiness> GetUserAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_user == null)
			{
				_user = await UserAccBusiness.GetAsync(ContextGuid, UserGuid, token, tranName);
			}
			return _user;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 219);
			throw;
		}
	}

	private bool HasLevel(UserLevel access)
	{
		try
		{
			return ((uint)access & (uint)LoginAccess) == (uint)access;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 231);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, short tryFixCount, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					ret.AddError(system, "نام کاربر پر نشده است.".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 240);
				}
				_ = UserGuid;
				if (UserGuid == Guid.Empty)
				{
					ret.AddError("گروه کاربری مشخص نشده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 241);
				}
				if (string.IsNullOrEmpty(UserName))
				{
					ret.AddError(system, "نام کاربری وارد شده".GetTranslate() + ("(" + UserName + ") ").GetTranslate() + "معتبر نمیباشد.".GetTranslate() + " \r\n" + $"CashierGuid:{Guid} \r\n" + "CashierName:" + Name, "UserName", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 243);
				}
				if (tryFixCount > 0 && system != EnSystem.UserInterface)
				{
					CashierBusiness cashierBusiness = await GetByNameAsync(ContextGuid, Name, token, tranName);
					if (cashierBusiness != null && cashierBusiness.Guid != Guid)
					{
						Name += "_";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						CashierBusiness cashierBusiness2 = this;
						short num = (short)(tryFixCount - 1);
						tryFixCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await cashierBusiness2.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 252);
						return ret;
					}
					cashierBusiness = await GetByUsernameAsync(ContextGuid, UserName, token, tranName);
					if (cashierBusiness != null && cashierBusiness.Guid != Guid)
					{
						UserName += "_";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						CashierBusiness cashierBusiness3 = this;
						short num = (short)(tryFixCount - 1);
						tryFixCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await cashierBusiness3.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 260);
						return ret;
					}
				}
				else
				{
					if (!(await ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER.CheckUserNameAsync(UserName, Guid, token, tranName)))
					{
						ret.AddError(system, "نام کاربری".GetTranslate() + "(" + UserName + ")" + " قبلا به کاربر دیگری تخصیص داده شده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 268);
					}
					CashierBusiness cashierBusiness4 = await ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER.GetByNameAsync(Name, token, tranName);
					if (cashierBusiness4 != null && cashierBusiness4.Guid != Guid)
					{
						ret.AddError(system, "قبلا فردی با نام مشابه".GetTranslate() + " (" + Name + ")" + " در سیستم ثبت شده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 271);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 276);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 277);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckIsNumberExistAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (Settings.NumberList != null)
			{
				foreach (IsabelleNumber item in Settings.NumberList.ToList())
				{
					CashierBusiness cashierBusiness = await GetByInternalNumberAsync(ContextGuid, item.Number, Guid, token, tranName);
					if (cashierBusiness != null && cashierBusiness.Guid != Guid)
					{
						return res.AddError("شماره وارد شده به کاربر".GetTranslate() + "  " + cashierBusiness.Name + "  " + "تخصیص داده شده است".GetTranslate(), "", "CheckIsNumberExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 291);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckIsNumberExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 297);
			res.AddReturnedValue(ex, "", "CheckIsNumberExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 298);
		}
		return res;
	}

	public static Task<CashierBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			AggregateSqlRepository cacheRepository = ClsCache.GetCache(contextGuid).CacheRepository;
			if (cacheRepository?.ICASHIER != null)
			{
				return cacheRepository.ICASHIER.GetAsync(guid, token, tranName);
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ICASHIER == null)
			{
				Log.Warning(ex2, "ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER is null!!");
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex2, "ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER is not null.", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 316);
			}
		}
		return Task.FromResult<CashierBusiness>(null);
	}

	public static Task<CashierBusiness> GetAsync(string contextGuid, string uName, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetAsync(uName?.FixString(), token, tranName);
	}

	public static Task<CashierBusiness> GetByNameAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetByNameAsync(name, token, tranName);
	}

	public static Task<string> GetDebugContextesAsync()
	{
		string text = "";
		try
		{
			foreach (string key in _dic.Keys)
			{
				if (_dic.TryGetValue(key, out var value))
				{
					text += $"ContextGuid({key})-CashierGuid({value})\r\n";
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDebugContextesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 335);
			text = text + "\r\nException:" + ex.Message;
		}
		return Task.FromResult(text);
	}

	public static Task<CashierBusiness> GetByUsernameAsync(string contextGuid, string username, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetByUsernameAsync(username, token, tranName);
	}

	public static async Task<CashierBusiness> GetCashierForLoginAsync(string contextGuid, LoginCallCenterToAccounting login, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (login == null)
		{
			throw new ArgumentNullException();
		}
		try
		{
			CashierBusiness cashier = login.TypeLogin switch
			{
				TypeLogin.LoginByUserName => await GetByUsernameAsync(contextGuid, login.UserName, token, tranName), 
				TypeLogin.LoginByExtantionNumber => await GetByInternalNumberAsync(contextGuid, login.ExtentionNumber, null, token, tranName), 
				_ => throw new NotImplementedException(), 
			};
			if (cashier != null)
			{
				if (IsHashedSame(CalcHashedPassword(login.Password, cashier.Guid), cashier.HashedPassword))
				{
					await SetCurrentCashierAsync(contextGuid, cashier.Guid);
					return cashier;
				}
				cashier = null;
			}
			else
			{
				cashier = null;
			}
			return cashier;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierForLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 373);
			throw;
		}
	}

	public static Task<IEnumerable<CashierBusiness>> GetAllAsync(string contextGuid, Guid userGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetAllAsync(userGuid, status, token, tranName);
	}

	public static Task<IEnumerable<CashierBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetAllAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool sendToServer, UserSettings setting, short tryFixCount, EnSystem system, Priority priority = Priority.Low, bool awaitOnSend = false, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
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
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierBusiness/SaveAsync", 60000, token))
				{
					ICashierRepository cacheRepositoryIcashier;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 393);
					}
					else
					{
						line = 410;
						if (base.IsDisposed)
						{
							throw new ObjectDisposedException("cashierBusiness is disposed!!");
						}
						line = 411;
						if (cache == null)
						{
							throw new NullReferenceException("cache is null!!");
						}
						line = 412;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 399), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 399);
						}
						line = 51;
						transactionHasBeenCreated = true;
						cacheRepositoryIcashier = ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER;
						if (cacheRepositoryIcashier == null)
						{
							goto end_IL_01cc;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, tryFixCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 405);
						if (system != EnSystem.UserInterface)
						{
							ret.throwExceptionIfError();
							goto IL_0439;
						}
						if (!ret.HasError)
						{
							goto IL_0439;
						}
						result = ret;
					}
					goto end_IL_00ed;
					IL_0439:
					if (system == EnSystem.SignalR)
					{
						if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
						{
							throw new NotImplementedException("چرا باید برای حسابداری از سرور این تابع صدا زده بشه!".GetTranslate());
						}
						setting = (await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName))?.Settings ?? setting ?? new UserSettings();
						setting.OtherSettings.NPACCEnable = setting.OtherSettings.NPACCEnable || system == EnSystem.SignalR;
						SetSetting(setting);
					}
					if (Settings == null)
					{
						throw new NullReferenceException("setting is null!!");
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cacheRepositoryIcashier.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 418);
					end_IL_01cc:;
				}
				goto end_IL_00d9;
				end_IL_00ed:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_:{line} \r\n System_:{system}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 423);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 424);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 429), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 429);
			}
			if (sendToServer && !ret.HasError)
			{
				if (awaitOnSend)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 432);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (ret.HasError)
			{
				ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER.ResetCache(default(CancellationToken), "");
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
				WebCashier webEntity = await CashierMapper.Instance.MapAsync(ContextGuid, this, new WebCashier());
				CashierBusiness cashierBusiness = await GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.SaveUser;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره کاربر".GetTranslate() + $"\r\n({UserName}-{Name})-{Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 469);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> select, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 479), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 479);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.KeepRangeAsync(select, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 481);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 485);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 486);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 491), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 491);
			}
		}
		return ret;
	}

	public static Task<bool> CheckUserName(string contextGuid, string uName, Guid _guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.CheckUserNameAsync(uName, _guid, token, tranName);
	}

	public static bool IsHashedSame(IEnumerable<byte> item1, IEnumerable<byte> item2)
	{
		try
		{
			if (item1 == item2 && item1 == null)
			{
				return true;
			}
			if (item1 == null || item2 == null)
			{
				return false;
			}
			if (item1.Count() != item2.Count())
			{
				return false;
			}
			List<byte> list = item1.ToList();
			List<byte> list2 = item2.ToList();
			for (int i = 0; i < item1.Count(); i = checked(i + 1))
			{
				if (list[i] != list2[i])
				{
					return false;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsHashedSame", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 513);
			throw;
		}
	}

	public bool CheckInputPassword(string password)
	{
		try
		{
			byte[] inputHash = CalcHashedPassword(password, Guid);
			return CheckHash(inputHash);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckInputPassword", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 526);
			throw;
		}
	}

	public bool CheckHash(byte[] inputHash)
	{
		if (inputHash == null || HashedPassword == null)
		{
			return false;
		}
		if (inputHash.Length != HashedPassword.Length)
		{
			return false;
		}
		for (int i = 0; i < inputHash.Length; i = checked(i + 1))
		{
			if (inputHash[i] != HashedPassword[i])
			{
				return false;
			}
		}
		return true;
	}

	public static byte[] CalcHashedPassword(string password, Guid guid)
	{
		try
		{
			return password.CalcHashSafe(guid.ToByteArray());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcHashedPassword", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 554);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<WebCashier> userAccCashierList, CancellationToken token, EnSystem system, bool awaitOnSemaphore = true, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierBusiness/SaveRangeAsync", 60000, token);
				if (dis.Token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 567);
					goto end_IL_0114;
				}
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 568), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 568);
				}
				transactionHasBeenCreated = true;
				foreach (WebCashier userAccCashier in userAccCashierList)
				{
					if (userAccCashier == null)
					{
						continue;
					}
					if (userAccCashier.HashedPassword == null)
					{
						throw new Exception("cashier hashedPassword is null!!\r\nJsonData:" + Json.ToStringJson(userAccCashier, indented: true));
					}
					if (!dis.Token.IsCancellationRequested)
					{
						CashierBusiness obj = await CashierMapper.Instance.MapAsync(contextGuid, userAccCashier, new CashierBusiness(contextGuid), "", token);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(sendToServer: false, null, 10, system, Priority.Low, awaitOnSend: false, awaitOnSemaphore: false, 10, "", token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 579);
						continue;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 577);
					goto end_IL_0114;
				}
				goto end_IL_0065;
				end_IL_0114:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 584);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 585);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 590), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 590);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CashierBusiness> userAccCashierList, bool sendToServer, EnSystem system, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			int current = 0;
			foreach (CashierBusiness item in userAccCashierList)
			{
				if (item != null)
				{
					current = checked(current + 1);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 605);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(sendToServer, null, 10, system, priority, awaitOnSend: false, awaitOnSemaphore: true, 10, "", token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 606);
					if (percentCompleted != null)
					{
						invokeToken2?.Cancel();
						invokeToken2 = new CancellationTokenSource(60000);
						CancellationToken token2 = invokeToken2.Token;
						percentCompleted(new PercenteCompletedEventArgs(userAccCashierList.Count(), current, item.Name, token2));
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 619);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 620);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(userAccCashierList?.Count() ?? 0, userAccCashierList?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 646), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 646);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICASHIER.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 648);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 651);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 654);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 655);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 660), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 660);
			}
		}
		return ret;
	}

	public static Task<CashierBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CashierBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Cashier;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال کاربران".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Others;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 682);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 684);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 686);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ICashier is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ICASHIER.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 693);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("کاربران سینک هستند تعداد کاربران سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 694);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 695);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 701);
						}
						current = checked(current + 1);
						CashierBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "کاربر جاری".GetTranslate() + ":" + obj?.Name;
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Cashier \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 712);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 718);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 723);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 727);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 728);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 731);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 734);
				ret.AddReturnedValue(ex, "", "ResendNotSents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 735);
			}
			finally
			{
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

	public static Task<CashierBusiness> GenerateNewWithDefaultsAsync(string contextGuid, Guid? guid = null)
	{
		try
		{
			return Task.FromResult(new CashierBusiness(contextGuid)
			{
				_settings = new UserSettings(),
				ChangeStatus = EntityChangeSt.New,
				Guid = (((guid ?? Guid.Empty) != Guid.Empty) ? guid.Value : Guid.NewGuid())
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 770);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	internal static async Task<Guid> GetCashierGuidByIssabelExtentionNumberAsync(string contextGuid, int extentionNumber, Guid? exceptGuid, CancellationToken token = default(CancellationToken))
	{
		if (extentionNumber <= 0)
		{
			throw new ArgumentException("extentionNumber");
		}
		try
		{
			IEnumerable<CashierBusiness> enumerable = await GetAllAsync(contextGuid, true, token);
			if (enumerable == null || !enumerable.Any())
			{
				return Guid.Empty;
			}
			if ((exceptGuid ?? Guid.Empty) != Guid.Empty)
			{
				enumerable = enumerable.Where((CashierBusiness q) => q.Guid != exceptGuid.Value);
				if (enumerable == null || !enumerable.Any())
				{
					return Guid.Empty;
				}
			}
			foreach (CashierBusiness item in enumerable)
			{
				if (!(item?.Settings?.NumberList?.Any()).GetValueOrDefault())
				{
					continue;
				}
				foreach (IsabelleNumber number in item.Settings.NumberList)
				{
					if (number != null && number.Number == extentionNumber)
					{
						return item.Guid;
					}
				}
			}
			return Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierGuidByIssabelExtentionNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 798);
			throw;
		}
	}

	public static async Task<CashierBusiness> GetByInternalNumberAsync(string contextGuid, int number, Guid? exceptGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			Guid guid = await GetCashierGuidByIssabelExtentionNumberAsync(contextGuid, number, exceptGuid, token);
			if (guid == Guid.Empty)
			{
				return null;
			}
			return await GetAsync(contextGuid, guid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByInternalNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 813);
			throw;
		}
	}

	public static async Task<List<CashierNumber>> GetCashierNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			List<CashierNumber> cashierNumber = new List<CashierNumber>();
			IEnumerable<CashierBusiness> enumerable = await GetAllAsync(contextGuid, true, token, tranName);
			if (enumerable != null && !enumerable.Any())
			{
				return cashierNumber;
			}
			foreach (CashierBusiness item in enumerable)
			{
				CashierNumber cashierNumber2 = new CashierNumber();
				if (item?.Settings?.NumberList != null)
				{
					foreach (IsabelleNumber number in item.Settings.NumberList)
					{
						CashierNumber cashierNumber3 = cashierNumber2;
						if (cashierNumber3.InternalNumber == null)
						{
							cashierNumber3.InternalNumber = new List<int>();
						}
						cashierNumber2.UserName = item.UserName;
						cashierNumber2.Name = item.Name;
						cashierNumber2.InternalNumber.Add(number.Number);
					}
				}
				cashierNumber.Add(cashierNumber2);
			}
			return cashierNumber;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 841);
			throw;
		}
	}

	public static async Task<List<Guid>> GetAllTafsilGroupGuidsForTurnoverAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return null;
			}
			CashierBusiness cashierBusiness = await GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashierBusiness == null)
			{
				return null;
			}
			UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token, tranName);
			List<Guid> result = userAccBusiness?.UserAccess?.TafsilGroupGuid?.ToList();
			if (!(userAccBusiness?.UserAccess?.TafsilGroupGuid?.Any()).GetValueOrDefault() && (userAccBusiness?.UserAccess?.TafsilGroupJustListGuid?.Any()).GetValueOrDefault())
			{
				result = new List<Guid>();
			}
			if (!(userAccBusiness?.UserAccess?.TafsilGroupGuid?.Any()).GetValueOrDefault() && !(userAccBusiness?.UserAccess?.TafsilGroupJustListGuid?.Any()).GetValueOrDefault())
			{
				result = null;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllTafsilGroupGuidsForTurnoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashierBusiness.cs", 863);
			throw;
		}
	}
}
