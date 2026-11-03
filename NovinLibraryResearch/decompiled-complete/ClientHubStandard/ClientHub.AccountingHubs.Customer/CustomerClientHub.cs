using System;
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

public class CustomerClientHub : RetriableHubConnection, ICustomerSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	private TaskCompletionSource<WebCus> _getTask;

	private TaskCompletionSource<WebCus> _getByName;

	private TaskCompletionSource<WebCus> _getFromDatabase;

	private TaskCompletionSource<bool> _checkUserNameTask;

	public string ContextGuid { get; set; }

	public CustomerClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnSaveAsync", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_saveTask?.TrySetResult(e);
				}
				catch (Exception ex6)
				{
					WebErrorLog.Instance.StartLog(ex6, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 32);
				}
			});
			HubConnectionExtensions.On<WebCus>(base.Connection, "OnGetAsync", (Action<WebCus>)delegate(WebCus e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 40);
				}
			});
			HubConnectionExtensions.On<WebCus>(base.Connection, "OnGetByNameAsync", (Action<WebCus>)delegate(WebCus e)
			{
				try
				{
					_getByName?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 48);
				}
			});
			HubConnectionExtensions.On<WebCus>(base.Connection, "OnGetFromDatabaseAsync", (Action<WebCus>)delegate(WebCus e)
			{
				try
				{
					_getFromDatabase?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 56);
				}
			});
			HubConnectionExtensions.On<bool>(base.Connection, "OnCheckUserNameAsync", (Action<bool>)delegate(bool e)
			{
				try
				{
					_checkUserNameTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 64);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerClientHub.cs", 67);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebCus Customer, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetAsync(Guid cusGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetByNameAsync(string firstName, string lastName, Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> CheckUserNameAsync(Guid guid, string userName, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
