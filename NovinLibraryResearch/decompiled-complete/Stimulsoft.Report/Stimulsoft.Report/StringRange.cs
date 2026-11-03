using System;

namespace Stimulsoft.Report;

public class StringRange : Range, IComparable
{
	public string From = string.Empty;

	public string To = string.Empty;

	public override string RangeName => "StringRange";

	public override Type RangeType => typeof(string);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is string)
			{
				From = (string)value;
			}
		}
	}

	public override object ToObject
	{
		get
		{
			return To;
		}
		set
		{
			if (value is string)
			{
				To = (string)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is StringRange stringRange) || From == null || To == null)
		{
			return -1;
		}
		return (From + To).CompareTo(stringRange.From + stringRange.To);
	}

	public override void Parse(string from, string to)
	{
		FromObject = from;
		ToObject = to;
	}

	public bool Contains(string value)
	{
		if (From == null && To == null)
		{
			return true;
		}
		if (value == null)
		{
			return true;
		}
		if (From == null && To != null)
		{
			return To.CompareTo(value) >= 0;
		}
		if (From != null && To == null)
		{
			return From.CompareTo(value) <= 0;
		}
		if (From.CompareTo(value) <= 0)
		{
			return To.CompareTo(value) >= 0;
		}
		return false;
	}

	public override string ToString()
	{
		return base.FromStrLoc + " " + From + " " + base.ToStrLoc.ToLowerInvariant() + " " + To;
	}

	public StringRange()
	{
	}

	public StringRange(string from, string to)
	{
		From = from;
		To = to;
	}
}
