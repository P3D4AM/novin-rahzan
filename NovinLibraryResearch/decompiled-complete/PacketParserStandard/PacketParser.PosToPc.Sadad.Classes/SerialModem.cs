using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Interface;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class SerialModem : IModem
{
	public delegate void Action();

	private AutoResetEvent signal = new AutoResetEvent(initialState: false);

	private readonly object locker = new object();

	private bool isDisposed;

	private bool isDisconnected = true;

	private int baudRate;

	private Parity parity;

	private int dataBits;

	private StopBits stopBits;

	private Handshake handshake;

	private int JustDecompileGenerated_Timeout_k__BackingField;

	public int BaudRate
	{
		get
		{
			return baudRate;
		}
		set
		{
			int num = (baudRate = value);
			Connection.BaudRate = num;
		}
	}

	public SerialPort Connection { get; private set; }

	public int DataBits
	{
		get
		{
			return dataBits;
		}
		set
		{
			int num = (dataBits = value);
			Connection.DataBits = num;
		}
	}

	public EnSadadDeviceType DeviceType { get; set; }

	public Handshake Handshake
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return handshake;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Handshake val = (handshake = value);
			Connection.Handshake = val;
		}
	}

	public bool HasTimeoutError { get; set; }

	public EnSadadPcPosConnectionType MediaType { get; set; }

	public Parity Parity
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return parity;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Parity val = (parity = value);
			Connection.Parity = val;
		}
	}

	private byte[] ReceiveBuffer { get; set; }

	public StopBits StopBits
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return stopBits;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			StopBits val = (stopBits = value);
			Connection.StopBits = val;
		}
	}

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

	public int TTD { get; set; }

	public int JustDecompileGenerated_get_Timeout()
	{
		return JustDecompileGenerated_Timeout_k__BackingField;
	}

	public void JustDecompileGenerated_set_Timeout(int value)
	{
		JustDecompileGenerated_Timeout_k__BackingField = value;
	}

	public SerialModem(EnSadadDeviceType deviceType, string portName, int? timeout = 5000, int ttd = 1)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		MediaType = EnSadadPcPosConnectionType.Serial;
		if (Connection == null || !Connection.PortName.Equals(portName) || isDisposed)
		{
			Connection = null;
			Connection = new SerialPort(portName);
			((Component)(object)Connection).Disposed += Connection_Disposed;
			Connection.DataReceived += new SerialDataReceivedEventHandler(Connection_DataReceived);
		}
		DeviceType = deviceType;
		TTD = ttd;
		BaudRate = 115200;
		Parity = (Parity)0;
		DataBits = 8;
		StopBits = (StopBits)1;
		Handshake = (Handshake)0;
		SetTimeout(timeout);
		ReceiveBuffer = new byte[0];
		HasTimeoutError = false;
	}

	public void CancelReceive()
	{
		Disconnect();
		signal.Set();
	}

	public void ClearBuffer()
	{
		if (Connection != null && Connection.IsOpen)
		{
			Connection.ReadExisting();
			Connection.ReadExisting();
		}
	}

	public void Connect()
	{
		try
		{
			lock (locker)
			{
				if (Connection != null && !Connection.IsOpen && isDisconnected)
				{
					Connection.Open();
					if (Connection != null && Connection.BaseStream != null)
					{
						GC.SuppressFinalize(Connection.BaseStream);
					}
				}
			}
		}
		catch (UnauthorizedAccessException)
		{
			Disconnect();
			Connection = null;
		}
	}

	private void Connection_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
	}

	private void Connection_Disposed(object sender, System.EventArgs e)
	{
		isDisposed = true;
	}

	public void Disconnect()
	{
		try
		{
			lock (locker)
			{
				if (!isDisconnected)
				{
					return;
				}
				if (Connection != null && Connection.IsOpen)
				{
					isDisconnected = false;
					Connection.DiscardInBuffer();
					Connection.DiscardOutBuffer();
					Connection.BaseStream.Flush();
					GC.ReRegisterForFinalize(Connection.BaseStream);
					SerialPort connection = Connection;
					if (connection != null)
					{
						connection.Close();
					}
					signal.Set();
					Thread.Sleep(300);
				}
				signal.Set();
				isDisconnected = true;
				Connection = null;
			}
		}
		catch (Exception)
		{
			isDisconnected = true;
			throw;
		}
	}

	public byte[] Receive()
	{
		HasTimeoutError = false;
		if (!signal.WaitOne(Timeout))
		{
			HasTimeoutError = true;
			return new byte[0];
		}
		try
		{
			lock (ReceiveBuffer)
			{
				if (ReceiveBuffer == null || ReceiveBuffer.Length == 0)
				{
					return new byte[0];
				}
				byte[] array = new byte[ReceiveBuffer.Length];
				ReceiveBuffer.CopyTo(array, 0);
				Array.Clear(ReceiveBuffer, 0, ReceiveBuffer.Length);
				return array;
			}
		}
		catch (Exception)
		{
			return new byte[0];
		}
	}

	public byte[] Receive(int timeout)
	{
		HasTimeoutError = false;
		if (!signal.WaitOne(timeout))
		{
			if (timeout == Timeout)
			{
				HasTimeoutError = true;
			}
			return new byte[0];
		}
		try
		{
			lock (ReceiveBuffer)
			{
				if (ReceiveBuffer == null || ReceiveBuffer.Length == 0)
				{
					return new byte[0];
				}
				byte[] array = new byte[ReceiveBuffer.Length];
				ReceiveBuffer.CopyTo(array, 0);
				Array.Clear(ReceiveBuffer, 0, ReceiveBuffer.Length);
				return array;
			}
		}
		catch (Exception)
		{
			return new byte[0];
		}
	}

	public void Send(PcPosBaseMessage message)
	{
		ClearBuffer();
		byte[] array = message.Build();
		if (Connection == null || !Connection.IsOpen)
		{
			return;
		}
		if (DeviceType == EnSadadDeviceType.Bitel)
		{
			byte[] array2 = array;
			foreach (byte value in array2)
			{
				Connection.BaseStream.WriteByte(value);
				Thread.Sleep(TTD);
			}
		}
		else
		{
			Connection.Write(array, 0, array.Length);
			Connection.BaseStream.Flush();
			Thread.Sleep(10);
		}
	}

	public void SetTimeout(int? timeout)
	{
		if (!timeout.HasValue || (double)timeout.Value < TimeSpan.FromSeconds(5.0).TotalMilliseconds)
		{
			timeout = checked((int)TimeSpan.FromSeconds(5.0).TotalMilliseconds);
		}
		try
		{
			if (Connection != null)
			{
				Connection.ReadTimeout = timeout.Value;
				Connection.WriteTimeout = timeout.Value;
			}
		}
		catch (Exception)
		{
		}
		Timeout = timeout.Value;
	}
}
