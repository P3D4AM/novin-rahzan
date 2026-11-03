using System;

namespace Stimulsoft.Report;

public class FloatRange : Range, IComparable
{
	public float From;

	public float To;

	public override string RangeName => "FloatRange";

	public override Type RangeType => typeof(float);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is float)
			{
				From = (float)value;
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
			if (value is float)
			{
				To = (float)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is FloatRange floatRange))
		{
			return -1;
		}
		return (From + To).CompareTo(floatRange.From + floatRange.To);
	}

	public override void Parse(string from, string to)
	{
		try
		{
			if (string.IsNullOrEmpty(from.Trim()))
			{
				FromObject = 0f;
			}
			else
			{
				FromObject = float.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0f;
			}
			else
			{
				ToObject = float.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(float value)
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

	public FloatRange()
	{
	}

	public FloatRange(float from, float to)
	{
		From = from;
		To = to;
	}
}
