using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub;

public class GardeshTafsilClientHub : RetriableHubConnection, IGardeshTafsilSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<byte[]> _getAllTask;

	public string ContextGuid { get; set; }

	public GardeshTafsilClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<byte[]>(base.Connection, "OnGetAllItems", (Action<byte[]>)delegate(byte[] e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\GardeshTafsilClientHub.cs", 33);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\GardeshTafsilClientHub.cs", 36);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebGardeshTafsili>>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token)
	{
		throw new NotImplementedException();
	}
}
