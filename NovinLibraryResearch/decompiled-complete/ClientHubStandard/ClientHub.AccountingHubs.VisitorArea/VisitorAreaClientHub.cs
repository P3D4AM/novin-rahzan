using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.VisitorArea;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.AccountingHubs.VisitorArea;

public class VisitorAreaClientHub : RetriableHubConnection, IVisitorAreaSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebVisitorArea>> _getAllTask;

	private TaskCompletionSource<WebVisitorArea> _getTask;

	public string ContextGuid { get; set; }

	public VisitorAreaClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebVisitorArea>>(base.Connection, "OnGetAllAsync", (Action<List<WebVisitorArea>>)delegate(List<WebVisitorArea> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\VisitorArea\\VisitorAreaClientHub.cs", 30);
				}
			});
			HubConnectionExtensions.On<WebVisitorArea>(base.Connection, "OnGetAsync", (Action<WebVisitorArea>)delegate(WebVisitorArea e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\VisitorArea\\VisitorAreaClientHub.cs", 38);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\VisitorArea\\VisitorAreaClientHub.cs", 41);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebVisitorArea>>> GetAllAsync(Guid? parentGuid, bool status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebVisitorArea>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
