using System;

namespace Stimulsoft.Report;

public class DateTimeRange : Range, IComparable
{
	public DateTime? From;

	public DateTime? To;

	public DateTime FromDate
	{
		get
		{
			if (From.HasValue)
			{
				return From.Value;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime ToDate
	{
		get
		{
			if (To.HasValue)
			{
				return To.Value;
			}
			return DateTime.MaxValue;
		}
	}

	public override string RangeName => "DateTimeRange";

	public override Type RangeType => typeof(DateTime);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is DateTime)
			{
				From = (DateTime)value;
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
			if (value is DateTime)
			{
				To = (DateTime)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is DateTimeRange dateTimeRange) || !From.HasValue || !To.HasValue)
		{
			return -1;
		}
		return (FromDate.Ticks + ToDate.Ticks).CompareTo(dateTimeRange.FromDate.Ticks + dateTimeRange.ToDate.Ticks);
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
				FromObject = DateTime.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = null;
			}
			else
			{
				ToObject = DateTime.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(DateTime value)
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

	public bool Contains(DateTime? value)
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
		string text = (From.HasValue ? $"{From:d}" : "-");
		string text2 = (To.HasValue ? $"{To:d}" : "-");
		return base.FromStrLoc + " " + text + " " + base.ToStrLoc.ToLowerInvariant() + " " + text2;
	}

	public DateTimeRange()
	{
	}

	public DateTimeRange(DateTime from, DateTime to)
	{
		From = from;
		To = to;
	}

	public DateTimeRange(DateTime? from, DateTime? to)
	{
		From = from;
		To = to;
	}
}
