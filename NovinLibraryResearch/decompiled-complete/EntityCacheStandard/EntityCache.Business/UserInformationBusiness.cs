using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClientHub;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Customer;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Host;
using Serilog;

namespace EntityCache.Business;

public class UserInformationBusiness : Serializable<UserInformationBusiness>, IUserInformation, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim loginResponseSemaphor = new SemaphoreSlim(1, 1);

	private Guid userAccGuid;

	private Guid? tafsilGuid;

	public BajeSettings BajeSettings { get; set; }

	public HesabyarSettings HesabyarSettings { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string UserRegisterDateSh => UserRegisterDateM.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public string UserExpireDateSh => UserExpireDateM.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public int CusId
	{
		get
		{
			if (CustomerInfo == null)
			{
				return 0;
			}
			return CustomerInfo.Id;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public int HesabTreeId => HesabInfo?.Id ?? 0;

	public string CacheApiURL { get; set; } = "";


	public short DepId { get; set; }

	public string PaymentURL { get; set; }

	public string UserName { get; set; }

	public string UserEmail { get; set; }

	public string UserMobile { get; set; }

	public int UserId { get; set; }

	public Guid UserAccGuid
	{
		get
		{
			return userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				userAccGuid = value;
			}
		}
	}

	public Guid UserDepGuid { get; set; }

	public DateTime UserRegisterDateM { get; set; }

	public DateTime UserExpireDateM { get; set; }

	public string RelativePath { get; set; }

	public string UserDescription { get; set; }

	public bool GpsEnable { get; set; }

	public string SiteUrl { get; set; }

	public SiteType SiteType { get; set; }

	public bool AndroidPrint { get; set; }

	public Sync SyncInfo { get; set; }

	public ENSource Source { get; set; }

	public LastSavedNumbers LastSaved { get; set; }

	public WebCus CustomerInfo { get; set; }

	public WebOrder Order { get; set; }

	public List<AndroidPrintStyle> AndroidPrintStyles { get; set; }

	public bool IsHaveTicketting { get; set; }

	public WebHesabTree HesabInfo { get; set; }

	public string DepartmentDescription { get; set; }

	public string MetaDescription { get; set; }

	public string MetaKeyword { get; set; }

	public string Script { get; set; }

	public string ScriptHeader { get; set; }

	public string ScriptFooter { get; set; }

	public string ScriptCounter { get; set; }

	public string ScriptEnamad { get; set; }

	public List<SocialNetwork> LstSocialNetwork { get; set; }

	public DateTime CurrentServerTime { get; set; }

	public byte[] HashedPassword { get; set; }

	public List<MediaLinks> LstMediaLinks { get; set; }

	public Guid? TafsilGuid
	{
		get
		{
			return tafsilGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				tafsilGuid = null;
			}
			else
			{
				tafsilGuid = value;
			}
		}
	}

	[Obsolete("", true)]
	public Guid UserAccountingGuid
	{
		get
		{
			return userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				userAccGuid = value;
			}
		}
	}

	public Guid DefaultCusGroup { get; set; }

	public List<UserAccCompact> UserAccCompacts { get; set; }

	public bool SyncsKolMoeinTafsil { get; set; }

	public static int DbCount { get; }

	public string PortalDomain { get; set; } = "https://PortalAcc.ir";


	[JsonIgnore]
	public string PrintDepAddWithTitle
	{
		get
		{
			string text = "";
			if (!string.IsNullOrEmpty(LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Address))
			{
				text += LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Address;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Insert(0, "آدرس: ");
			}
			return text;
		}
	}

	[JsonIgnore]
	public string PrintDepTelWithTitle
	{
		get
		{
			string text = "";
			if (!string.IsNullOrEmpty(LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Telephone))
			{
				text += LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Telephone;
			}
			if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Mobile))
			{
				text += LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Mobile;
			}
			if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Fax))
			{
				text += LocalSettingBusiness.GetSetting(ContextGuid).CompanyInfo.Fax;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Insert(0, "تلفن: ");
			}
			return text;
		}
	}

	public DateTime Version { get; set; } = DateTime.Now;


	public string ContextGuid { get; set; }

	public Guid CashierGuid
	{
		get
		{
			return userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				userAccGuid = value;
			}
		}
	}

	public List<DepartmentPaymentGatewayRelation> PaymentGatewayRelation { get; set; }

	public UserInformationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public UserInformationBusiness()
	{
	}

	public ReturnedSaveFuncInfo CheckValidation(CancellationToken token)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(UserName))
			{
				returnedSaveFuncInfo.AddError("نام کاربری وارد شده خالی است", "UserName", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 127);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 131);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 132);
		}
		return returnedSaveFuncInfo;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			int line = 2;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "UserInformationBusiness/SaveAsync", 60000, token))
					{
						line = 4;
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 150);
						}
						else
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 151), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 151);
							}
							line = 5;
							if (dis.Token.IsCancellationRequested)
							{
								result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 153);
							}
							else
							{
								transactionHasBeenCreated = true;
								ret.AddReturnedValue(CheckValidation(token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 155);
								if (dis.Token.IsCancellationRequested)
								{
									result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 156);
								}
								else
								{
									line = 6;
									if (!ret.HasError)
									{
										ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IUserInformation.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 159);
										line = 7;
										goto end_IL_014f;
									}
									result = ret;
								}
							}
						}
						goto end_IL_00b1;
						end_IL_014f:;
					}
					goto end_IL_00a0;
					end_IL_00b1:;
				}
				catch (OperationCanceledException)
				{
					goto end_IL_00a0;
				}
				catch (Exception ex2)
				{
					if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
					{
						Log.Warning(ex2, $"#Line_{line} cache is nulll or disposed!");
						throw;
					}
					WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}/Cahce is not disposed or null.", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 171);
					ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 172);
					goto end_IL_00a0;
				}
				return result;
				end_IL_00a0:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 177), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 177);
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAsync(EnSystem system, string username = "", string password = "", short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
				if (DefaultCusGroup == Guid.Empty)
				{
					DefaultCusGroup = (await CusGroupBusiness.GetAllAsync(ContextGuid, system, true, isLoadWithGuidEmpty: true, token))?.FirstOrDefault()?.Guid ?? Guid.Empty;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 192), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 192);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IUserInformation.UpdateAsync(this, username, password, tryCount, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 194);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 198);
				ret.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 199);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 204), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 204);
			}
		}
		return ret;
	}

	public static async Task<UserInformationBusiness> GetAsync(string contextGuid, string tranName = "")
	{
		int line = 0;
		try
		{
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CancellationToken token = new CancellationTokenSource(10000).Token;
			while (cache?.CacheRepository?.IUserInformation == null)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested)
				{
					throw new Exception("cache timedOut for initialization after 10 seconds!");
				}
			}
			if (cache == null)
			{
				throw new Exception("cache is null");
			}
			if (cache?.CacheRepository?.IUserInformation == null)
			{
				throw new Exception("User info is null");
			}
			line = 1;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IUserInformation.GetAsync(token, tranName);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line} " + "\r\n ContextGuid:" + contextGuid + " \r\n TranName:" + tranName + " " + $"\r\n IUserInformation Is Null? {ClsCache.GetCache(contextGuid)?.CacheRepository?.IUserInformation == null} " + $"\r\n ClsCache.GetCache Is Null? {ClsCache.GetCache(contextGuid) == null} " + $"\r\n CacheRepository.IsDispose:{(ClsCache.GetCache(contextGuid)?.CacheRepository?.IsDisposed).GetValueOrDefault(true)}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 229);
			return null;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> GetFromWebServiceAsync(string contextGuid, List<ClientConfig> configs, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetFromWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
			int line = 0;
			try
			{
				Stopwatch.StartNew();
				WebServiceRequest<LoginExtention> item = new WebServiceRequest<LoginExtention>
				{
					ConnectionData = configs[0].ToLoginExtention()
				};
				line = 1;
				LoginResponseEventArgs loginResponse = new LoginResponseEventArgs();
				ReturnedSaveFuncInfoWithValue<WebResponse<LoginResponseEventArgs>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<LoginResponseEventArgs, WebServiceRequest<LoginExtention>>(item, "https://novinrank.ir/Webservice/User/NewLogin", null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetFromWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 251);
				ret.throwExceptionIfError();
				line = 2;
				if (returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
				{
					line = 3;
					loginResponse = returnedSaveFuncInfoWithValue.value.Data;
					line = 4;
					ClsCache cache = ClsCache.GetCache(contextGuid);
					bool updateAccount = cache == null || cache.Source != ENSource.Accounting;
					line = 5;
					if ((ClsCache.GetCache(contextGuid)?.BufferConfigs?.Count()).GetValueOrDefault() != 0 && loginResponse != null && loginResponse.Status == EnLoginResult.Success && loginResponse.UserInfo != null)
					{
						Task.Run(() => ProcessLoginResponse(contextGuid, loginResponse, token, updateAccount, 10));
					}
				}
				line = 6;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}", "GetFromWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 268);
				ret.AddReturnedValue(ex, "", "GetFromWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 269);
			}
			return ret;
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> GetFromWebServiceAsync(string contextGuid, ClientConfig config, CancellationToken token = default(CancellationToken))
	{
		return GetFromWebServiceAsync(contextGuid, new List<ClientConfig> { config }, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<UserInformationBusiness>> ProcessLoginResponse(string contextGuid, LoginResponseEventArgs e, CancellationToken token, bool updateAccount, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<UserInformationBusiness> ret = new ReturnedSaveFuncInfoWithValue<UserInformationBusiness>();
		ClsCache cache = ClsCache.GetCache(contextGuid);
		if (cache == null)
		{
			return ret.AddError(EnSystem.None, "you must initialize cache first", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 279);
		}
		if ((e?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
		{
			return ret.AddError(EnSystem.None, $"Login result is :{e?.Status ?? EnLoginResult.Unknown}", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 280);
		}
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = "ProccessLoginResponse_" + Guid.NewGuid();
		}
		bool transactionHasBeenCreated = false;
		int line = 0;
		UserInformationBusiness user = new UserInformationBusiness(contextGuid);
		try
		{
			_ = 12;
			ReturnedSaveFuncInfoWithValue<UserInformationBusiness> result;
			try
			{
				ENSource currentCacheSource = cache.Source;
				line = 1011;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "UserInformationBusiness/ProccessLoginResponse", 60000, token))
				{
					line = 1012;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 292);
					}
					else
					{
						line = 1013;
						if (autoTran)
						{
							ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await cache.GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 294), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 294);
						}
						transactionHasBeenCreated = true;
						line = 100;
						await UserInformationMapper.Instance.MapAsync(contextGuid, e.UserInfo, user, "", token);
						line = 101;
						if (e.Status != EnLoginResult.Success || e.UserInfo == null)
						{
							goto end_IL_0215;
						}
						line = 1000;
						if (cache?.IsDisposed ?? true)
						{
							result = ret.AddError(EnSystem.None, "cache is null or disposed!", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 302);
						}
						else
						{
							if (currentCacheSource == ENSource.Accounting)
							{
								goto IL_0931;
							}
							line = 103;
							if (e.UserInfo.GlobalSettings != null)
							{
								ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await LocalSettingBusiness.SetSettingAsync(contextGuid, e.UserInfo.GlobalSettings, send2Server: true, token), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 307);
							}
							if (e.UserInfo.AbonmanSettings != null)
							{
								ActivationAbonmanSettings activationAbonmanSettings = await LocalSettingBusiness.GetAbonmanSettingsAsync(contextGuid);
								if (activationAbonmanSettings != null && activationAbonmanSettings.Version < e.UserInfo.AbonmanSettings.Version)
								{
									ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
									returnedSaveFuncInfoWithValue.AddReturnedValue(await LocalSettingBusiness.SetAbonmanSettingsAsync(contextGuid, e.UserInfo.AbonmanSettings), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 315);
								}
							}
							line = 104;
							if (e.UserInfo.UserAcc == null)
							{
								goto IL_0929;
							}
							CashierBusiness cashier = null;
							CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
							if (cache.Configs.Contains(EnCacheConfig.Accounting) && cashierBusiness == null)
							{
								cashier = ((!(e.UserInfo.CashierGuid == Guid.Empty)) ? (await CashierBusiness.GetAsync(contextGuid, e.UserInfo.CashierGuid, token)) : (await CashierBusiness.GetAsync(contextGuid, e.UserInfo.UserAccGuid, token)));
							}
							line = 107;
							if (cashier != null)
							{
								await CashierBusiness.SetCurrentCashierAsync(contextGuid, cashier.Guid, token);
							}
							line = 105;
							if (!dis.Token.IsCancellationRequested)
							{
								goto IL_0929;
							}
							result = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 336);
						}
					}
					goto end_IL_0138;
					IL_0931:
					line = 120;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 342);
					}
					else
					{
						ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await user.UpdateAsync(EnSystem.UserInterface, e?.UserInfo?.CustomerInfo?.User ?? "", e?.UserInfo?.CustomerInfo?.Password ?? "", tryCount, token, tranName), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 343);
						if (e.UserInfo.CustomerInfo == null || currentCacheSource == ENSource.Accounting)
						{
							goto end_IL_0215;
						}
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 346);
						}
						else
						{
							line = 2;
							CusBusiness obj = await CustomerMapper.Instance.MapAsync(contextGuid, e.UserInfo.CustomerInfo, new CusBusiness(contextGuid), "", token);
							line = 3;
							bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate = currentCacheSource != ENSource.Accounting;
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await obj.SaveAsync(EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, updateAccount, 10, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, awaitForSend: false, null, 10, "", null, null, null, token), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 351);
							if (!dis.Token.IsCancellationRequested)
							{
								line = 4;
								goto end_IL_0215;
							}
							result = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 352);
						}
					}
					goto end_IL_0138;
					IL_0929:
					line = 108;
					goto IL_0931;
					end_IL_0215:;
				}
				goto end_IL_0122;
				end_IL_0138:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{cache?.DatabaseManager == null}, e.UserInfo.UserAccountingGuid={e.UserInfo.UserAccGuid}", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 359);
				ret.AddReturnedValue(ex, "", "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 360);
				goto end_IL_0122;
			}
			return result;
			end_IL_0122:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 364), ReturnedState.None, "ProcessLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 364);
			}
			cache?.SendData(LoginExtention.GenerateOnLoginResponseProcessedPacket(), Priority.High);
		}
		ret.value = user;
		return ret;
	}

	public static async Task<Tuple<EnLoginResult, UserInformationBusiness>> GetAsync(string contextGuid, string username, string password, CancellationToken token, int watingSeconds = 60, string tranName = "")
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			_ = 3;
			try
			{
				ReturnedSaveFuncInfoWithValue<ClientHub.Buffer> temp = await ClsCache.GetCache(contextGuid).GetCustomerBuffer(username, password, token, checked(watingSeconds * 1000));
				await temp.value.GetStateAsync();
				if (temp.value.LastLoginState.Status == EnLoginResult.Success)
				{
					UserInformationBusiness item = await UserInformationMapper.Instance.MapAsync(contextGuid, temp.value.LastLoginState.UserInfo, new UserInformationBusiness(contextGuid), "", token);
					return new Tuple<EnLoginResult, UserInformationBusiness>(temp.value.LastLoginState.Status, item);
				}
				UserInformationBusiness userInformationBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.IUserInformation.GetByCredentialAsync(username, password, token, tranName);
				if (userInformationBusiness != null && userInformationBusiness.TafsilGuid.HasValue && userInformationBusiness.TafsilGuid != Guid.Empty)
				{
					return new Tuple<EnLoginResult, UserInformationBusiness>(EnLoginResult.Success, userInformationBusiness);
				}
				return new Tuple<EnLoginResult, UserInformationBusiness>(EnLoginResult.Unknown, userInformationBusiness);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 393);
				throw;
			}
		}
	}

	public static Task<IEnumerable<UserInformationBusiness>> GetAllAsync(string contextGuid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUserInformation.GetAllAsync(token, tranName);
	}

	public bool IsTruePassword(string password)
	{
		using (new JaegerMonitor("IsTruePassword", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			try
			{
				byte[] array = password.CalcHash();
				if (array == null || HashedPassword == null || array.Length != HashedPassword.Length)
				{
					return false;
				}
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					if (array[i] != HashedPassword[i])
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "IsTruePassword", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 413);
				throw;
			}
		}
	}

	internal List<AndroidPrintStyle> GetAndroidPrintStyles(PrintType printType)
	{
		using (new JaegerMonitor("GetAndroidPrintStyles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			List<AndroidPrintStyle> list = new List<AndroidPrintStyle>();
			try
			{
				List<AndroidPrintStyle> list2 = AndroidPrintStyles?.Where((AndroidPrintStyle p) => p != null && !string.IsNullOrEmpty(p.Html) && p.PrintType == printType)?.ToList();
				if (list2 != null && list2.Count > 0)
				{
					foreach (AndroidPrintStyle item in list2)
					{
						list.Add(new AndroidPrintStyle
						{
							Delay = item.Delay,
							Name = item.Name,
							PrintType = item.PrintType,
							ConnectionValue = item.ConnectionValue,
							ConnectoinType = item.ConnectoinType,
							Html = item.Html,
							PrintFormat = item.PrintFormat
						});
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAndroidPrintStyles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 440);
			}
			return list;
		}
	}

	public void CreateNewOrder()
	{
		using (new JaegerMonitor("CreateNewOrder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			try
			{
				Order = new WebOrder
				{
					ListKala = new List<WebOrderDetail>(),
					Date = DateTime.Now,
					FactorType = Document.PishFactor,
					IsTemp = false
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreateNewOrder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 456);
			}
		}
	}

	public void FillHash(string Password)
	{
		using (new JaegerMonitor("FillHash", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs"))
		{
			try
			{
				HashedPassword = Password.CalcHash();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "Pass:" + Password, "FillHash", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\UserInformationBusiness.cs", 462);
			}
		}
	}
}
