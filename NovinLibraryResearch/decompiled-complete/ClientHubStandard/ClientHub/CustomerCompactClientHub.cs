using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Customer;
using Serilog;

namespace ClientHub;

public class CustomerCompactClientHub : RetriableHubConnection, ICustomerCompactSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<WebCusCompact> _getTask;

	private TaskCompletionSource<List<WebCusCompact>> _getAllTask;

	private TaskCompletionSource<List<WebCusCompact>> _getAllByFilter;

	public string ContextGuid { get; set; }

	public CustomerCompactClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<WebCusCompact>(base.Connection, "OnGetItem", (Action<WebCusCompact>)delegate(WebCusCompact e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerCompactClientHub.cs", 32);
				}
			});
			HubConnectionExtensions.On<List<WebCusCompact>>(base.Connection, "OnGetAllAsync", (Action<List<WebCusCompact>>)delegate(List<WebCusCompact> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerCompactClientHub.cs", 40);
				}
			});
			HubConnectionExtensions.On<List<WebCusCompact>>(base.Connection, "OnGetAllByFilterAsync", (Action<List<WebCusCompact>>)delegate(List<WebCusCompact> e)
			{
				try
				{
					_getAllByFilter?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerCompactClientHub.cs", 48);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerCompactClientHub.cs", 51);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCusCompact>> GetAsync(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>> GetAllAsync(List<Guid> lstMoeinGuids, bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCusCompact>>> GetAllAsync(CustomerCompactFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
