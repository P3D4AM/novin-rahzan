using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class IntegerSequentialValueList : ISequentialValueList<int>, IEnumerable<int>, IEnumerable
{
	private readonly int[] data;

	private readonly int maxValue;

	public int Count => data.Length;

	public int MaxValue => maxValue;

	public IntegerSequentialValueList(int[] data, int maxValue)
	{
		this.data = data;
		this.maxValue = maxValue;
	}

	public IEnumerator<int> GetEnumerator()
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
