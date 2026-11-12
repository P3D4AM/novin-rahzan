using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class PatternList<T> : IPattern<T>, IEquatable<PatternList<T>>, IEnumerable<IPattern<T>>, IEnumerable where T : IComparable<T>
{
	private List<IPattern<T>> Rep { get; }

	public PatternList()
	{
		Rep = new List<IPattern<T>>();
	}

	public PatternList(params IPattern<T>[] items)
	{
		Rep = new List<IPattern<T>>(items);
	}

	public PatternList(IEnumerable<IPattern<T>> items)
	{
		Rep = new List<IPattern<T>>(items);
	}

	public bool IsMatch(T value)
	{
		return Rep.Any((IPattern<T> i) => i.IsMatch(value));
	}

	public bool Equals(PatternList<T> other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return Rep.SequenceEqual(other.Rep);
	}

	public IEnumerator<IPattern<T>> GetEnumerator()
	{
		return Rep.GetEnumerator();
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((PatternList<T>)obj);
	}

	public override int GetHashCode()
	{
		return Rep.GetHashCode();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)Rep).GetEnumerator();
	}

	public static bool operator ==(PatternList<T> left, PatternList<T> right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(PatternList<T> left, PatternList<T> right)
	{
		return !object.Equals(left, right);
	}
}
