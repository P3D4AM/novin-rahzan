using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub;

public class CashierClientHub : RetriableHubConnection, ICashierSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<WebCashier> _getTask;

	private TaskCompletionSource<WebCashier> _getByUserTask;

	private TaskCompletionSource<List<WebCashier>> _getAllTask;

	public string ContextGuid { get; set; }

	public CashierClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCashier>>(base.Connection, "OnGetAllItems", (Action<List<WebCashier>>)delegate(List<WebCashier> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\CashierClientHub.cs", 33);
				}
			});
			HubConnectionExtensions.On<WebCashier>(base.Connection, "OnGetItem", (Action<WebCashier>)delegate(WebCashier e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\CashierClientHub.cs", 42);
				}
			});
			HubConnectionExtensions.On<WebCashier>(base.Connection, "OnGetByUsernameAsync", (Action<WebCashier>)delegate(WebCashier e)
			{
				try
				{
					_getByUserTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\CashierClientHub.cs", 50);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\CashierClientHub.cs", 53);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCashier>>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCashier>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCashier>> GetByUsernameAsync(string username, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
