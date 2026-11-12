using System;
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

namespace Microsoft.EntityFrameworkCore;

public static class EF
{
	internal static readonly MethodInfo PropertyMethod = typeof(EF).GetTypeInfo().GetDeclaredMethod("Property");

	public static DbFunctions Functions { get; } = new DbFunctions();


	public static Func<TContext, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, DbSet<TResult>>> queryExpression) where TContext : DbContext where TResult : class
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	[Obsolete("Use DbSet instead")]
	public static Func<TContext, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, DbQuery<TResult>>> queryExpression) where TContext : DbContext where TResult : class
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncEnumerableQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, Task<TResult>> CompileAsyncQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, CancellationToken, Task<TResult>> CompileAsyncQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, CancellationToken, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledAsyncTaskQuery<TContext, TResult>(queryExpression).ExecuteAsync;
	}

	public static Func<TContext, IEnumerable<TResult>> CompileQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, DbSet<TResult>>> queryExpression) where TContext : DbContext where TResult : class
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	[Obsolete("Use DbSet instead")]
	public static Func<TContext, IEnumerable<TResult>> CompileQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, DbQuery<TResult>>> queryExpression) where TContext : DbContext where TResult : class
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, IEnumerable<TResult>> CompileQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, IEnumerable<TResult>> CompileQuery<TContext, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TResult> CompileQuery<TContext, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TResult> CompileQuery<TContext, TParam1, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TResult> CompileQuery<TContext, TParam1, TParam2, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TResult> CompileQuery<TContext, TParam1, TParam2, TParam3, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TResult> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IQueryable<TResult>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult, TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, IIncludableQueryable<TResult, TProperty>>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, IEnumerable<TResult>>(queryExpression).Execute;
	}

	public static Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult> CompileQuery<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>([JetBrains.Annotations.NotNull] Expression<Func<TContext, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>> queryExpression) where TContext : DbContext
	{
		return new CompiledQuery<TContext, TResult>(queryExpression).Execute;
	}

	public static TProperty Property<TProperty>([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull][NotParameterized] string propertyName)
	{
		throw new InvalidOperationException(CoreStrings.PropertyMethodInvoked);
	}
}
