using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal struct ReadOnlyArray<T> : IReadOnlyArray<T>, IEnumerable<T>, IEnumerable
{
	private readonly T[] data;

	public int Length => data.Length;

	public T this[int index] => data[index];

	public ReadOnlyArray(T[] data)
	{
		this.data = data;
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < data.Length; i++)
		{
			yield return data[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
