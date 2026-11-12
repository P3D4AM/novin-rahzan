using System;
using System.Globalization;

namespace Babel.ParserGenerator;

public class LexLocation : IMerge<LexLocation>, IComparable<LexLocation>, IEquatable<LexLocation>
{
	public readonly int startTokenNumber;

	public readonly int endTokenNumber;

	public LexLocation(LexLocation location)
	{
		startTokenNumber = location.startTokenNumber;
		endTokenNumber = location.endTokenNumber;
	}

	public LexLocation(int startTokenNumber, int endTokenNumber)
	{
		this.startTokenNumber = startTokenNumber;
		this.endTokenNumber = endTokenNumber;
	}

	public override int GetHashCode()
	{
		return (startTokenNumber << 1) ^ endTokenNumber;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as LexLocation);
	}

	public LexLocation Merge(LexLocation last)
	{
		if (this == last || last == null || Contains(last))
		{
			return this;
		}
		if (last.Contains(this))
		{
			return last;
		}
		int num = ((!StartsBefore(last)) ? last.startTokenNumber : startTokenNumber);
		int num2 = ((!EndsAfter(last)) ? last.endTokenNumber : endTokenNumber);
		return new LexLocation(num, num2);
	}

	public static LexLocation Merge(LexLocation lhs, LexLocation rhs)
	{
		if (lhs != null)
		{
			return lhs.Merge(rhs);
		}
		return rhs;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.CurrentCulture, "({0},{1})", startTokenNumber, endTokenNumber);
	}

	public bool Contains(LexLocation value)
	{
		if (startTokenNumber <= value.startTokenNumber)
		{
			return endTokenNumber >= value.endTokenNumber;
		}
		return false;
	}

	public bool Before(LexLocation value)
	{
		if (endTokenNumber < value.startTokenNumber)
		{
			return true;
		}
		return false;
	}

	public bool StartsBefore(LexLocation value)
	{
		if (startTokenNumber < value.startTokenNumber)
		{
			return true;
		}
		return false;
	}

	public bool EndsAfter(LexLocation value)
	{
		if (endTokenNumber > value.endTokenNumber)
		{
			return true;
		}
		return false;
	}

	public bool IsZeroLength()
	{
		return startTokenNumber == endTokenNumber;
	}

	public bool Overlaps(LexLocation value)
	{
		if (endTokenNumber > value.startTokenNumber)
		{
			return value.endTokenNumber > startTokenNumber;
		}
		return false;
	}

	public LexLocation GetFirstTokenLocation()
	{
		if (startTokenNumber + 1 == endTokenNumber)
		{
			return this;
		}
		return new LexLocation(startTokenNumber, startTokenNumber + 1);
	}

	public int CompareTo(LexLocation other)
	{
		if (other == null)
		{
			return 1;
		}
		if (startTokenNumber != other.startTokenNumber)
		{
			return startTokenNumber - other.startTokenNumber;
		}
		return endTokenNumber - other.endTokenNumber;
	}

	public bool Equals(LexLocation other)
	{
		if (other == null)
		{
			return false;
		}
		if (startTokenNumber == other.startTokenNumber)
		{
			return endTokenNumber == other.endTokenNumber;
		}
		return false;
	}
}
