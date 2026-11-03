namespace Riss.Devices;

public class Enroll
{
	private ulong a;

	private EnrollType b;

	private bool c;

	private byte[] d;

	private string e;

	private string f;

	public ulong DIN
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

	public EnrollType EnrollType
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

	public bool IsDuress
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

	public byte[] Fingerprint
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

	public string Password
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

	public string CardID
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
}
