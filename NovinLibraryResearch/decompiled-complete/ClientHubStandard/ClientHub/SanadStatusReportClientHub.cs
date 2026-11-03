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

public class SanadStatusReportClientHub : RetriableHubConnection, ISanadStatusReportSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebSanadStatusReport>> _getTask;

	public string ContextGuid { get; set; }

	public SanadStatusReportClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebSanadStatusReport>>(base.Connection, "OnGetAllItems", (Action<List<WebSanadStatusReport>>)delegate(List<WebSanadStatusReport> e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Sanad\\SanadStatusReportClientHub.cs", 30);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Sanad\\SanadStatusReportClientHub.cs", 33);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebSanadStatusReport>>> GetAllAsync(Document? asType, bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
