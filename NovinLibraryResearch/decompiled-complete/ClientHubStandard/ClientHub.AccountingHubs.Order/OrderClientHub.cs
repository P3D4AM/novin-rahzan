using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Order;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using Serilog;

namespace ClientHub.AccountingHubs.Order;

public class OrderClientHub : RetriableHubConnection, IOrderSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<WebOrderReport> _getTask;

	private TaskCompletionSource<List<WebOrderReport>> _getAllTask;

	public string ContextGuid { get; set; }

	public OrderClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<WebOrderReport>(base.Connection, "OnGetLastOrderByTafsilGuidAsync", (Action<WebOrderReport>)delegate(WebOrderReport e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Order\\OrderClientHub.cs", 31);
				}
			});
			HubConnectionExtensions.On<List<WebOrderReport>>(base.Connection, "OnGetAllAsync", (Action<List<WebOrderReport>>)delegate(List<WebOrderReport> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Order\\OrderClientHub.cs", 39);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Order\\OrderClientHub.cs", 42);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebOrderReport>> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebOrderReport>>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
