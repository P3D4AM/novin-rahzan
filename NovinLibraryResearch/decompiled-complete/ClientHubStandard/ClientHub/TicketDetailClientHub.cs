using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using Serilog;

namespace ClientHub;

public class TicketDetailClientHub : RetriableHubConnection, ITicketDetailSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	private TaskCompletionSource<List<WebTicketDet>> _getAllTask;

	private TaskCompletionSource<List<WebTicketReport>> _getAll;

	public string ContextGuid { get; set; }

	public TicketDetailClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnSaveResponse", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_saveTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketDetailClientHub.cs", 34);
				}
			});
			HubConnectionExtensions.On<List<WebTicketDet>>(base.Connection, "OnGetAllItems", (Action<List<WebTicketDet>>)delegate(List<WebTicketDet> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketDetailClientHub.cs", 42);
				}
			});
			HubConnectionExtensions.On<List<WebTicketReport>>(base.Connection, "OnGetAllByFilterResponseAsync", (Action<List<WebTicketReport>>)delegate(List<WebTicketReport> e)
			{
				try
				{
					_getAll?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketDetailClientHub.cs", 50);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketDetailClientHub.cs", 53);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebTicketDet item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTicketDet>>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
