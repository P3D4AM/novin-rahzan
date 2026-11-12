using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CompiledAsyncEnumerableQuery<TContext, TResult> : CompiledQueryBase<TContext, IAsyncEnumerable<TResult>> where TContext : DbContext
{
	public CompiledAsyncEnumerableQuery([JetBrains.Annotations.NotNull] LambdaExpression queryExpression)
		: base(queryExpression)
	{
	}

	public virtual IAsyncEnumerable<TResult> Execute([JetBrains.Annotations.NotNull] TContext context)
	{
		return ExecuteCore(context);
	}

	public virtual IAsyncEnumerable<TResult> Execute<TParam1>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1)
	{
		return ExecuteCore(context, param1);
	}

	public virtual IAsyncEnumerable<TResult> Execute<TParam1, TParam2>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2)
	{
		return ExecuteCore(context, param1, param2);
	}

	public virtual IAsyncEnumerable<TResult> Execute<TParam1, TParam2, TParam3>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3)
	{
		return ExecuteCore(context, param1, param2, param3);
	}

	public virtual IAsyncEnumerable<TResult> Execute<TParam1, TParam2, TParam3, TParam4>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4)
	{
		return ExecuteCore(context, param1, param2, param3, param4);
	}

	public virtual IAsyncEnumerable<TResult> Execute<TParam1, TParam2, TParam3, TParam4, TParam5>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4, [JetBrains.Annotations.CanBeNull] TParam5 param5)
	{
		return ExecuteCore(context, param1, param2, param3, param4, param5);
	}

	protected override Func<QueryContext, IAsyncEnumerable<TResult>> CreateCompiledQuery(IQueryCompiler queryCompiler, Expression expression)
	{
		return queryCompiler.CreateCompiledAsyncQuery<IAsyncEnumerable<TResult>>(expression);
	}
}
