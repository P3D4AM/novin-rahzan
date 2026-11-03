using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Utilities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Connection;
using PacketParser.Issabel;

namespace EntityCache.CallCenter;

public class IsabelleCallCenter : IsabelleCallCenterBase, IHasContext, IDisposable
{
	private EnConnectionState _connectionState;

	private bool _disposed;

	private IssabelClient _client;

	public string ContextGuid { get; set; }

	public byte[] ConnectionImage { get; private set; }

	public new string Ip { get; set; }

	public new int Port { get; set; }

	public new string UserName { get; set; }

	public new string Password { get; set; }

	public new string FriendllyName { get; set; }

	public new List<IsabelleQueue> Queues { get; set; }

	public new string LineReleaseCode { get; set; }

	public new EnConnectionState CallCenterStatus
	{
		get
		{
			return _connectionState;
		}
		set
		{
			_connectionState = value;
			_client = (IssabelClient)IssabelInstanceHolder.GetInstance(ContextGuid).IssabelSignalRClient;
			_client.ConnectionStateChanged += Issabel_ConnectionStateChanged;
		}
	}

	public IsabelleCallCenter(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public void Dispose()
	{
		try
		{
			if (!_disposed)
			{
				_disposed = true;
				_client.ConnectionStateChanged -= Issabel_ConnectionStateChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IsabelleCallCenter.cs", 53);
		}
	}

	private Task Issabel_ConnectionStateChanged(object sender, ConnectionStateEventArgs e)
	{
		try
		{
			switch (e.NewState)
			{
			case EnConnectionState.Connected:
				ConnectionImage = ImageResourceManager.ConnectionImageConnected;
				break;
			case EnConnectionState.Disconnected:
				ConnectionImage = ImageResourceManager.ConnectionImageDisconnect;
				break;
			case EnConnectionState.Connecting:
				ConnectionImage = ImageResourceManager.ConnectionImageConnecting;
				break;
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_ConnectionStateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IsabelleCallCenter.cs", 76);
			throw;
		}
	}
}
