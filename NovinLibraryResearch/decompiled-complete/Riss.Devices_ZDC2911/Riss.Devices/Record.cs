using System;

namespace Riss.Devices;

public class Record
{
	private int a;

	private ulong b;

	private DateTime c;

	private int d;

	private int e;

	private string f;

	private ulong g;

	private int h;

	private int i;

	private int j;

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

	public ulong DIN
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

	public DateTime Clock
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

	public int Verify
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

	public int Action
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

	public string Remark
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

	public ulong MDIN
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

	public int DoorStatus
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

	public int JobCode
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

	public int Antipassback
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

	public Record()
	{
		f = string.Empty;
	}
}
