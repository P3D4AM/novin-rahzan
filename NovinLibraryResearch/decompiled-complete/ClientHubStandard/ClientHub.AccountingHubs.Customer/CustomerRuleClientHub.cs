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
using PacketParser.Entities.WordPress;
using Serilog;

namespace ClientHub.AccountingHubs.Customer;

public class CustomerRuleClientHub : RetriableHubConnection, ICustomerRoleSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebCustomerRole>> _getAllTask;

	private TaskCompletionSource<WebCustomerRole> _getTask;

	public string ContextGuid { get; set; }

	public CustomerRuleClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebCustomerRole>>(base.Connection, "OnGetAllAsync", (Action<List<WebCustomerRole>>)delegate(List<WebCustomerRole> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerRuleClientHub.cs", 33);
				}
			});
			HubConnectionExtensions.On<WebCustomerRole>(base.Connection, "OnGetAsync", (Action<WebCustomerRole>)delegate(WebCustomerRole e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerRuleClientHub.cs", 41);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Customer\\CustomerRuleClientHub.cs", 44);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebCustomerRole>>> GetAllAsync(bool? status = true, CancellationToken token = default(CancellationToken), string tranName = null)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebCustomerRole>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
