using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading;
using Riss.Devices;

internal class b : Zd2911Monitor
{
	private const int m_a = 5000;

	private SerialPort m_b;

	private ReceiveHandler c;

	private bool d;

	[SpecialName]
	public virtual void add_ReceiveHandler(ReceiveHandler value)
	{
		ReceiveHandler receiveHandler = c;
		ReceiveHandler receiveHandler2;
		do
		{
			receiveHandler2 = receiveHandler;
			ReceiveHandler value2 = (ReceiveHandler)Delegate.Combine(receiveHandler2, value);
			receiveHandler = Interlocked.CompareExchange(ref c, value2, receiveHandler2);
		}
		while ((object)receiveHandler != receiveHandler2);
	}

	[SpecialName]
	public virtual void remove_ReceiveHandler(ReceiveHandler value)
	{
		ReceiveHandler receiveHandler = c;
		ReceiveHandler receiveHandler2;
		do
		{
			receiveHandler2 = receiveHandler;
			ReceiveHandler value2 = (ReceiveHandler)Delegate.Remove(receiveHandler2, value);
			receiveHandler = Interlocked.CompareExchange(ref c, value2, receiveHandler2);
		}
		while ((object)receiveHandler != receiveHandler2);
	}

	[SpecialName]
	public virtual bool get_IsBusy()
	{
		return d;
	}

	public b(int A_0, int A_1)
	{
		a(A_0, A_1);
	}

	private void a(int A_0, int A_1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		m_b = new SerialPort();
		m_b.PortName = "COM" + A_0;
		m_b.BaudRate = A_1;
		m_b.DataBits = 8;
		m_b.StopBits = (StopBits)1;
		m_b.Parity = (Parity)0;
		m_b.RtsEnable = true;
		m_b.ReceivedBytesThreshold = 34;
		m_b.ReadTimeout = 5000;
	}

	public override bool OpenListen()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		bool result = false;
		try
		{
			if (!m_b.IsOpen)
			{
				m_b.Open();
				d = true;
				result = true;
				m_b.DataReceived += new SerialDataReceivedEventHandler(a);
			}
		}
		catch
		{
		}
		return result;
	}

	public override void CloseListen()
	{
		if (m_b != null && m_b.IsOpen)
		{
			m_b.Close();
			d = false;
			m_b = null;
		}
	}

	private void a(object A_0, SerialDataReceivedEventArgs A_1)
	{
		try
		{
			int bytesToRead = m_b.BytesToRead;
			byte[] array = new byte[bytesToRead];
			m_b.Read(array, 0, array.Length);
			if (1 == a(42330, 599, array) && a(array))
			{
				Record record = new Record();
				record.DIN = BitConverter.ToUInt64(array, 16);
				ulong a_ = BitConverter.ToUInt64(array, 24);
				int year = Zd2911Utils.k(a_);
				int month = Zd2911Utils.j(a_);
				int day = Zd2911Utils.i(a_);
				record.Verify = Zd2911Utils.g(a_);
				record.Action = Zd2911Utils.h(a_);
				record.JobCode = Zd2911Utils.f(a_);
				record.Antipassback = Zd2911Utils.e(a_);
				record.DN = Zd2911Utils.d(a_);
				int hour = Zd2911Utils.c(a_);
				int minute = Zd2911Utils.b(a_);
				int second = Zd2911Utils.a(a_);
				record.Clock = new DateTime(year, month, day, hour, minute, second);
				ReceiveEventArg receiveEventArg = new ReceiveEventArg();
				receiveEventArg.record = record;
				if (c != null)
				{
					c(this, receiveEventArg);
				}
			}
		}
		catch
		{
		}
	}

	private int a(ushort A_0, ushort A_1, byte[] A_2)
	{
		ushort num = BitConverter.ToUInt16(A_2, 0);
		ushort num2 = BitConverter.ToUInt16(A_2, 4);
		if (A_0 != num || A_1 != num2)
		{
			return 0;
		}
		int num3 = BitConverter.ToUInt16(A_2, 6) + 8;
		ushort num4 = BitConverter.ToUInt16(A_2, num3);
		if (num4 != a(A_2, num3))
		{
			return 0;
		}
		return 1;
	}

	private ushort a(byte[] A_0, int A_1)
	{
		ushort num = 0;
		for (ushort num2 = 0; num2 < A_1; num2++)
		{
			num += A_0[num2];
		}
		return num;
	}

	private bool a(byte[] A_0)
	{
		uint num = BitConverter.ToUInt32(A_0, 8);
		uint num2 = BitConverter.ToUInt32(A_0, 12);
		int num3 = 0;
		if (2012071004 != num)
		{
			return false;
		}
		for (int num4 = 0; num4 < 16; num4++)
		{
			num3 += A_0[num4 + 16];
		}
		if (num3 != num2)
		{
			return false;
		}
		return true;
	}
}
