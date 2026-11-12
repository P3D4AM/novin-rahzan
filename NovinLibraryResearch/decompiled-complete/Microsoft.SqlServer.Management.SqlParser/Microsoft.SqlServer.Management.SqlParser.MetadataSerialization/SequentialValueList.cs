using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class SequentialValueList<T> : ISequentialValueList<T>, IEnumerable<T>, IEnumerable
{
	private readonly IList<T> data;

	public int Count => data.Count;

	public SequentialValueList(IList<T> data)
	{
		this.data = data;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
