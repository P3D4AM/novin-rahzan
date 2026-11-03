using System.Runtime.CompilerServices;

namespace Riss.Devices;

public class Monitor
{
	[CompilerGenerated]
	private int a;

	[CompilerGenerated]
	private string b;

	[CompilerGenerated]
	private int c;

	[CompilerGenerated]
	private int d;

	[CompilerGenerated]
	private int e;

	public int Mode
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

	public string UDPAddress
	{
		[CompilerGenerated]
		get
		{
			return b;
		}
		[CompilerGenerated]
		set
		{
			b = value;
		}
	}

	public int UDPPort
	{
		[CompilerGenerated]
		get
		{
			return c;
		}
		[CompilerGenerated]
		set
		{
			c = value;
		}
	}

	public int SerialPort
	{
		[CompilerGenerated]
		get
		{
			return d;
		}
		[CompilerGenerated]
		set
		{
			d = value;
		}
	}

	public int SerialBaudRate
	{
		[CompilerGenerated]
		get
		{
			return e;
		}
		[CompilerGenerated]
		set
		{
			e = value;
		}
	}
}
