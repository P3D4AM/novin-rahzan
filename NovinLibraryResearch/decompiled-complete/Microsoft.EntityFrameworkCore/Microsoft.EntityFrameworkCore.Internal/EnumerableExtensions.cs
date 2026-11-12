using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

[DebuggerStepThrough]
public static class EnumerableExtensions
{
	private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		private readonly Func<T, T, bool> _func;

		public DynamicEqualityComparer(Func<T, T, bool> func)
		{
			_func = func;
		}

		public bool Equals(T x, T y)
		{
			return _func(x, y);
		}

		public int GetHashCode(T obj)
		{
			return 0;
		}
	}

	public static IOrderedEnumerable<TSource> OrderByOrdinal<TSource>([JetBrains.Annotations.NotNull] this IEnumerable<TSource> source, [JetBrains.Annotations.NotNull] Func<TSource, string> keySelector)
	{
		return source.OrderBy<TSource, string>(keySelector, StringComparer.Ordinal);
	}

	public static IEnumerable<T> Distinct<T>([JetBrains.Annotations.NotNull] this IEnumerable<T> source, [JetBrains.Annotations.NotNull] Func<T, T, bool> comparer) where T : class
	{
		return source.Distinct(new DynamicEqualityComparer<T>(comparer));
	}

	public static string Join([JetBrains.Annotations.NotNull] this IEnumerable<object> source, [JetBrains.Annotations.NotNull] string separator = ", ")
	{
		return string.Join(separator, source);
	}

	public static bool StructuralSequenceEqual<TSource>([JetBrains.Annotations.NotNull] this IEnumerable<TSource> first, [JetBrains.Annotations.NotNull] IEnumerable<TSource> second)
	{
		if (first == second)
		{
			return true;
		}
		using IEnumerator<TSource> enumerator = first.GetEnumerator();
		using IEnumerator<TSource> enumerator2 = second.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator2.MoveNext() || !StructuralComparisons.StructuralEqualityComparer.Equals(enumerator.Current, enumerator2.Current))
			{
				return false;
			}
		}
		return !enumerator2.MoveNext();
	}

	public static bool StartsWith<TSource>([JetBrains.Annotations.NotNull] this IEnumerable<TSource> first, [JetBrains.Annotations.NotNull] IEnumerable<TSource> second)
	{
		if (first == second)
		{
			return true;
		}
		using (IEnumerator<TSource> enumerator2 = first.GetEnumerator())
		{
			using IEnumerator<TSource> enumerator = second.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!enumerator2.MoveNext() || !object.Equals(enumerator2.Current, enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static int IndexOf<T>([JetBrains.Annotations.NotNull] this IEnumerable<T> source, [JetBrains.Annotations.NotNull] T item)
	{
		return source.IndexOf(item, EqualityComparer<T>.Default);
	}

	public static int IndexOf<T>([JetBrains.Annotations.NotNull] this IEnumerable<T> source, [JetBrains.Annotations.NotNull] T item, [JetBrains.Annotations.NotNull] IEqualityComparer<T> comparer)
	{
		return source.Select((T x, int index) => (!comparer.Equals(item, x)) ? (-1) : index).FirstOr((int x) => x != -1, -1);
	}

	public static T FirstOr<T>([JetBrains.Annotations.NotNull] this IEnumerable<T> source, [JetBrains.Annotations.NotNull] T alternate)
	{
		return source.DefaultIfEmpty(alternate).First();
	}

	public static T FirstOr<T>([JetBrains.Annotations.NotNull] this IEnumerable<T> source, [JetBrains.Annotations.NotNull] Func<T, bool> predicate, [JetBrains.Annotations.NotNull] T alternate)
	{
		return source.Where(predicate).FirstOr(alternate);
	}

	public static bool Any([JetBrains.Annotations.NotNull] this IEnumerable source)
	{
		IEnumerator enumerator = source.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				_ = enumerator.Current;
				return true;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}
}
