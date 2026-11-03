using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub;

public class ContextHolderClientHub : RetriableHubConnection, IContextHolderSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<ReturnedSaveFuncInfo> _rollBackTask;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _beginTask;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _commitTask;

	private TaskCompletionSource<bool> _existTask;

	public string ContextGuid { get; set; }

	public ContextHolderClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnRollBackTransaction", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_rollBackTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\ContextHolderClientHub.cs", 29);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnBeginTransaction", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_beginTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\ContextHolderClientHub.cs", 37);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnCommitTransaction", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_commitTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\ContextHolderClientHub.cs", 45);
				}
			});
			HubConnectionExtensions.On<bool>(base.Connection, "OnExistTransaction", (Action<bool>)delegate(bool e)
			{
				try
				{
					_existTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\ContextHolderClientHub.cs", 53);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\ContextHolderClientHub.cs", 56);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> RollBackTransactionAsync(EnCacheConfig config, string tranName, bool disposeContext)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> BeginTransactionAsync(EnCacheConfig config, string tranName, int timeout)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> CommitTransactionAsync(EnCacheConfig config, string tranName, bool disposedContext)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> ExistsAsync(EnCacheConfig config, string tranName)
	{
		throw new NotImplementedException();
	}
}
