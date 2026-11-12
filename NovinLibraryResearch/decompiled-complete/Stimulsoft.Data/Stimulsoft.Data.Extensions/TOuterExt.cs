using System;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Data.Extensions;

public static class TOuterExt
{
	public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) where TOuter : class where TInner : class
	{
		ILookup<TKey, TInner> innerLookup = inner.ToLookup(innerKeySelector);
		ILookup<TKey, TOuter> outerLookup = outer.ToLookup(outerKeySelector);
		IEnumerable<TResult> second = from innerItem in inner
			where !outerLookup.Contains(innerKeySelector(innerItem))
			select resultSelector(null, innerItem);
		return outer.SelectMany(delegate(TOuter outerItem)
		{
			IEnumerable<TInner> enumerable = innerLookup[outerKeySelector(outerItem)];
			return (!enumerable.Any()) ? new TInner[1] : enumerable;
		}, resultSelector).Concat(second);
	}
}
