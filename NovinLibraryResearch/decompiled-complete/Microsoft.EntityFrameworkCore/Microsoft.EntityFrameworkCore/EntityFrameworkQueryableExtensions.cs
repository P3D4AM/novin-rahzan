using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class EntityFrameworkQueryableExtensions
{
	private class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>, IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IAsyncEnumerable<TEntity>
	{
		private readonly IQueryable<TEntity> _queryable;

		public Expression Expression => _queryable.Expression;

		public Type ElementType => _queryable.ElementType;

		public IQueryProvider Provider => _queryable.Provider;

		public IncludableQueryable(IQueryable<TEntity> queryable)
		{
			_queryable = queryable;
		}

		public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);
		}

		public IEnumerator<TEntity> GetEnumerator()
		{
			return _queryable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	internal static readonly MethodInfo IncludeMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethods("Include").Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 2 && mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

	internal static readonly MethodInfo ThenIncludeAfterEnumerableMethodInfo = GetThenIncludeMethodInfo(typeof(IEnumerable<>));

	internal static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethods("ThenInclude").Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 3 && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

	internal static readonly MethodInfo StringIncludeMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethods("Include").Single((MethodInfo mi) => mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType == typeof(string)));

	internal static readonly MethodInfo IgnoreQueryFiltersMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod("IgnoreQueryFilters");

	internal static readonly MethodInfo AsNoTrackingMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod("AsNoTracking");

	internal static readonly MethodInfo AsTrackingMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethods("AsTracking").Single((MethodInfo m) => m.GetParameters().Length == 1);

	internal static readonly MethodInfo TagWithMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod("TagWith");

	public static Task<bool> AnyAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.AnyWithoutPredicate, source, cancellationToken);
	}

	public static Task<bool> AnyAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.AnyWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<bool> AllAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.All, source, predicate, cancellationToken);
	}

	public static Task<int> CountAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<int>>(QueryableMethods.CountWithoutPredicate, source, cancellationToken);
	}

	public static Task<int> CountAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<int>>(QueryableMethods.CountWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<long> LongCountAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<long>>(QueryableMethods.LongCountWithoutPredicate, source, cancellationToken);
	}

	public static Task<long> LongCountAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<long>>(QueryableMethods.LongCountWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> FirstAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> FirstAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> FirstOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstOrDefaultWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> FirstOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstOrDefaultWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> LastAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> LastAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> LastOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastOrDefaultWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> LastOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastOrDefaultWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> SingleAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> SingleAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> SingleOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleOrDefaultWithoutPredicate, source, cancellationToken);
	}

	public static Task<TSource> SingleOrDefaultAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(predicate, "predicate");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleOrDefaultWithPredicate, source, predicate, cancellationToken);
	}

	public static Task<TSource> MinAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.MinWithoutSelector, source, cancellationToken);
	}

	public static Task<TResult> MinAsync<TSource, TResult>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<TResult>>(QueryableMethods.MinWithSelector, source, selector, cancellationToken);
	}

	public static Task<TSource> MaxAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.MaxWithoutSelector, source, cancellationToken);
	}

	public static Task<TResult> MaxAsync<TSource, TResult>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<TResult>>(QueryableMethods.MaxWithSelector, source, selector, cancellationToken);
	}

	public static Task<decimal> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<decimal, Task<decimal>>(QueryableMethods.GetSumWithoutSelector(typeof(decimal)), source, cancellationToken);
	}

	public static Task<decimal?> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<decimal?, Task<decimal?>>(QueryableMethods.GetSumWithoutSelector(typeof(decimal?)), source, cancellationToken);
	}

	public static Task<decimal> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<decimal>>(QueryableMethods.GetSumWithSelector(typeof(decimal)), source, selector, cancellationToken);
	}

	public static Task<decimal?> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<decimal?>>(QueryableMethods.GetSumWithSelector(typeof(decimal?)), source, selector, cancellationToken);
	}

	public static Task<int> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<int, Task<int>>(QueryableMethods.GetSumWithoutSelector(typeof(int)), source, cancellationToken);
	}

	public static Task<int?> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<int?, Task<int?>>(QueryableMethods.GetSumWithoutSelector(typeof(int?)), source, cancellationToken);
	}

	public static Task<int> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<int>>(QueryableMethods.GetSumWithSelector(typeof(int)), source, selector, cancellationToken);
	}

	public static Task<int?> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<int?>>(QueryableMethods.GetSumWithSelector(typeof(int?)), source, selector, cancellationToken);
	}

	public static Task<long> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<long, Task<long>>(QueryableMethods.GetSumWithoutSelector(typeof(long)), source, cancellationToken);
	}

	public static Task<long?> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<long?, Task<long?>>(QueryableMethods.GetSumWithoutSelector(typeof(long?)), source, cancellationToken);
	}

	public static Task<long> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<long>>(QueryableMethods.GetSumWithSelector(typeof(long)), source, selector, cancellationToken);
	}

	public static Task<long?> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<long?>>(QueryableMethods.GetSumWithSelector(typeof(long?)), source, selector, cancellationToken);
	}

	public static Task<double> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<double, Task<double>>(QueryableMethods.GetSumWithoutSelector(typeof(double)), source, cancellationToken);
	}

	public static Task<double?> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<double?, Task<double?>>(QueryableMethods.GetSumWithoutSelector(typeof(double?)), source, cancellationToken);
	}

	public static Task<double> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetSumWithSelector(typeof(double)), source, selector, cancellationToken);
	}

	public static Task<double?> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetSumWithSelector(typeof(double?)), source, selector, cancellationToken);
	}

	public static Task<float> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<float, Task<float>>(QueryableMethods.GetSumWithoutSelector(typeof(float)), source, cancellationToken);
	}

	public static Task<float?> SumAsync([JetBrains.Annotations.NotNull] this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<float?, Task<float?>>(QueryableMethods.GetSumWithoutSelector(typeof(float?)), source, cancellationToken);
	}

	public static Task<float> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<float>>(QueryableMethods.GetSumWithSelector(typeof(float)), source, selector, cancellationToken);
	}

	public static Task<float?> SumAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<float?>>(QueryableMethods.GetSumWithSelector(typeof(float?)), source, selector, cancellationToken);
	}

	public static Task<decimal> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<decimal, Task<decimal>>(QueryableMethods.GetAverageWithoutSelector(typeof(decimal)), source, cancellationToken);
	}

	public static Task<decimal?> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<decimal?, Task<decimal?>>(QueryableMethods.GetAverageWithoutSelector(typeof(decimal?)), source, cancellationToken);
	}

	public static Task<decimal> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<decimal>>(QueryableMethods.GetAverageWithSelector(typeof(decimal)), source, selector, cancellationToken);
	}

	public static Task<decimal?> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<decimal?>>(QueryableMethods.GetAverageWithSelector(typeof(decimal?)), source, selector, cancellationToken);
	}

	public static Task<double> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<int, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(int)), source, cancellationToken);
	}

	public static Task<double?> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<int?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(int?)), source, cancellationToken);
	}

	public static Task<double> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(int)), source, selector, cancellationToken);
	}

	public static Task<double?> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(int?)), source, selector, cancellationToken);
	}

	public static Task<double> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<long, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(long)), source, cancellationToken);
	}

	public static Task<double?> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<long?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(long?)), source, cancellationToken);
	}

	public static Task<double> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(long)), source, selector, cancellationToken);
	}

	public static Task<double?> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(long?)), source, selector, cancellationToken);
	}

	public static Task<double> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<double, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(double)), source, cancellationToken);
	}

	public static Task<double?> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<double?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(double?)), source, cancellationToken);
	}

	public static Task<double> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(double)), source, selector, cancellationToken);
	}

	public static Task<double?> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(double?)), source, selector, cancellationToken);
	}

	public static Task<float> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<float, Task<float>>(QueryableMethods.GetAverageWithoutSelector(typeof(float)), source, cancellationToken);
	}

	public static Task<float?> AverageAsync([JetBrains.Annotations.NotNull] this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<float?, Task<float?>>(QueryableMethods.GetAverageWithoutSelector(typeof(float?)), source, cancellationToken);
	}

	public static Task<float> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<float>>(QueryableMethods.GetAverageWithSelector(typeof(float)), source, selector, cancellationToken);
	}

	public static Task<float?> AverageAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selector, "selector");
		return ExecuteAsync<TSource, Task<float?>>(QueryableMethods.GetAverageWithSelector(typeof(float?)), source, selector, cancellationToken);
	}

	public static Task<bool> ContainsAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] TSource item, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.Contains, source, Expression.Constant(item, typeof(TSource)), cancellationToken);
	}

	public static async Task<List<TSource>> ToListAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<TSource> list = new List<TSource>();
		await foreach (TSource item in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
		{
			list.Add(item);
		}
		return list;
	}

	public static async Task<TSource[]> ToArrayAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (await source.ToListAsync(cancellationToken)).ToArray();
	}

	public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, [JetBrains.Annotations.NotNull] Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationPropertyPath, "navigationPropertyPath");
		return new IncludableQueryable<TEntity, TProperty>((source.Provider is EntityQueryProvider) ? source.Provider.CreateQuery<TEntity>(Expression.Call(null, IncludeMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)), new Expression[2]
		{
			source.Expression,
			Expression.Quote(navigationPropertyPath)
		})) : source);
	}

	private static MethodInfo GetThenIncludeMethodInfo(Type navType)
	{
		return (from mi in typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethods("ThenInclude")
			where mi.GetGenericArguments().Count() == 3
			select mi).Single(delegate(MethodInfo mi)
		{
			TypeInfo typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].GetTypeInfo();
			return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == navType;
		});
	}

	public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>([JetBrains.Annotations.NotNull] this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, [JetBrains.Annotations.NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
	{
		object queryable;
		if (!(source.Provider is EntityQueryProvider))
		{
			queryable = source;
		}
		else
		{
			queryable = source.Provider.CreateQuery<TEntity>(Expression.Call(null, ThenIncludeAfterEnumerableMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)), new Expression[2]
			{
				source.Expression,
				Expression.Quote(navigationPropertyPath)
			}));
		}
		return new IncludableQueryable<TEntity, TProperty>((IQueryable<TEntity>)queryable);
	}

	public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>([JetBrains.Annotations.NotNull] this IIncludableQueryable<TEntity, TPreviousProperty> source, [JetBrains.Annotations.NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
	{
		object queryable;
		if (!(source.Provider is EntityQueryProvider))
		{
			queryable = source;
		}
		else
		{
			queryable = source.Provider.CreateQuery<TEntity>(Expression.Call(null, ThenIncludeAfterReferenceMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)), new Expression[2]
			{
				source.Expression,
				Expression.Quote(navigationPropertyPath)
			}));
		}
		return new IncludableQueryable<TEntity, TProperty>((IQueryable<TEntity>)queryable);
	}

	public static IQueryable<TEntity> Include<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, [JetBrains.Annotations.NotNull][NotParameterized] string navigationPropertyPath) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationPropertyPath, "navigationPropertyPath");
		if (!(source.Provider is EntityQueryProvider))
		{
			return source;
		}
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, StringIncludeMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression, Expression.Constant(navigationPropertyPath)));
	}

	public static IQueryable<TEntity> IgnoreQueryFilters<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		if (!(source.Provider is EntityQueryProvider))
		{
			return source;
		}
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, IgnoreQueryFiltersMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression));
	}

	public static IQueryable<TEntity> AsNoTracking<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		if (!(source.Provider is EntityQueryProvider))
		{
			return source;
		}
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, AsNoTrackingMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression));
	}

	public static IQueryable<TEntity> AsTracking<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		if (!(source.Provider is EntityQueryProvider))
		{
			return source;
		}
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, AsTrackingMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression));
	}

	public static IQueryable<TEntity> AsTracking<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, QueryTrackingBehavior track) where TEntity : class
	{
		if (track != 0)
		{
			return source.AsNoTracking();
		}
		return source.AsTracking();
	}

	public static IQueryable<T> TagWith<T>([JetBrains.Annotations.NotNull] this IQueryable<T> source, [JetBrains.Annotations.NotNull][NotParameterized] string tag)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(tag, "tag");
		if (!(source.Provider is EntityQueryProvider))
		{
			return source;
		}
		return source.Provider.CreateQuery<T>(Expression.Call(null, TagWithMethodInfo.MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(tag)));
	}

	public static void Load<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		while (enumerator.MoveNext())
		{
		}
	}

	public static async Task LoadAsync<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		await using IAsyncEnumerator<TSource> enumerator = source.AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
		while (await enumerator.MoveNextAsync())
		{
		}
	}

	public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default(CancellationToken))
	{
		return source.ToDictionaryAsync(keySelector, (TSource e) => e, null, cancellationToken);
	}

	public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Func<TSource, TKey> keySelector, [JetBrains.Annotations.NotNull] IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
	{
		return source.ToDictionaryAsync(keySelector, (TSource e) => e, comparer, cancellationToken);
	}

	public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Func<TSource, TKey> keySelector, [JetBrains.Annotations.NotNull] Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
	{
		return source.ToDictionaryAsync(keySelector, elementSelector, null, cancellationToken);
	}

	public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source, [JetBrains.Annotations.NotNull] Func<TSource, TKey> keySelector, [JetBrains.Annotations.NotNull] Func<TSource, TElement> elementSelector, [JetBrains.Annotations.NotNull] IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keySelector, "keySelector");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(elementSelector, "elementSelector");
		Dictionary<TKey, TElement> d = new Dictionary<TKey, TElement>(comparer);
		await foreach (TSource item in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
		{
			d.Add(keySelector(item), elementSelector(item));
		}
		return d;
	}

	public static async Task ForEachAsync<T>([JetBrains.Annotations.NotNull] this IQueryable<T> source, [JetBrains.Annotations.NotNull] Action<T> action, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(action, "action");
		await foreach (T item in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
		{
			action(item);
		}
	}

	public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>([JetBrains.Annotations.NotNull] this IQueryable<TSource> source)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		if (source is IAsyncEnumerable<TSource> result)
		{
			return result;
		}
		throw new InvalidOperationException(CoreStrings.IQueryableNotAsync(typeof(TSource)));
	}

	private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo, IQueryable<TSource> source, Expression expression, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (source.Provider is IAsyncQueryProvider asyncQueryProvider)
		{
			if (operatorMethodInfo.IsGenericMethod)
			{
				operatorMethodInfo = ((operatorMethodInfo.GetGenericArguments().Length == 2) ? operatorMethodInfo.MakeGenericMethod(typeof(TSource), typeof(TResult).GetGenericArguments().Single()) : operatorMethodInfo.MakeGenericMethod(typeof(TSource)));
			}
			return asyncQueryProvider.ExecuteAsync<TResult>(Expression.Call(null, operatorMethodInfo, (expression != null) ? new Expression[2] { source.Expression, expression } : new Expression[1] { source.Expression }), cancellationToken);
		}
		throw new InvalidOperationException(CoreStrings.IQueryableProviderNotAsync);
	}

	private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo, IQueryable<TSource> source, LambdaExpression expression, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExecuteAsync<TSource, TResult>(operatorMethodInfo, source, Expression.Quote(expression), cancellationToken);
	}

	private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo, IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ExecuteAsync<TSource, TResult>(operatorMethodInfo, source, (Expression)null, cancellationToken);
	}
}
