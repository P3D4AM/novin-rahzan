using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Riss.Devices;

internal class a : ZdCommBase
{
	private const int m_a = 65536;

	private Socket m_b;

	private IPEndPoint m_c;

	public override void Close()
	{
		if (this.m_b != null)
		{
			if (this.m_b.Connected)
			{
				this.m_b.Shutdown(SocketShutdown.Send);
			}
			this.m_b.Close();
		}
		this.m_b = null;
	}

	[SpecialName]
	public IPEndPoint d()
	{
		if (this.m_c == null)
		{
			c();
		}
		return this.m_c;
	}

	[SpecialName]
	public void a(IPEndPoint A_0)
	{
		this.m_c = A_0;
	}

	private void c()
	{
		IPAddress address = IPAddress.Parse(base.Device.IpAddress);
		IPEndPoint a_ = new IPEndPoint(address, Convert.ToInt32(base.Device.IpPort));
		a(a_);
	}

	private void b()
	{
		this.m_b = new Socket(d().AddressFamily, SocketType.Stream, ProtocolType.IP);
		LingerOption optionValue = new LingerOption(enable: true, 5);
		this.m_b.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, optionValue);
		this.m_b.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 30000);
		this.m_b.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 30000);
		this.m_b.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 65536);
	}

	public override bool Open()
	{
		bool flag = false;
		c();
		b();
		this.m_b.Connect(d());
		flag = this.m_b.Connected;
		if (!flag)
		{
			Close();
		}
		return flag;
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
			int size2 = ((num3 > 65536) ? 65536 : num3);
			size2 = this.m_b.Send(buffer2, num2, size2, SocketFlags.None);
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
			int num5 = this.m_b.Receive(array, num, num2, SocketFlags.None);
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

	private bool a()
	{
		if (this.m_b.Poll(10000, SelectMode.SelectRead))
		{
			byte[] buffer = new byte[1];
			try
			{
				if (this.m_b.Receive(buffer, 1, SocketFlags.Peek) == 0)
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
}
