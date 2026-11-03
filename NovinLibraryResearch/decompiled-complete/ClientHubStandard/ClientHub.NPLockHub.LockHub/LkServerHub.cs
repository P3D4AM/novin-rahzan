using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.LkService;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Lock;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.NPLockHub.LockHub;

public class LkServerHub : RetriableHubConnection, ILkServerTcpSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public LkServerHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<clsLkError>> CheckValidLkClientAsync(string uniqueId, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<clsLkError> res = new ReturnedSaveFuncInfoWithValue<clsLkError>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<clsLkError> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "CheckValidLkClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkServerHub.cs", 27);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<clsLkError> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<clsLkError>>(base.Connection, "CheckValidLkClientAsync", (object)uniqueId, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "CheckValidLkClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkServerHub.cs", 30);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "CheckValidLkClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkServerHub.cs", 34);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckValidLkClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkServerHub.cs", 37);
			res.AddReturnedValue(ex2, "", "CheckValidLkClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkServerHub.cs", 38);
		}
		return res;
	}
}
