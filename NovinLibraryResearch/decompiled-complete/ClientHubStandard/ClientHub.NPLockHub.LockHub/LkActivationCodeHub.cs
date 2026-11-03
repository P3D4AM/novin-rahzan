using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Lock;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.NPLockHub.LockHub;

public class LkActivationCodeHub : RetriableHubConnection, IActivationCodeSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public LkActivationCodeHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> ReturnActiveCodeAsync(string activationCode, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "ReturnActiveCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 26);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "ReturnActiveCodeAsync", (object)activationCode, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "ReturnActiveCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 29);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ReturnActiveCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 33);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ReturnActiveCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 36);
			res.AddReturnedValue(ex2, "", "ReturnActiveCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 37);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingSadeActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAccountingSadeActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 47);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAccountingSadeActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAccountingSadeActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 50);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAccountingSadeActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 54);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountingSadeActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 57);
			res.AddReturnedValue(ex2, "", "GetAccountingSadeActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 58);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingMotevasetActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAccountingMotevasetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 68);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAccountingMotevasetActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAccountingMotevasetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 71);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAccountingMotevasetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 75);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountingMotevasetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 78);
			res.AddReturnedValue(ex2, "", "GetAccountingMotevasetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 79);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingPishrafteActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAccountingPishrafteActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 89);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAccountingPishrafteActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAccountingPishrafteActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 92);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAccountingPishrafteActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 96);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountingPishrafteActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 99);
			res.AddReturnedValue(ex2, "", "GetAccountingPishrafteActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 100);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingJameActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAccountingJameActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 110);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAccountingJameActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAccountingJameActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 113);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAccountingJameActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 117);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountingJameActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 120);
			res.AddReturnedValue(ex2, "", "GetAccountingJameActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 121);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingSpecialActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAccountingSpecialActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 131);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAccountingSpecialActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAccountingSpecialActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 134);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAccountingSpecialActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 138);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountingSpecialActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 141);
			res.AddReturnedValue(ex2, "", "GetAccountingSpecialActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 142);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetTamdidActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetTamdidActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 152);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetTamdidActivationCodeAsync", (object)softwareSerial, (object)lkSerial, (object)accountingPack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetTamdidActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 155);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetTamdidActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 159);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetTamdidActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 162);
			res.AddReturnedValue(ex2, "", "GetTamdidActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 163);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetAttendanceActivationCodeAsync(short level, string softwareSerial, string lkSerial, HozoorPackage pack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetAttendanceActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 173);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetAttendanceActivationCodeAsync", (object)level, (object)softwareSerial, (object)lkSerial, (object)pack, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetAttendanceActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 176);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAttendanceActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 180);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAttendanceActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 183);
			res.AddReturnedValue(ex2, "", "GetAttendanceActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 184);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel1(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationAttendanceLevel1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 194);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationAttendanceLevel1", (object)softwareSerial, (object)lkSerial, (object)package, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationAttendanceLevel1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 197);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationAttendanceLevel1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 201);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationAttendanceLevel1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 204);
			res.AddReturnedValue(ex2, "", "GetActivationAttendanceLevel1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 205);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel2(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationAttendanceLevel2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 215);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationAttendanceLevel2", (object)softwareSerial, (object)lkSerial, (object)package, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationAttendanceLevel2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 218);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationAttendanceLevel2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 222);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationAttendanceLevel2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 225);
			res.AddReturnedValue(ex2, "", "GetActivationAttendanceLevel2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 226);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel3(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationAttendanceLevel3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 236);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationAttendanceLevel3", (object)softwareSerial, (object)lkSerial, (object)package, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationAttendanceLevel3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 239);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationAttendanceLevel3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 243);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationAttendanceLevel3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 246);
			res.AddReturnedValue(ex2, "", "GetActivationAttendanceLevel3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 247);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel4(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationAttendanceLevel4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 257);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationAttendanceLevel4", (object)softwareSerial, (object)lkSerial, (object)package, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationAttendanceLevel4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 260);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationAttendanceLevel4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 264);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationAttendanceLevel4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 267);
			res.AddReturnedValue(ex2, "", "GetActivationAttendanceLevel4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 268);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel5(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationAttendanceLevel5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 278);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationAttendanceLevel5", (object)softwareSerial, (object)lkSerial, (object)package, (object)days, (object)clientNetNumber, (object)expiredDate, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationAttendanceLevel5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 281);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationAttendanceLevel5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 286);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationAttendanceLevel5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 289);
			res.AddReturnedValue(ex2, "", "GetActivationAttendanceLevel5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkActivationCodeHub.cs", 290);
		}
		return res;
	}
}
