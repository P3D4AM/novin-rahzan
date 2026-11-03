using System;

namespace Stimulsoft.Report;

public class DoubleRange : Range, IComparable
{
	public double From;

	public double To;

	public override string RangeName => "DoubleRange";

	public override Type RangeType => typeof(double);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is double)
			{
				From = (double)value;
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
			if (value is double)
			{
				To = (double)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is DoubleRange doubleRange))
		{
			return -1;
		}
		return (From + To).CompareTo(doubleRange.From + doubleRange.To);
	}

	public override void Parse(string from, string to)
	{
		try
		{
			if (string.IsNullOrEmpty(from.Trim()))
			{
				FromObject = 0.0;
			}
			else
			{
				FromObject = double.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0.0;
			}
			else
			{
				ToObject = double.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(double value)
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

	public DoubleRange()
	{
	}

	public DoubleRange(double from, double to)
	{
		From = from;
		To = to;
	}
}
