using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Lock;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;
using Serilog;

namespace ClientHub.NPLockHub.LockHub;

public class SoftwareSerialManagerHub : RetriableHubConnection, ISoftwareSerialManagerSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public SoftwareSerialManagerHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerial_AccountingAsync(string version, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetSoftwareSerial_AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 28);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetSoftwareSerialAccountingAsync", (object)version, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetSoftwareSerial_AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 31);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetSoftwareSerial_AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 35);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetSoftwareSerial_AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 38);
			res.AddReturnedValue(ex2, "", "GetSoftwareSerial_AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 39);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerial_HozorAsync(string version, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 49);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "GetSoftwareSerial_HozorAsync", (object)version, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 52);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 56);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 59);
			res.AddReturnedValue(ex2, "", "GetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 60);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSoftwareSerial_HozorAsync(string value, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 70);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetSoftwareSerialHozorAsync", (object)value, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "SetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 73);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 75);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 78);
			res.AddReturnedValue(ex2, "", "SetSoftwareSerial_HozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 79);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<AccountingPackageClass>> SetAccountingSoftwareSerial(string softwareSerial, string activationCode, List<PackageContainer> packages, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<AccountingPackageClass> res = new ReturnedSaveFuncInfoWithValue<AccountingPackageClass>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetAccountingSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 89);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<AccountingPackageClass> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<AccountingPackageClass>>(base.Connection, "SetAccountingSoftwareSerial", (object)softwareSerial, (object)activationCode, (object)packages, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "SetAccountingSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 92);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SetAccountingSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 96);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetAccountingSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 99);
			res.AddReturnedValue(ex2, "", "SetAccountingSoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\SoftwareSerialManagerHub.cs", 100);
		}
		return res;
	}
}
