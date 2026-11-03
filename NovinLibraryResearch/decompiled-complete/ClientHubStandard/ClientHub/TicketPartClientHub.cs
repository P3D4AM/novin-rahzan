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

public class TicketPartClientHub : RetriableHubConnection, ITicketPartSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebTicketPart>> _getAllTask;

	private TaskCompletionSource<WebTicketPart> _getTask;

	public string ContextGuid { get; set; }

	public TicketPartClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebTicketPart>>(base.Connection, "OnGetAllItems", (Action<List<WebTicketPart>>)delegate(List<WebTicketPart> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketPartClientHub.cs", 32);
				}
			});
			HubConnectionExtensions.On<WebTicketPart>(base.Connection, "OnGetItem", (Action<WebTicketPart>)delegate(WebTicketPart e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketPartClientHub.cs", 40);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Ticket\\TicketPartClientHub.cs", 43);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTicketPart>>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebTicketPart>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
