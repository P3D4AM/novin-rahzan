using System;

namespace Stimulsoft.Report;

public class LongRange : Range, IComparable
{
	public long From;

	public long To;

	public override string RangeName => "LongRange";

	public override Type RangeType => typeof(long);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is long)
			{
				From = (long)value;
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
			if (value is long)
			{
				To = (long)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is LongRange longRange))
		{
			return -1;
		}
		return (From + To).CompareTo(longRange.From + longRange.To);
	}

	public override void Parse(string from, string to)
	{
		try
		{
			if (string.IsNullOrEmpty(from.Trim()))
			{
				FromObject = 0;
			}
			else
			{
				FromObject = long.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0;
			}
			else
			{
				ToObject = long.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(long value)
	{
		if (From <= value)
		{
			return To >= value;
		}
		return false;
	}

	public override string ToString()
	{
		return $"{base.FromStrLoc} {From} {base.ToStrLoc.ToLowerInvariant()} {To}";
	}

	public LongRange()
	{
	}

	public LongRange(long from, long to)
	{
		From = from;
		To = to;
	}
}
