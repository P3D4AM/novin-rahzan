using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlCodeCollection<T> : IEnumerable<T>, IEnumerable where T : SqlCodeObject
{
	int Count { get; }

	bool IsReadOnly { get; }

	T this[int index] { get; }

	bool Contains(T item);

	void CopyTo(T[] array, int arrayIndex);

	int IndexOf(T item);
}
