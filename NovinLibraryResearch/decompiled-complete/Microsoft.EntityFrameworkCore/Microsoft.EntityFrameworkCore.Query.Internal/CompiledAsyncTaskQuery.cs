using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CompiledAsyncTaskQuery<TContext, TResult> : CompiledQueryBase<TContext, Task<TResult>> where TContext : DbContext
{
	public CompiledAsyncTaskQuery([JetBrains.Annotations.NotNull] LambdaExpression queryExpression)
		: base(queryExpression)
	{
	}

	public virtual Task<TResult> ExecuteAsync([JetBrains.Annotations.NotNull] TContext context)
	{
		return ExecuteCore(context);
	}

	public virtual Task<TResult> ExecuteAsync([JetBrains.Annotations.NotNull] TContext context, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1)
	{
		return ExecuteCore(context, param1);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken, param1);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2)
	{
		return ExecuteCore(context, param1, param2);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken, param1, param2);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3)
	{
		return ExecuteCore(context, param1, param2, param3);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken, param1, param2, param3);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4)
	{
		return ExecuteCore(context, param1, param2, param3, param4);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken, param1, param2, param3, param4);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4, TParam5>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4, [JetBrains.Annotations.CanBeNull] TParam5 param5)
	{
		return ExecuteCore(context, param1, param2, param3, param4, param5);
	}

	public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4, TParam5>([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.CanBeNull] TParam3 param3, [JetBrains.Annotations.CanBeNull] TParam4 param4, [JetBrains.Annotations.CanBeNull] TParam5 param5, CancellationToken cancellationToken)
	{
		return ExecuteCore(context, cancellationToken, param1, param2, param3, param4, param5);
	}

	protected override Func<QueryContext, Task<TResult>> CreateCompiledQuery(IQueryCompiler queryCompiler, Expression expression)
	{
		return queryCompiler.CreateCompiledAsyncQuery<Task<TResult>>(expression);
	}
}
