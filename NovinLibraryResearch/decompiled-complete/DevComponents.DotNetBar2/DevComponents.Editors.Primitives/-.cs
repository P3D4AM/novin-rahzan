using System;

namespace DevComponents.Editors.Primitives;

internal class _255D
{
	private string __2562;

	private int __2563;

	private int __2564;

	public string _255E
	{
		get
		{
			return __2562;
		}
		set
		{
			__2562 = value;
		}
	}

	public int _255F
	{
		get
		{
			return __2563;
		}
		set
		{
			__2563 = value;
		}
	}

	public Predicate<string> _2560 => _2566;

	public Predicate<string> _2561 => _2567;

	public _255D(string prefix)
	{
		__2562 = prefix;
	}

	public _255D(string prefix, int maxMatches)
	{
		__2562 = prefix;
		__2563 = maxMatches;
	}

	public void _2565()
	{
		__2564 = 0;
	}

	private bool _2566(string ٮ)
	{
		return ٮ.ToLower().StartsWith(__2562);
	}

	private bool _2567(string ٮ)
	{
		if (__2564 > __2563)
		{
			return false;
		}
		bool flag = ٮ.ToLower().StartsWith(__2562);
		if (flag && __2563 > 0)
		{
			__2564++;
		}
		return flag;
	}
}
