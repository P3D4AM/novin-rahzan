using System.Runtime.CompilerServices;

namespace Riss.Devices;

public abstract class Zd2911Monitor
{
	[CompilerGenerated]
	private Monitor a;

	public Monitor Monitor
	{
		[CompilerGenerated]
		get
		{
			return a;
		}
		[CompilerGenerated]
		set
		{
			a = value;
		}
	}

	public abstract bool IsBusy { get; }

	public abstract event ReceiveHandler ReceiveHandler;

	public Zd2911Monitor()
	{
	}

	public abstract bool OpenListen();

	public abstract void CloseListen();

	public static Zd2911Monitor CreateZd2911Monitor(Monitor m)
	{
		Zd2911Monitor zd2911Monitor = null;
		switch (m.Mode)
		{
		case 0:
			zd2911Monitor = new h(m.UDPAddress, m.UDPPort);
			zd2911Monitor.Monitor = m;
			break;
		case 1:
			zd2911Monitor = new b(m.SerialPort, m.SerialBaudRate);
			zd2911Monitor.Monitor = m;
			break;
		}
		return zd2911Monitor;
	}
}
