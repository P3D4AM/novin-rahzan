using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository;

public class UserInfoWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private List<ClientConfig> configs;

	private Dictionary<string, TaskCompletionSource<UserInformationBusiness>> taskDictionary = new Dictionary<string, TaskCompletionSource<UserInformationBusiness>>();

	public bool IsDisposed { get; private set; }

	public UserInfoWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		Cache.Context.LoginExtention.OnLoginResponse += Instance_OnLoginResponse;
		Cache.Context.WebUserAcc.OnSync += WebUserAcc_OnSync;
		Cache.Context.WebUserAcc.OnReadResponse += WebUserAcc_OnReadResponse;
	}

	private Task WebUserAcc_OnReadResponse(object sender, ConnectionEventArgs<WebUserAcc> e)
	{
		return SaveUsersAsync(e, default(CancellationToken));
	}

	private async Task WebUserAcc_OnSync(object sender, ConnectionEventArgs<WebUserAcc> e)
	{
		CancellationToken cancellationToken = e.Connection.cancellationToken;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveUsersAsync(e, cancellationToken), ReturnedState.None, "WebUserAcc_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 42);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WebUserAcc_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 47);
			ret.AddReturnedValue(ex2, "", "WebUserAcc_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 48);
		}
		finally
		{
			if (!ret.HasError && !(e.Connection?.cancellationToken.IsCancellationRequested ?? true))
			{
				await Cache.SendData(Sync.GenerateSyncDone(EntityPart.User, e.PartitionId, ret), Priority.High);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveUsersAsync(ConnectionEventArgs<WebUserAcc> e, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Cache.Source == ENSource.Accounting)
			{
				throw new Exception("سرور نباید سینک کاربران را برای حسابداری ارسال کند!".GetTranslate());
			}
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return ret;
			}
			foreach (WebUserAcc userAcc in e.ConnectionData)
			{
				token.ThrowIfCancellationRequested();
				UserAccBusiness obj = await UserAccMapper.Instance.MapAsync(Cache.ContextGuid, userAcc, null, "", token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(sendToServer: false, EnSystem.SignalR, Priority.Low, awaitOnSemaphore: true, await4Send: false, 10, "", token, 0), ReturnedState.None, "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 69);
				if (!(userAcc.CashierList?.Any() ?? false))
				{
					continue;
				}
				foreach (WebCashier item in userAcc.CashierList ?? new List<WebCashier>())
				{
					token.ThrowIfCancellationRequested();
					if (userAcc.AccountingVersion < 2)
					{
						item.BankGuid1 = userAcc.BankGuid1;
						item.BankGuid2 = userAcc.BankGuid2;
						item.BankGuid3 = userAcc.BankGuid3;
						item.BankGuid4 = userAcc.BankGuid4;
						item.AllBanksAccess = userAcc.AllBanksAccess;
						item.SandoghGuid = userAcc.SandoghGuid;
						item.AllSandoghsAccess = userAcc.AllSandoghsAccess;
						item.DepotGuid = userAcc.DepotGuid;
						item.AllDepotsAccess = userAcc.AllDepotsAccess;
						item.AllVisitorAccess = userAcc.AllVisitorAccess;
						item.VisitorGuid = userAcc.VisitorGuid;
						item.CusGuid = userAcc.CusGuid;
					}
					CashierBusiness cashierBusiness = await CashierMapper.Instance.MapAsync(Cache.ContextGuid, item, null, "", token);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckCashierPreRequestedHasBeenSavedAsync(cashierBusiness, token), ReturnedState.None, "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 99);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cashierBusiness.SaveAsync(sendToServer: false, null, 10, EnSystem.SignalR, Priority.Low, awaitOnSend: false, awaitOnSemaphore: false, 10, "", token), ReturnedState.None, "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 100);
				}
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 104);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 107);
			ret.AddReturnedValue(ex2, "", "SaveUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 108);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckCashierPreRequestedHasBeenSavedAsync(CashierBusiness cashier, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			CashierBusiness cashierBusiness = cashier;
			ReturnedSaveFuncInfo returnedSaveFunc = ret;
			string translate = "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل شخص پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate();
			cashierBusiness.SetCusGuid(await CehckTafsilAsync(returnedSaveFunc, translate, await cashier.GetCusGuidAsync(EnSystem.SignalR), token));
			line = 1;
			cashierBusiness = cashier;
			cashierBusiness.BankGuid1 = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل بانک1 پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.BankGuid1, token);
			line = 2;
			cashierBusiness = cashier;
			cashierBusiness.BankGuid2 = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل بانک2 پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.BankGuid2, token);
			line = 3;
			cashierBusiness = cashier;
			cashierBusiness.BankGuid3 = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل بانک3 پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.BankGuid3, token);
			line = 4;
			cashierBusiness = cashier;
			cashierBusiness.BankGuid4 = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل بانک4 پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.BankGuid4, token);
			line = 5;
			cashierBusiness = cashier;
			cashierBusiness.VisitorGuid = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل ویزیتور پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.VisitorGuid, token);
			line = 6;
			cashierBusiness = cashier;
			cashierBusiness.SandoghGuid = await CehckTafsilAsync(ret, "هنوز حسابهای تفصیلی سینک نشده است به همین دلیل صندوق پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), cashier.SandoghGuid, token);
			line = 7;
			CusGroupBusiness cusGroupBusiness = await CusGroupBusiness.GetAsync(Cache.ContextGuid, cashier?.DefaultCusGroup ?? Guid.Empty);
			if (cashier.DefaultCusGroup.HasValue && cashier.DefaultCusGroup != Guid.Empty && cusGroupBusiness == null)
			{
				line = 20;
				cashier.DefaultCusGroup = null;
				line = 21;
				ret.AddError(EnSystem.SignalR, "هنوز گروه اشخاص سینک نشده است به همین دلیل گروه اشخاص پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), "", "CheckCashierPreRequestedHasBeenSavedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 142);
			}
			line = 24;
			DepotBusiness depotBusiness = await DepotBusiness.GetAsync(Cache.ContextGuid, cashier.DepotGuid ?? Guid.Empty);
			line = 25;
			if ((cashier.DepotGuid ?? Guid.Empty) != Guid.Empty && depotBusiness == null)
			{
				line = 26;
				cashier.DepotGuid = null;
				line = 27;
				ret.AddError(EnSystem.SignalR, "هنوز انبارها سینک نشده است به همین دلیل انبار پیش فرض کاربر رو حذف کردم و نباید سینک دان برای کاربران ارسال شود.".GetTranslate(), "", "CheckCashierPreRequestedHasBeenSavedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 154);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckCashierPreRequestedHasBeenSavedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 159);
			ret.AddReturnedValue(ex, "", "CheckCashierPreRequestedHasBeenSavedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 160);
		}
		return ret;
	}

	private async Task<Guid?> CehckTafsilAsync(ReturnedSaveFuncInfo returnedSaveFunc, string errorMessage, Guid? tafsilGuid, CancellationToken token)
	{
		if ((tafsilGuid ?? Guid.Empty) == Guid.Empty)
		{
			return null;
		}
		if (await TafsilBusiness.GetAsync(Cache.ContextGuid, EnSystem.SignalR, tafsilGuid.Value, "", token, "CehckTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs") == null)
		{
			returnedSaveFunc.AddError(EnSystem.SignalR, errorMessage, "", "CehckTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 171);
			return null;
		}
		return tafsilGuid;
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (Cache?.Context != null)
				{
					Cache.Context.LoginExtention.OnLoginResponse -= Instance_OnLoginResponse;
					Cache.Context.WebUserAcc.OnSync -= WebUserAcc_OnSync;
					Cache.Context.WebUserAcc.OnReadResponse -= WebUserAcc_OnReadResponse;
				}
				Cache = null;
				configs = null;
				taskDictionary = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 195);
		}
	}

	private async Task Instance_OnLoginResponse(object sender, LoginResponseEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (Cache == null || (e?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
				{
					return;
				}
				CancellationToken token = e.connection.cancellationToken;
				line = 1;
				bool updateAccount = Cache.Source != ENSource.Accounting;
				if (Cache.Configs.Any((EnCacheConfig x) => x == EnCacheConfig.Accounting))
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					await UserInformationBusiness.ProcessLoginResponse(e.connection.Context.Guid, e, token, updateAccount, 10);
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					line = 3;
					string key = e.CustomerUserName + e.CustomerPassword;
					line = 4;
					if (taskDictionary.ContainsKey(key))
					{
						TaskCompletionSource<UserInformationBusiness> taskCompletionSource = taskDictionary[key];
						taskCompletionSource.TrySetResult(await UserInformationMapper.Instance.MapAsync(e.Context.Guid, e.UserInfo, null, "", token));
					}
				}
				line = 50;
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					line = 6;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				if (Cache?.DatabaseManager == null || IsDisposed)
				{
					Log.Warning(ex2, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{Cache?.DatabaseManager == null}//isDisposed:{IsDisposed}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{Cache?.DatabaseManager == null}//isDisposed:{IsDisposed}", "Instance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 230);
				}
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<UserInformationBusiness>> GetAsync(string customerUsername, string customerPassword, CancellationToken token, WebContextHolder context, int expireAfter = 10000)
	{
		ReturnedSaveFuncInfoWithValue<UserInformationBusiness> ret = new ReturnedSaveFuncInfoWithValue<UserInformationBusiness>();
		try
		{
			string tranName = "";
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 247);
			}
			ret.value = await Cache.CacheRepository.IUserInformation.GetByCredentialAsync(customerUsername, customerPassword, token, tranName);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 251);
			}
			if (ret.value != null && ret.value.DepId != 0)
			{
				ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await Cache.CacheRepository.IUserInformation.GetByCredentialAsync(customerUsername, customerPassword, token, tranName);
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 256);
				}
				string key = customerUsername + customerPassword;
				if (taskDictionary.ContainsKey(key))
				{
					taskDictionary[key].TrySetCanceled();
					taskDictionary[key] = new TaskCompletionSource<UserInformationBusiness>();
				}
				else
				{
					taskDictionary.Add(key, new TaskCompletionSource<UserInformationBusiness>());
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 265);
				}
				List<ClientConfig> list = configs.CopyAll();
				foreach (ClientConfig item in list)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 269);
					}
					item.CustomerPassword = customerPassword;
					item.CustomerUsername = customerUsername;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 273);
				}
				ClientHub.Buffer buffer = new ClientHub.Buffer(Cache.Source, DI.Kernel, context, Cache.BufferConnectionType);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 275);
				}
				ClientHub.Buffer buffer2 = buffer;
				IEnumerable<ClientConfig> enumerable = list;
				CancellationToken? token2 = token;
				await buffer2.ConnectAsync(enumerable, token2, await LocalSettingBusiness.GetDbGuidAsync(context.Guid, token));
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 277);
				}
				Task cancel = Task.Delay(expireAfter, token);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 279);
				}
				Task task = await Task.WhenAny(new Task[2]
				{
					taskDictionary[key].Task,
					cancel
				});
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 281);
				}
				if (task == cancel)
				{
					ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await Cache.CacheRepository.IUserInformation.GetByCredentialAsync(customerUsername, customerPassword, token, tranName);
				}
				else
				{
					ReturnedSaveFuncInfoWithValue<UserInformationBusiness> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await taskDictionary[key].Task;
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 288);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 291);
			ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\UserInfoWebServiceRepository.cs", 292);
		}
		return ret;
	}
}
