using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class RangeList<T> : IPattern<T> where T : IComparable<T>
{
	public List<Range<T>> Ranges { get; }

	public bool IsEmpty => Ranges.TrueForAll((Range<T> i) => i.IsEmpty);

	public RangeList()
	{
		Ranges = new List<Range<T>>();
	}

	public RangeList(IEnumerable<Range<T>> ranges)
	{
		Ranges = new List<Range<T>>(ranges);
	}

	public RangeList(Range<T> head, params Range<T>[] rest)
	{
		if (rest == null)
		{
			throw new ArgumentNullException("rest");
		}
		Ranges = new List<Range<T>>(rest.Length + 1) { head };
		Ranges.AddRange(rest);
	}

	public RangeList(T singleValue)
	{
		Ranges = new List<Range<T>>(1)
		{
			new Range<T>(singleValue)
		};
	}

	public RangeList(T left, T right, bool includesLeft = true, bool includesRight = true)
	{
		Ranges = new List<Range<T>>
		{
			new Range<T>(left, right, includesLeft, includesRight)
		};
	}

	public bool IsMatch(T value)
	{
		if (Ranges.Count != 0)
		{
			return Ranges.Any((Range<T> r) => r.IsMatch(value));
		}
		return true;
	}
}
