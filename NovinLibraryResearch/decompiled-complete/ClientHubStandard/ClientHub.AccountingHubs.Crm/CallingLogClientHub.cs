using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Crm;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Crm;
using Serilog;

namespace ClientHub.AccountingHubs.Crm;

public class CallingLogClientHub : RetriableHubConnection, ICallingLogSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebCallingLogReport>> _getTask;

	private TaskCompletionSource<WebCallingLog> _getUuidTask;

	private TaskCompletionSource<WebCallingLog> _getGuidTask;

	public string ContextGuid { get; set; }

	public CallingLogClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCallingLogReport>>(base.Connection, "OnGetAllAsync", (Action<List<WebCallingLogReport>>)delegate(List<WebCallingLogReport> e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogClientHub.cs", 33);
				}
			});
			HubConnectionExtensions.On<WebCallingLog>(base.Connection, "OnGetAsync", (Action<WebCallingLog>)delegate(WebCallingLog e)
			{
				try
				{
					_getUuidTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogClientHub.cs", 41);
				}
			});
			HubConnectionExtensions.On<WebCallingLog>(base.Connection, "OnGetByGuidAsync", (Action<WebCallingLog>)delegate(WebCallingLog e)
			{
				try
				{
					_getGuidTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogClientHub.cs", 49);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogClientHub.cs", 52);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCallingLogReport>>> GetAllAsync(CallingLogFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCallingLog>> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCallingLog>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
