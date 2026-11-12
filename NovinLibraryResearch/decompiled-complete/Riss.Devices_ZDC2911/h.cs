using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Riss.Devices;

internal class h : Zd2911Monitor
{
	private ReceiveHandler m_a;

	private IPAddress b;

	private IPEndPoint c;

	private UdpClient d;

	private bool e;

	[SpecialName]
	public virtual void add_ReceiveHandler(ReceiveHandler value)
	{
		ReceiveHandler receiveHandler = this.m_a;
		ReceiveHandler receiveHandler2;
		do
		{
			receiveHandler2 = receiveHandler;
			ReceiveHandler value2 = (ReceiveHandler)Delegate.Combine(receiveHandler2, value);
			receiveHandler = Interlocked.CompareExchange(ref this.m_a, value2, receiveHandler2);
		}
		while ((object)receiveHandler != receiveHandler2);
	}

	[SpecialName]
	public virtual void remove_ReceiveHandler(ReceiveHandler value)
	{
		ReceiveHandler receiveHandler = this.m_a;
		ReceiveHandler receiveHandler2;
		do
		{
			receiveHandler2 = receiveHandler;
			ReceiveHandler value2 = (ReceiveHandler)Delegate.Remove(receiveHandler2, value);
			receiveHandler = Interlocked.CompareExchange(ref this.m_a, value2, receiveHandler2);
		}
		while ((object)receiveHandler != receiveHandler2);
	}

	[SpecialName]
	public virtual bool get_IsBusy()
	{
		return e;
	}

	public h(string A_0, int A_1)
	{
		b = IPAddress.Parse(A_0);
		c = new IPEndPoint(b, A_1);
		d = new UdpClient(c);
	}

	public override bool OpenListen()
	{
		if (d != null)
		{
			e = true;
			d.BeginReceive(a, d);
			return true;
		}
		return false;
	}

	public override void CloseListen()
	{
		if (d != null)
		{
			e = false;
			d.Client.Shutdown(SocketShutdown.Send);
			d.Close();
			d = null;
		}
	}

	private void a(IAsyncResult A_0)
	{
		try
		{
			Record record = new Record();
			IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
			byte[] array = ((UdpClient)A_0.AsyncState).EndReceive(A_0, ref remoteEP);
			if (a(array))
			{
				record.DIN = BitConverter.ToUInt64(array, 8);
				ulong a_ = BitConverter.ToUInt64(array, 16);
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
				receiveEventArg.endPoint = remoteEP;
				if (array.Length > 24)
				{
					receiveEventArg.sn = new byte[8];
					Array.Copy(array, 24, receiveEventArg.sn, 0, 8);
				}
				if (this.m_a != null)
				{
					this.m_a(this, receiveEventArg);
				}
				byte[] array2 = new byte[array.Length];
				array.CopyTo(array2, 0);
				UdpClient A_ = (UdpClient)A_0.AsyncState;
				p @object = new p(array2, ref A_, remoteEP);
				Thread thread = new Thread(@object.a);
				thread.Start();
			}
			((UdpClient)A_0.AsyncState).BeginReceive(a, (UdpClient)A_0.AsyncState);
		}
		catch
		{
		}
	}

	private bool a(byte[] A_0)
	{
		uint num = BitConverter.ToUInt32(A_0, 0);
		uint num2 = BitConverter.ToUInt32(A_0, 4);
		int num3 = 0;
		if (2012071004 != num)
		{
			return false;
		}
		for (int num4 = 0; num4 < 16; num4++)
		{
			num3 += A_0[num4 + 8];
		}
		if (num3 != num2)
		{
			return false;
		}
		return true;
	}
}
