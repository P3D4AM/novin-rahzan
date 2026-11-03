using System;
using System.Collections.Generic;

namespace Riss.Devices;

public class User
{
	private int a;

	private ulong b;

	private string c;

	private string d;

	private Sex e;

	private bool f;

	private string g;

	private string h;

	private int i;

	private DateTime j;

	private int k;

	private bool l;

	private bool m;

	private bool n;

	private int o;

	private List<Enroll> p;

	private int q;

	private DateTime r;

	private DateTime s;

	private int t;

	private int u;

	private uint v;

	public int Privilege
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

	public string UserName
	{
		get
		{
			return c;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				c = value;
			}
		}
	}

	public string IDNumber
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

	public Sex Sex
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

	public bool Enable
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

	public string Comment
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

	public string DeptId
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

	public int AttType
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

	public DateTime Birthday
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

	public int AccessControl
	{
		get
		{
			return k;
		}
		set
		{
			k = value;
		}
	}

	public bool ValidityPeriod
	{
		get
		{
			return l;
		}
		set
		{
			l = value;
		}
	}

	public bool UseUserGroupACTZ
	{
		get
		{
			return m;
		}
		set
		{
			m = value;
		}
	}

	public bool UseUserGroupVM
	{
		get
		{
			return n;
		}
		set
		{
			n = value;
		}
	}

	public int Department
	{
		get
		{
			return o;
		}
		set
		{
			o = value;
		}
	}

	public List<Enroll> Enrolls
	{
		get
		{
			if (p == null)
			{
				p = new List<Enroll>();
			}
			return p;
		}
		set
		{
			p = value;
		}
	}

	public int AccessTimeZone
	{
		get
		{
			return q;
		}
		set
		{
			q = value;
		}
	}

	public DateTime ValidDate
	{
		get
		{
			return r;
		}
		set
		{
			r = value;
		}
	}

	public DateTime InvalidDate
	{
		get
		{
			return s;
		}
		set
		{
			s = value;
		}
	}

	public int UserGroup
	{
		get
		{
			return t;
		}
		set
		{
			t = value;
		}
	}

	public int LockControl
	{
		get
		{
			return u;
		}
		set
		{
			u = value;
		}
	}

	public uint Res
	{
		get
		{
			return v;
		}
		set
		{
			v = value;
		}
	}

	public User()
	{
		g = string.Empty;
		b = 1uL;
		c = string.Empty;
		h = string.Empty;
		t = 0;
		q = 0;
		r = new DateTime(2000, 1, 1);
		s = new DateTime(2099, 12, 31);
		u = 3;
		v = 0u;
	}
}
