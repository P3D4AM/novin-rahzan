using System;

namespace Stimulsoft.Report;

public class CharRange : Range, IComparable
{
	public char From { get; set; } = 'A';


	public char To { get; set; } = 'Z';


	public override string RangeName => "CharRange";

	public override Type RangeType => typeof(char);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is char)
			{
				From = (char)value;
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
			if (value is char)
			{
				To = (char)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is CharRange charRange))
		{
			return -1;
		}
		return (From + To).CompareTo(charRange.From + charRange.To);
	}

	public override void Parse(string from, string to)
	{
		if (!string.IsNullOrEmpty(from))
		{
			FromObject = from[0];
		}
		if (!string.IsNullOrEmpty(to))
		{
			ToObject = to[0];
		}
	}

	public bool Contains(char value)
	{
		if (From <= value)
		{
			return To >= value;
		}
		return false;
	}

	public override string ToString()
	{
		return $"{base.FromStrLoc} '{From}' {base.ToStrLoc.ToLowerInvariant()} '{To}'";
	}

	public CharRange()
	{
	}

	public CharRange(char from, char to)
	{
		From = from;
		To = to;
	}
}
