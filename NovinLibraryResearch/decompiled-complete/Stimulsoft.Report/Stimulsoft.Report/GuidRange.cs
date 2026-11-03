using System;

namespace Stimulsoft.Report;

public class GuidRange : Range, IComparable
{
	public Guid From = Guid.Empty;

	public Guid To = Guid.Empty;

	public override string RangeName => "GuidRange";

	public override Type RangeType => typeof(Guid);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is Guid)
			{
				From = (Guid)value;
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
			if (value is Guid)
			{
				To = (Guid)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (obj is GuidRange guidRange)
		{
			_ = From;
			_ = To;
			return (From.ToString() + To).CompareTo(guidRange.From.ToString() + guidRange.To);
		}
		return -1;
	}

	public override void Parse(string from, string to)
	{
		try
		{
			FromObject = new Guid(from);
			ToObject = new Guid(to);
		}
		catch
		{
		}
	}

	public bool Contains(Guid value)
	{
		if (From.CompareTo(value) <= 0)
		{
			return To.CompareTo(value) >= 0;
		}
		return false;
	}

	public override string ToString()
	{
		return $"{base.FromStrLoc} {From} {base.ToStrLoc.ToLowerInvariant()} {To}";
	}

	public GuidRange()
	{
	}

	public GuidRange(Guid from, Guid to)
	{
		From = from;
		To = to;
	}
}
