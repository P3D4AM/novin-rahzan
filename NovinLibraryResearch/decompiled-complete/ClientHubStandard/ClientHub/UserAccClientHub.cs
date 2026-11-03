using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub;

public class UserAccClientHub : RetriableHubConnection, IUserAccSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<WebUserAcc> _getTask;

	public string ContextGuid { get; set; }

	public UserAccClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<WebUserAcc>(base.Connection, "OnGetUser", (Action<WebUserAcc>)delegate(WebUserAcc e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\UserAccClientHub.cs", 29);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\User\\UserAccClientHub.cs", 32);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebUserAcc>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
