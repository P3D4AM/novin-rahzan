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

public class DemoClientHub : RetriableHubConnection, IDemoSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public DemoClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> CheckLockAsync(string version, Package package, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "CheckLockAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\DemoClientHub.cs", 26);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<string> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<string>>(base.Connection, "CheckLockAsync", (object)version, (object)package, (object)ignoreHttpExceptions, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "CheckLockAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\DemoClientHub.cs", 29);
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "CheckLockAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\DemoClientHub.cs", 32);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckLockAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\DemoClientHub.cs", 35);
			res.AddReturnedValue(ex2, "", "CheckLockAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\DemoClientHub.cs", 36);
		}
		return res;
	}
}
