using System;
using System.Data;
using System.Threading.Tasks;
using ClientHub.NPLockHub;
using ClientHub.NPLockHub.LockHub;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Connection.LowLevel.Accounting.Lock;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.LockService;

public class NpLockHubInstances : INpAsyncDisposable, IAsyncDisposable
{
	private bool _isDisposed;

	private static NpLockHubInstances _instance;

	public static NpLockHubInstances Instance => _instance;

	public bool IsDisposed => _isDisposed;

	public ICheckActivationSignalR CheckActivation { get; set; }

	public IActivationCodeSignalR ActivationCode { get; set; }

	public IDemoSignalR Demo { get; set; }

	public ISoftwareSerialManagerSignalR SoftwareSerialManager { get; set; }

	public ILkEnvironmentSignalR Environment { get; set; }

	public ILkServerTcpSignalR Server { get; set; }

	public ConnectionEventArgs<HubConnectionState> LastStatus { get; set; }

	public static event Func<object, ConnectionEventArgs<HubConnectionState>, Task> OnHubChanged;

	public static void SetInstance(string contextGuid, NpLockHubInstances instance)
	{
		try
		{
			if (_instance != null && !_instance.IsDisposed)
			{
				throw new DuplicateNameException("Context with Guid:" + contextGuid + " is already Exist");
			}
			_instance = instance;
			_instance?.HandleEvents();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\LockService\\NpLockHubInstances.cs", 41);
		}
	}

	private void HandleEvents()
	{
		try
		{
			((CheckActivationHub)_instance.CheckActivation).Changed += Instances_Changed;
			((DemoClientHub)_instance.Demo).Changed += Instances_Changed;
			((LkActivationCodeHub)_instance.ActivationCode).Changed += Instances_Changed;
			((LkEnvironmentHub)_instance.Environment).Changed += Instances_Changed;
			((LkServerHub)_instance.Server).Changed += Instances_Changed;
			((SoftwareSerialManagerHub)_instance.SoftwareSerialManager).Changed += Instances_Changed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleEvents", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\LockService\\NpLockHubInstances.cs", 54);
		}
	}

	public async ValueTask DisposeAsync()
	{
		_ = 5;
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (CheckActivation != null)
				{
					((CheckActivationHub)CheckActivation).Changed -= Instances_Changed;
					await CheckActivation.DisposeAsync();
					CheckActivation = null;
				}
				if (ActivationCode != null)
				{
					((LkActivationCodeHub)ActivationCode).Changed -= Instances_Changed;
					await ActivationCode.DisposeAsync();
					ActivationCode = null;
				}
				if (Demo != null)
				{
					((DemoClientHub)Demo).Changed -= Instances_Changed;
					await Demo.DisposeAsync();
					Demo = null;
				}
				if (SoftwareSerialManager != null)
				{
					((SoftwareSerialManagerHub)SoftwareSerialManager).Changed -= Instances_Changed;
					await SoftwareSerialManager.DisposeAsync();
					SoftwareSerialManager = null;
				}
				if (Environment != null)
				{
					((LkEnvironmentHub)Environment).Changed -= Instances_Changed;
					await Environment.DisposeAsync();
					Environment = null;
				}
				if (Server != null)
				{
					((LkServerHub)Server).Changed -= Instances_Changed;
					await Server.DisposeAsync();
					Server = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\LockService\\NpLockHubInstances.cs", 99);
		}
	}

	private Task Instances_Changed(object sender, ConnectionEventArgs<HubConnectionState> e)
	{
		try
		{
			LastStatus = e;
			NpLockHubInstances.OnHubChanged?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instances_Changed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\LockService\\NpLockHubInstances.cs", 108);
		}
		return Task.CompletedTask;
	}
}
