namespace System.Collections.Generic;

internal static class ListExtensions
{
	public static T AddAndReturn<T>(this IList<T> list, T item)
	{
		list.Add(item);
		return item;
	}
}
