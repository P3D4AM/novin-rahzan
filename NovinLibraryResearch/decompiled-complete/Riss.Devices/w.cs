using System;
using System.IO.Ports;
using Riss.Devices;

internal class w : ZdCommBase
{
	private const int m_a = 500;

	private SerialPort b = new SerialPort();

	public override void Close()
	{
		if (b != null)
		{
			b.Close();
		}
		b = null;
	}

	private void a()
	{
		b.PortName = "COM" + base.Device.SerialPort;
		b.BaudRate = base.Device.Baudrate;
		b.DataBits = 8;
		b.StopBits = (StopBits)1;
		b.Parity = (Parity)0;
		b.RtsEnable = true;
		b.ReadTimeout = 500;
		b.WriteTimeout = 500;
	}

	public override bool Open()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		bool result = false;
		if (b == null)
		{
			b = new SerialPort();
		}
		a();
		try
		{
			b.Open();
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		int num = 0;
		byte[] array = buffer;
		int num2 = 0;
		int num3 = size;
		int num4 = num3;
		DateTime now = DateTime.Now;
		while (num3 != 0 && (DateTime.Now - now).TotalMilliseconds < (double)timeout)
		{
			int num5 = ((num3 > 1024) ? 1024 : num3);
			b.Write(array, num2, num5);
			num2 += num5;
			num3 -= num5;
			num += num5;
			if (num5 <= 0)
			{
				break;
			}
			now = DateTime.Now;
			base.CommunicationPercent = (double)num / (double)num4 * 100.0;
		}
		return num;
	}

	public override int RecvData(ref byte[] buffer, int size, int timeout)
	{
		byte[] array = new byte[size];
		int num = 0;
		int num2 = size;
		int num3 = num2;
		int num4 = 0;
		DateTime now = DateTime.Now;
		while (num2 != 0 && (DateTime.Now - now).TotalMilliseconds < (double)timeout)
		{
			int num5 = b.Read(array, num, num2);
			num += num5;
			num2 -= num5;
			num4 += num5;
			if (num5 > 0)
			{
				now = DateTime.Now;
			}
			base.CommunicationPercent = (double)num4 / (double)num3 * 100.0;
		}
		buffer = array;
		return num4;
	}
}
