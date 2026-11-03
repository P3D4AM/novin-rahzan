using System;
using System.IO;
using System.Net.Sockets;
using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Interface;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class NetworkModem : IModem
{
	private int JustDecompileGenerated_Timeout_k__BackingField;

	public TcpClient Connection { get; private set; }

	public EnSadadDeviceType DeviceType { get; set; }

	public bool HasTimeoutError { get; set; }

	public string IP { get; private set; }

	public EnSadadPcPosConnectionType MediaType { get; set; }

	public int Port { get; private set; }

	public int Timeout
	{
		get
		{
			return JustDecompileGenerated_get_Timeout();
		}
		set
		{
			JustDecompileGenerated_set_Timeout(value);
		}
	}

	public int JustDecompileGenerated_get_Timeout()
	{
		return JustDecompileGenerated_Timeout_k__BackingField;
	}

	private void JustDecompileGenerated_set_Timeout(int value)
	{
		JustDecompileGenerated_Timeout_k__BackingField = value;
	}

	public NetworkModem(EnSadadDeviceType deviceType, string ip, int port = 8888, int? timeout = 10000)
	{
		MediaType = EnSadadPcPosConnectionType.Lan;
		DeviceType = deviceType;
		IP = ip;
		Port = port;
		Connection = new TcpClient();
		SetTimeout(timeout);
	}

	public void CancelReceive()
	{
		try
		{
			if (Connection != null && Connection.Client != null && Connection.Connected)
			{
				Connection.GetStream().Close();
				Connection.Client.Close();
			}
		}
		catch (Exception)
		{
		}
	}

	public void Connect()
	{
		if (Connection == null || Connection.Client == null || !Connection.Connected)
		{
			Connection = new TcpClient
			{
				SendTimeout = Timeout,
				ReceiveTimeout = Timeout
			};
			try
			{
				Connection.Client.Connect(IP, Port);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	public void Disconnect()
	{
		if (Connection == null || Connection.Client == null || !Connection.Client.Connected)
		{
			return;
		}
		try
		{
			Connection.Client.Disconnect(reuseSocket: false);
			Connection.Client.Shutdown(SocketShutdown.Both);
		}
		catch (Exception)
		{
		}
		finally
		{
			Connection.Client = null;
		}
	}

	public byte[] Receive()
	{
		if (Connection == null || Connection.Client == null || !Connection.Connected)
		{
			return new byte[0];
		}
		byte[] array = new byte[2048];
		try
		{
			HasTimeoutError = false;
			NetworkStream stream = Connection.GetStream();
			stream.ReadTimeout = Timeout;
			stream.Read(array, 0, 2048);
			return array;
		}
		catch (IOException)
		{
			Connection.Close();
			HasTimeoutError = true;
			return new byte[0];
		}
	}

	public byte[] Receive(int timeout)
	{
		if (Connection == null || Connection.Client == null || !Connection.Connected)
		{
			return new byte[0];
		}
		byte[] array = new byte[2048];
		try
		{
			HasTimeoutError = false;
			NetworkStream stream = Connection.GetStream();
			stream.Flush();
			stream.ReadTimeout = timeout;
			stream.Read(array, 0, 2048);
			return array;
		}
		catch (IOException)
		{
			Connection.Close();
			HasTimeoutError = true;
			return new byte[0];
		}
	}

	public void Send(PcPosBaseMessage message)
	{
		Connect();
		if (Connection != null && Connection.Client != null && Connection.Connected)
		{
			NetworkStream stream = Connection.GetStream();
			byte[] array = message.Build();
			stream.Write(array, 0, array.Length);
		}
	}

	public void SetTimeout(int? timeout)
	{
		if (!timeout.HasValue || (double)timeout.Value < TimeSpan.FromSeconds(10.0).TotalMilliseconds)
		{
			timeout = checked((int)TimeSpan.FromSeconds(10.0).TotalMilliseconds);
		}
		try
		{
			if (Connection != null && Connection.Client != null && Connection.Connected)
			{
				Connection.SendTimeout = timeout.Value;
				Connection.ReceiveTimeout = timeout.Value;
			}
		}
		catch (Exception)
		{
		}
		Timeout = timeout.Value;
	}
}
