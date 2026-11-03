using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Crm;
using Serilog;

namespace ClientHub;

public class CallingLogDetailClientHub : RetriableHubConnection, ICallingLogDetailSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebCallingLogDetail>> _getAllTask;

	private TaskCompletionSource<WebCallingLogDetail> _getTask;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	public string ContextGuid { get; set; }

	public CallingLogDetailClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCallingLogDetail>>(base.Connection, "OnGetAllByFilter", (Action<List<WebCallingLogDetail>>)delegate(List<WebCallingLogDetail> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogDetailClientHub.cs", 34);
				}
			});
			HubConnectionExtensions.On<WebCallingLogDetail>(base.Connection, "OnGetAsync", (Action<WebCallingLogDetail>)delegate(WebCallingLogDetail e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogDetailClientHub.cs", 42);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnSaveAsync", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_saveTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogDetailClientHub.cs", 50);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Crm\\CallingLogDetailClientHub.cs", 53);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCallingLogDetail>>> GetAllAsync(CallingLogDetailFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCallingLogDetail>> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebCallingLogDetail callDet, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
