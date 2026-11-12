using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment;

internal static class AlgebraicUtil
{
	public static IEnumerable<int[]> GetPermutations(IReadOnlyList<int> src)
	{
		if (src.Contains(0))
		{
			yield break;
		}
		int total = src.Aggregate(1, (int current, int t) => current * t);
		for (int i = 0; i < total; i++)
		{
			int[] array = new int[src.Count];
			int a = i;
			for (int j = 0; j < array.Length; j++)
			{
				a = Math.DivRem(a, src[j], out array[j]);
			}
			yield return array;
		}
	}

	public static IEnumerable<TBasket> GetCombinations<TBasket, TItem>(IReadOnlyList<IReadOnlyList<TItem>> columns, Func<TBasket, TItem, TBasket> aggregator) where TBasket : new()
	{
		List<int[]> list = (from a in GetPermutations(columns.Select((IReadOnlyList<TItem> c) => c.Count).ToList())
			select (int[])a.Clone()).ToList();
		foreach (int[] item in list)
		{
			yield return item.Select((int j, int i) => columns[i][j]).Aggregate(new TBasket(), aggregator);
		}
	}
}
