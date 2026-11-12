using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IEnumerable<TElement>, IEnumerable
{
	private readonly List<TElement> _elements = new List<TElement>();

	public virtual TKey Key { get; }

	public Grouping([JetBrains.Annotations.CanBeNull] TKey key)
	{
		Key = key;
	}

	public virtual void Add([JetBrains.Annotations.CanBeNull] TElement element)
	{
		_elements.Add(element);
	}

	public virtual IEnumerator<TElement> GetEnumerator()
	{
		return _elements.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
