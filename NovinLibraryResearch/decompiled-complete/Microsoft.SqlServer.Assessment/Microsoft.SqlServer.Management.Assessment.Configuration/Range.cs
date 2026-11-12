using System;
using System.Text;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class Range<T> : IPattern<T> where T : IComparable<T>
{
	public T Left { get; }

	public T Right { get; }

	public bool IncludesLeft { get; }

	public bool IncludesRight { get; }

	public bool IsEmpty
	{
		get
		{
			if (Left == null)
			{
				return Right == null;
			}
			return false;
		}
	}

	public bool IsSingular => object.Equals(Left, Right);

	public Range()
	{
		IncludesLeft = (IncludesRight = true);
	}

	public Range(T singleValue)
	{
		IncludesLeft = (IncludesRight = true);
		Left = (Right = singleValue);
	}

	public Range(T left, T right, bool includesLeft = true, bool includesRight = true)
	{
		Left = left;
		Right = right;
		bool flag = object.Equals(Left, Right);
		IncludesLeft = flag || includesLeft;
		IncludesRight = flag || includesRight;
	}

	public override bool Equals(object obj)
	{
		if (obj is Range<T> other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(Range<T> other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(Left, other.Left) && object.Equals(Right, other.Right) && IncludesLeft == other.IncludesLeft)
		{
			return IncludesRight == other.IncludesRight;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = ((Left != null) ? Left.GetHashCode() : 0);
		num = (num * 397) ^ ((Right != null) ? Right.GetHashCode() : 0);
		num = (num * 397) ^ IncludesLeft.GetHashCode();
		return (num * 397) ^ IncludesRight.GetHashCode();
	}

	public bool IsMatch(T value)
	{
		if (Left == null || (IncludesLeft ? (value.CompareTo(Left) >= 0) : (value.CompareTo(Left) > 0)))
		{
			if (Right != null)
			{
				if (!IncludesRight)
				{
					return value.CompareTo(Right) < 0;
				}
				return value.CompareTo(Right) <= 0;
			}
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		T left;
		if (object.Equals(Left, Right))
		{
			left = Left;
			return ((left != null) ? left.ToString() : null) ?? string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = stringBuilder.Append(IncludesLeft ? '[' : '(');
		left = Left;
		StringBuilder stringBuilder3 = stringBuilder2.Append(((left != null) ? left.ToString() : null) ?? string.Empty).Append(',');
		left = Right;
		stringBuilder3.Append(((left != null) ? left.ToString() : null) ?? string.Empty).Append(IncludesRight ? ']' : ')');
		return stringBuilder.ToString();
	}
}
