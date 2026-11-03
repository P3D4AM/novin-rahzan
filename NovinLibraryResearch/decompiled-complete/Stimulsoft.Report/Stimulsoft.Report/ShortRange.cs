using System;

namespace Stimulsoft.Report;

public class ShortRange : Range, IComparable
{
	public short From;

	public short To;

	public override string RangeName => "ShortRange";

	public override Type RangeType => typeof(short);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is short)
			{
				From = (short)value;
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
			if (value is short)
			{
				To = (short)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is ShortRange shortRange))
		{
			return -1;
		}
		return (From + To).CompareTo(shortRange.From + shortRange.To);
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
				FromObject = short.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0;
			}
			else
			{
				ToObject = short.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(short value)
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

	public ShortRange()
	{
	}

	public ShortRange(short from, short to)
	{
		From = from;
		To = to;
	}
}
