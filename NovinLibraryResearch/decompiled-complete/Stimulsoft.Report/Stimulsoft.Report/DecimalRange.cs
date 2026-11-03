using System;

namespace Stimulsoft.Report;

public class DecimalRange : Range, IComparable
{
	public decimal From;

	public decimal To;

	public override string RangeName => "DecimalRange";

	public override Type RangeType => typeof(decimal);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is decimal)
			{
				From = (decimal)value;
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
			if (value is decimal)
			{
				To = (decimal)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is DecimalRange decimalRange))
		{
			return -1;
		}
		return (From + To).CompareTo(decimalRange.From + decimalRange.To);
	}

	public override void Parse(string from, string to)
	{
		try
		{
			if (string.IsNullOrEmpty(from.Trim()))
			{
				FromObject = 0m;
			}
			else
			{
				FromObject = decimal.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0m;
			}
			else
			{
				ToObject = decimal.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(decimal value)
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

	public DecimalRange()
	{
	}

	public DecimalRange(decimal from, decimal to)
	{
		From = from;
		To = to;
	}
}
