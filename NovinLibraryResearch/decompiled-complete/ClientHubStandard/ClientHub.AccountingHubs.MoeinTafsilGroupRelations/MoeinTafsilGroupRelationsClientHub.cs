using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.MoeinTafsilGroupRelations;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.AccountingHubs.MoeinTafsilGroupRelations;

public class MoeinTafsilGroupRelationsClientHub : RetriableHubConnection, IMoeinTafsilGroupRelationsSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebMoeinTafsilGroupRelation>> _getAllTask;

	public string ContextGuid { get; set; }

	public MoeinTafsilGroupRelationsClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebMoeinTafsilGroupRelation>>(base.Connection, "OnGetAllAsync", (Action<List<WebMoeinTafsilGroupRelation>>)delegate(List<WebMoeinTafsilGroupRelation> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\MoeinTafsilGroupRelations\\MoeinTafsilGroupRelationsClientHub.cs", 29);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\MoeinTafsilGroupRelations\\MoeinTafsilGroupRelationsClientHub.cs", 32);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebMoeinTafsilGroupRelation>>> GetAllAsync(Guid? moeinGuid, Guid? tafsilLevelGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
