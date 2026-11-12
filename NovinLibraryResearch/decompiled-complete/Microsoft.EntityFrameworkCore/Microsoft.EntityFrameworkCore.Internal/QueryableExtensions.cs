using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class QueryableExtensions
{
	internal static readonly MethodInfo LeftJoinMethodInfo = typeof(QueryableExtensions).GetTypeInfo().GetDeclaredMethods("LeftJoin").Single((MethodInfo mi) => mi.GetParameters().Length == 5);

	public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
	{
		throw new NotImplementedException();
	}
}
