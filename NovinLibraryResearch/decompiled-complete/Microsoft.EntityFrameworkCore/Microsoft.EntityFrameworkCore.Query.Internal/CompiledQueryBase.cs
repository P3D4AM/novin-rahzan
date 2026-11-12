using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public abstract class CompiledQueryBase<TContext, TResult> where TContext : DbContext
{
	private sealed class QueryExpressionRewriter : ExpressionVisitor
	{
		private readonly TContext _context;

		private readonly IReadOnlyCollection<ParameterExpression> _parameters;

		public QueryExpressionRewriter(TContext context, IReadOnlyCollection<ParameterExpression> parameters)
		{
			_context = context;
			_parameters = parameters;
		}

		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			if (typeof(TContext).GetTypeInfo().IsAssignableFrom(parameterExpression.Type.GetTypeInfo()))
			{
				return Expression.Constant(_context);
			}
			if (!_parameters.Contains(parameterExpression))
			{
				return parameterExpression;
			}
			return Expression.Parameter(parameterExpression.Type, "__" + parameterExpression.Name);
		}
	}

	private readonly LambdaExpression _queryExpression;

	private Func<QueryContext, TResult> _executor;

	protected CompiledQueryBase([JetBrains.Annotations.NotNull] LambdaExpression queryExpression)
	{
		_queryExpression = queryExpression;
	}

	protected virtual TResult ExecuteCore([JetBrains.Annotations.NotNull] TContext context, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		return ExecuteCore(context, default(CancellationToken), parameters);
	}

	protected virtual TResult ExecuteCore([JetBrains.Annotations.NotNull] TContext context, CancellationToken cancellationToken, [JetBrains.Annotations.NotNull] params object[] parameters)
	{
		Func<QueryContext, TResult> func = EnsureExecutor(context);
		QueryContext queryContext = context.GetService<IQueryContextFactory>().Create();
		queryContext.CancellationToken = cancellationToken;
		for (int i = 0; i < parameters.Length; i++)
		{
			queryContext.AddParameter("__" + _queryExpression.Parameters[i + 1].Name, parameters[i]);
		}
		return func(queryContext);
	}

	protected abstract Func<QueryContext, TResult> CreateCompiledQuery([JetBrains.Annotations.NotNull] IQueryCompiler queryCompiler, [JetBrains.Annotations.NotNull] Expression expression);

	private Func<QueryContext, TResult> EnsureExecutor(TContext context)
	{
		return NonCapturingLazyInitializer.EnsureInitialized(ref _executor, context, _queryExpression, delegate(TContext c, LambdaExpression q)
		{
			IQueryCompiler service = context.GetService<IQueryCompiler>();
			Expression expression = new QueryExpressionRewriter(c, q.Parameters).Visit(q.Body);
			return CreateCompiledQuery(service, expression);
		});
	}
}
