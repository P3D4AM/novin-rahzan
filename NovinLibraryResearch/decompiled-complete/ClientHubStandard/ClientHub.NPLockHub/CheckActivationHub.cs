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

namespace ClientHub.NPLockHub;

public class CheckActivationHub : RetriableHubConnection, ICheckActivationSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public CheckActivationHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> CheckLKWithWebServiceAsync(Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 27);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "CheckLKWithWebServiceAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 30);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 32);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 35);
			res.AddReturnedValue(ex2, "", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 36);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetActivationCodeAsync(string value, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 46);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetActivationCodeAsync", (object)value, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "SetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 49);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 51);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 54);
			res.AddReturnedValue(ex2, "", "SetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 55);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationCodeAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 65);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetActivationCodeAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 68);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 72);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 75);
			res.AddReturnedValue(ex2, "", "GetActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 76);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> RemoveScaleNoAndDaysAsync(string activationCode, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "RemoveScaleNoAndDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 86);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "RemoveScaleNoAndDaysAsync", (object)activationCode, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "RemoveScaleNoAndDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 89);
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "RemoveScaleNoAndDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 92);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "RemoveScaleNoAndDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 95);
			res.AddReturnedValue(ex2, "", "RemoveScaleNoAndDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 96);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> SetRemoveDaysAsync(string activationCode, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetRemoveDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 106);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "SetRemoveDaysAsync", (object)activationCode, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "SetRemoveDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 109);
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SetRemoveDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 112);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetRemoveDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 115);
			res.AddReturnedValue(ex2, "", "SetRemoveDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 116);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetClientSetAsync(Package package, string value, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 126);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetClientSetAsync", (object)package, (object)value, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "SetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 129);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 131);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 134);
			res.AddReturnedValue(ex2, "", "SetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 135);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetLkSerialAsync(string version, Package package, long value, bool isDemo, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 145);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetLkSerialAsync", (object)version, (object)package, (object)value, (object)isDemo, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 148);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 150);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 153);
			res.AddReturnedValue(ex2, "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 154);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetHoghoghActivationCodeAsync(Package package, string value, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetHoghoghActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 164);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetHoghoghActivationCodeAsync", (object)package, (object)value, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "SetHoghoghActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 167);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetHoghoghActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 169);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetHoghoghActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 172);
			res.AddReturnedValue(ex2, "", "SetHoghoghActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 173);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetActivateDateAsync(DateTime value, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetActivateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 183);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetActivaDateAsync", (object)value, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token), ReturnedState.None, "SetActivateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 186);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetActivateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 188);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetActivateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 191);
			res.AddReturnedValue(ex2, "", "SetActivateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 192);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<DateTime>> GetActiveDateAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<DateTime> res = new ReturnedSaveFuncInfoWithValue<DateTime>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<DateTime> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetActiveDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 202);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<DateTime> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<DateTime>>(base.Connection, "GetActiveDateAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetActiveDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 205);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetActiveDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 209);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetActiveDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 212);
			res.AddReturnedValue(ex2, "", "GetActiveDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 213);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> GetApplicationTypeAsync(string softwareSerial, string hardwareSerial, string activationCode, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<Level_Accounting> res = new ReturnedSaveFuncInfoWithValue<Level_Accounting>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetApplicationTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 223);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<Level_Accounting> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<Level_Accounting>>(base.Connection, "GetApplicationTypeAsync", (object)softwareSerial, (object)hardwareSerial, (object)activationCode, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetApplicationTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 226);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetApplicationTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 230);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetApplicationTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 233);
			res.AddReturnedValue(ex2, "", "GetApplicationTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 234);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<Level_Hozor>> GetHozorTypeAsync(string softwareSerial, string hardwareSerial, string activationCode, bool isDemo, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<Level_Hozor> res = new ReturnedSaveFuncInfoWithValue<Level_Hozor>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 244);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<Level_Hozor> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<Level_Hozor>>(base.Connection, "GetHozorTypeAsync", (object)softwareSerial, (object)hardwareSerial, (object)activationCode, (object)isDemo, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 247);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 251);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 254);
			res.AddReturnedValue(ex2, "", "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 255);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetClientSetAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 265);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetClientSetAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 268);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 272);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 275);
			res.AddReturnedValue(ex2, "", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 276);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<short>> GetNumberOfDaysAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<short> res = new ReturnedSaveFuncInfoWithValue<short>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<short> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetNumberOfDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 286);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<short> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<short>>(base.Connection, "GetNumberOfDaysAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetNumberOfDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 289);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetNumberOfDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 293);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetNumberOfDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 296);
			res.AddReturnedValue(ex2, "", "GetNumberOfDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 297);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetScaleNoAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetScaleNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 307);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetScaleNoAsync", (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetScaleNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 310);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetScaleNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 314);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetScaleNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 317);
			res.AddReturnedValue(ex2, "", "GetScaleNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\CheckActivationHub.cs", 318);
		}
		return res;
	}
}
