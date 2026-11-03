using System;

namespace Stimulsoft.Report;

public class IntRange : Range, IComparable
{
	public int From;

	public int To;

	public override string RangeName => "IntRange";

	public override Type RangeType => typeof(int);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is int)
			{
				From = (int)value;
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
			if (value is int)
			{
				To = (int)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is IntRange intRange))
		{
			return -1;
		}
		return (From + To).CompareTo(intRange.From + intRange.To);
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
				FromObject = int.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0;
			}
			else
			{
				ToObject = int.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(int value)
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

	public IntRange()
	{
	}

	public IntRange(int from, int to)
	{
		From = from;
		To = to;
	}
}
