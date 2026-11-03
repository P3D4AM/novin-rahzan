using System;

namespace Stimulsoft.Report;

public class ByteRange : Range, IComparable
{
	public byte From;

	public byte To;

	public override string RangeName => "ByteRange";

	public override Type RangeType => typeof(byte);

	public override object FromObject
	{
		get
		{
			return From;
		}
		set
		{
			if (value is byte)
			{
				From = (byte)value;
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
			if (value is byte)
			{
				To = (byte)value;
			}
		}
	}

	int IComparable.CompareTo(object obj)
	{
		if (!(obj is ByteRange byteRange))
		{
			return -1;
		}
		return (From + To).CompareTo(byteRange.From + byteRange.To);
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
				FromObject = byte.Parse(from);
			}
			if (string.IsNullOrEmpty(to.Trim()))
			{
				ToObject = 0;
			}
			else
			{
				ToObject = byte.Parse(to);
			}
		}
		catch
		{
		}
	}

	public bool Contains(byte value)
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

	public ByteRange()
	{
	}

	public ByteRange(byte from, byte to)
	{
		From = from;
		To = to;
	}
}
