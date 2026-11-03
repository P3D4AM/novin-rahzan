using System.Runtime.CompilerServices;

namespace Riss.Devices;

public class Device
{
	private int a;

	private string b;

	private string c;

	private int d;

	private int e;

	private string f;

	private int g;

	private string h;

	private string i;

	private int j;

	[CompilerGenerated]
	private CommunicationType k;

	public int DN
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public string SerialNumber
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public string Model
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
		}
	}

	public CommunicationType CommunicationType
	{
		[CompilerGenerated]
		get
		{
			return k;
		}
		[CompilerGenerated]
		set
		{
			k = value;
		}
	}

	public int Baudrate
	{
		get
		{
			return d;
		}
		set
		{
			d = value;
		}
	}

	public int SerialPort
	{
		get
		{
			return e;
		}
		set
		{
			e = value;
		}
	}

	public string Password
	{
		get
		{
			return f;
		}
		set
		{
			f = value;
		}
	}

	public int IpPort
	{
		get
		{
			return g;
		}
		set
		{
			g = value;
		}
	}

	public string IpAddress
	{
		get
		{
			return h;
		}
		set
		{
			h = value;
		}
	}

	public string Label
	{
		get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public int ConnectionModel
	{
		get
		{
			return j;
		}
		set
		{
			j = value;
		}
	}
}
