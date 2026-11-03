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

public class CheckedSaveClientHub : RetriableHubConnection, ICheckedSaveSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebCheckedSave>> _getAllTask;

	public string ContextGuid { get; set; }

	public CheckedSaveClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCheckedSave>>(base.Connection, "OnGetAllItems", (Action<List<WebCheckedSave>>)delegate(List<WebCheckedSave> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\CheckedSaveClientHub.cs", 30);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\CheckedSaveClientHub.cs", 33);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCheckedSave>>> GetAllAsync(checkedTablReportType reportType, Guid hesabGuid, bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
