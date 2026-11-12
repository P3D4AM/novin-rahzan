using System.Collections.Generic;

namespace Babel.ParserGenerator;

internal static class MinimalListUtils
{
	public static int BinarySearch<T>(IMinimalList<T> list, T item, IComparer<T> comparer, int startIndex, int endIndex)
	{
		while (startIndex < endIndex)
		{
			int num = (startIndex + endIndex) / 2;
			int num2 = comparer.Compare(item, list[num]);
			if (num2 < 0)
			{
				endIndex = num;
				continue;
			}
			if (num2 > 0)
			{
				startIndex = num + 1;
				continue;
			}
			return num;
		}
		return ~startIndex;
	}

	public static int BinarySearch<T>(IMinimalList<T> list, T item, IComparer<T> comparer)
	{
		return BinarySearch(list, item, comparer, 0, list.Count);
	}

	public static int FirstEqualOrGreater<T>(IMinimalList<T> list, T item, IComparer<T> comparer)
	{
		int num = BinarySearch(list, item, comparer);
		if (num >= 0)
		{
			while (num > 0 && comparer.Compare(item, list[num - 1]) == 0)
			{
				num--;
			}
			return num;
		}
		return ~num;
	}
}
