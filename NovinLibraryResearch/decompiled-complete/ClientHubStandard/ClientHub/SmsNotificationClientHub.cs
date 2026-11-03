using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using Serilog;

namespace ClientHub;

public class SmsNotificationClientHub : RetriableHubConnection, ISmsNotificationSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public event Func<BaseSmsNotification, Task> DataRecieved;

	public SmsNotificationClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<BaseSmsNotification>(base.Connection, "OnNotification", (Action<BaseSmsNotification>)delegate(BaseSmsNotification e)
			{
				this.DataRecieved?.Invoke(e);
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNotificationClientHub.cs", 25);
		}
		return Task.CompletedTask;
	}
}
