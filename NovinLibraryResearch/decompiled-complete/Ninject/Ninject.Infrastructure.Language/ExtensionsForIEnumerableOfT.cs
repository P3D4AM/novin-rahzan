using System;
using System.Collections.Generic;
using System.Linq;

namespace Ninject.Infrastructure.Language;

public static class ExtensionsForIEnumerableOfT
{
	public static void Map<T>(this IEnumerable<T> series, Action<T> action)
	{
		foreach (T item in series)
		{
			action(item);
		}
	}

	public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> series)
	{
		return series.Select((T x) => x);
	}

	public static T SingleOrThrowException<T>(this IEnumerable<T> series, Func<ActivationException> exceptionCreator)
	{
		IEnumerator<T> enumerator = series.GetEnumerator();
		enumerator.MoveNext();
		T current = enumerator.Current;
		if (enumerator.MoveNext())
		{
			throw exceptionCreator();
		}
		return current;
	}
}
