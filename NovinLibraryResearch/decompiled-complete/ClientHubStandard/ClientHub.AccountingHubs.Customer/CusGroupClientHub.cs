using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Customer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.AccountingHubs.Customer;

public class CusGroupClientHub : RetriableHubConnection, ICustomerGroupSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebCusGroup>> _getAllTask;

	private TaskCompletionSource<WebCusGroup> _getTask;

	public string ContextGuid { get; set; }

	public CusGroupClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCusGroup>>(base.Connection, "OnGetAllAsync", (Action<List<WebCusGroup>>)delegate(List<WebCusGroup> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CusGroupClientHub.cs", 30);
				}
			});
			HubConnectionExtensions.On<WebCusGroup>(base.Connection, "OnGetAsync", (Action<WebCusGroup>)delegate(WebCusGroup e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CusGroupClientHub.cs", 38);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CusGroupClientHub.cs", 41);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCusGroup>>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCusGroup>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
