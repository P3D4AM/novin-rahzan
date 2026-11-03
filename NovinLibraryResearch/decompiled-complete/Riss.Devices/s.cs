using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Riss.Devices;

internal class s : ZdCommBase
{
	private const int m_a = 1024;

	private const long m_b = 2147483648L;

	private const long m_c = 402653184L;

	private const long m_d = 2550136844L;

	private Socket e;

	private IPEndPoint f;

	[SpecialName]
	public IPEndPoint d()
	{
		if (f == null)
		{
			c();
		}
		return f;
	}

	[SpecialName]
	public void a(IPEndPoint A_0)
	{
		f = A_0;
	}

	private void c()
	{
		IPAddress address = IPAddress.Parse(base.Device.IpAddress);
		IPEndPoint a_ = new IPEndPoint(address, Convert.ToInt32(base.Device.IpPort));
		a(a_);
	}

	private void b()
	{
		e = new Socket(d().AddressFamily, SocketType.Dgram, ProtocolType.Udp);
		byte[] optionInValue = new byte[1] { Convert.ToByte(value: false) };
		byte[] optionOutValue = new byte[4];
		e.IOControl((IOControlCode)2550136844L, optionInValue, optionOutValue);
		e.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 30000);
		e.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 30000);
		e.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024);
	}

	private bool a()
	{
		if (e.Poll(10000, SelectMode.SelectRead))
		{
			byte[] buffer = new byte[1];
			try
			{
				if (e.Receive(buffer, 1, SocketFlags.Peek) == 0)
				{
					return true;
				}
			}
			catch
			{
			}
		}
		return false;
	}

	public override void Close()
	{
		if (e != null)
		{
			if (e.Connected)
			{
				e.Shutdown(SocketShutdown.Send);
			}
			e.Close();
		}
		e = null;
	}

	public override bool Open()
	{
		c();
		b();
		e.Connect(d());
		bool connected = e.Connected;
		if (!connected)
		{
			Close();
		}
		return connected;
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		if (a() && !Open())
		{
			return 0;
		}
		int num = 0;
		byte[] buffer2 = buffer;
		int num2 = 0;
		int num3 = size;
		int num4 = num3;
		DateTime now = DateTime.Now;
		while (num3 != 0 && (DateTime.Now - now).TotalMilliseconds < (double)timeout)
		{
			int size2 = ((num3 > 1024) ? 1024 : num3);
			size2 = e.Send(buffer2, num2, size2, SocketFlags.None);
			num2 += size2;
			num3 -= size2;
			num += size2;
			if (size2 <= 0)
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
		if (a() && !Open())
		{
			return 0;
		}
		byte[] array = new byte[size];
		int num = 0;
		int num2 = size;
		int num3 = num2;
		int num4 = 0;
		DateTime now = DateTime.Now;
		while (num2 != 0 && (DateTime.Now - now).TotalMilliseconds < (double)timeout)
		{
			int num5 = 0;
			try
			{
				e.Blocking = true;
				num5 = e.Receive(array, num, num2, SocketFlags.None);
			}
			catch
			{
				break;
			}
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
