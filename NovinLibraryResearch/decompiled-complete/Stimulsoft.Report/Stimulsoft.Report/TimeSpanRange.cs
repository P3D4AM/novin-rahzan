using System;

namespace Stimulsoft.Report;

public class TimeSpanRange : Range, IComparable
{
	public TimeSpan? From;

	public TimeSpan? To;

	public TimeSpan FromTime
	{
		get
		{
			if (From.HasValue)
			{
				return From.Value;
			}
			return TimeSpan.MinValue;
		}
	}

	public TimeSpan ToTime
	{
		get
		{
			if (To.HasValue)
			{
				return To.Value;
			}
			return TimeSpan.MaxValue;
		}
	}

	public override string RangeName => "TimeSpanRange";

	public override Type RangeType => typeof(TimeSpan);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is TimeSpan)
			{
				From = (TimeSpan)value;
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
			if (value is TimeSpan)
			{
				To = (TimeSpan)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is TimeSpanRange timeSpanRange) || !From.HasValue || !To.HasValue)
		{
			return -1;
		}
		return (FromTime.Ticks + ToTime.Ticks).CompareTo(timeSpanRange.FromTime.Ticks + timeSpanRange.ToTime.Ticks);
	}

	public override void Parse(string from, string to)
	{
		try
		{
			if (string.IsNullOrEmpty(from.Trim()))
			{
				FromObject = null;
			}
			else
			{
				FromObject = TimeSpan.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = null;
			}
			else
			{
				ToObject = TimeSpan.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(TimeSpan value)
	{
		if (!From.HasValue && !To.HasValue)
		{
			return true;
		}
		if (!From.HasValue && To.HasValue)
		{
			return To.Value >= value;
		}
		if (From.HasValue && !To.HasValue)
		{
			return From.Value <= value;
		}
		if (From.Value <= value)
		{
			return To.Value >= value;
		}
		return false;
	}

	public bool Contains(TimeSpan? value)
	{
		if (!From.HasValue && !To.HasValue)
		{
			return true;
		}
		if (!value.HasValue)
		{
			return false;
		}
		if (!From.HasValue && To.HasValue)
		{
			return To.Value >= value.Value;
		}
		if (From.HasValue && !To.HasValue)
		{
			return From.Value <= value.Value;
		}
		if (From.Value <= value.Value)
		{
			return To.Value >= value.Value;
		}
		return false;
	}

	public override string ToString()
	{
		string text = (From.HasValue ? $"{From:t}" : "-");
		string text2 = (To.HasValue ? $"{To:t}" : "-");
		return base.FromStrLoc + " " + text + " " + base.ToStrLoc.ToLowerInvariant() + " " + text2;
	}

	public TimeSpanRange()
	{
	}

	public TimeSpanRange(TimeSpan from, TimeSpan to)
	{
		From = from;
		To = to;
	}

	public TimeSpanRange(TimeSpan? from, TimeSpan? to)
	{
		From = from;
		To = to;
	}
}
