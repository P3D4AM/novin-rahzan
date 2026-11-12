using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using Riss.Devices;

internal class q : ZdCommBase
{
	private const int m_a = 2000;

	private static SerialPort b;

	public override void Close()
	{
		if (b != null)
		{
			if (b.IsOpen)
			{
				b.Close();
				Thread.Sleep(50);
			}
			((Component)(object)b).Dispose();
		}
	}

	private void a()
	{
		b.PortName = "COM" + base.Device.SerialPort;
		b.BaudRate = base.Device.Baudrate;
		b.DataBits = 8;
		b.StopBits = (StopBits)1;
		b.Parity = (Parity)0;
		b.RtsEnable = true;
		b.ReadTimeout = 2000;
		b.WriteTimeout = 2000;
	}

	public override bool Open()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		bool flag = false;
		if (b != null && b.IsOpen)
		{
			b.Close();
			Thread.Sleep(50);
		}
		if (b == null)
		{
			b = new SerialPort();
		}
		a();
		try
		{
			if (!b.IsOpen)
			{
				Thread.Sleep(50);
				b.Open();
			}
			return true;
		}
		catch
		{
			if (b.IsOpen)
			{
				b.Close();
				Thread.Sleep(50);
			}
			a();
			try
			{
				if (!b.IsOpen)
				{
					Thread.Sleep(50);
					b.Open();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		Thread.Sleep(10);
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
		Thread.Sleep(10);
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
