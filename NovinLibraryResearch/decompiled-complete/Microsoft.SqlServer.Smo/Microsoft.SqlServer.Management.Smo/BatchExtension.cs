using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Smo;

internal static class BatchExtension
{
	internal static IEnumerable<string> WithBatches(this IEnumerable<string> script, string batchTerminator)
	{
		foreach (string item in script)
		{
			if (item.EndsWith(Environment.NewLine))
			{
				yield return item + batchTerminator;
				continue;
			}
			yield return item;
			yield return batchTerminator;
		}
	}

	internal static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
	{
		T[] array = new T[batchSize];
		int num = 0;
		foreach (T item in collection)
		{
			array[num++] = item;
			if (num == batchSize)
			{
				yield return array;
				array = new T[batchSize];
				num = 0;
			}
		}
		Array.Resize(ref array, num);
		yield return array;
	}
}
