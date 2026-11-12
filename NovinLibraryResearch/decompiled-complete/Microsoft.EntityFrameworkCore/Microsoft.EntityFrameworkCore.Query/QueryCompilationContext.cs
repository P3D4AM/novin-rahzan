using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public class QueryCompilationContext
{
	public static readonly ParameterExpression QueryContextParameter = Expression.Parameter(typeof(QueryContext), "queryContext");

	private readonly IQueryTranslationPreprocessorFactory _queryTranslationPreprocessorFactory;

	private readonly IQueryableMethodTranslatingExpressionVisitorFactory _queryableMethodTranslatingExpressionVisitorFactory;

	private readonly IQueryTranslationPostprocessorFactory _queryTranslationPostprocessorFactory;

	private readonly IShapedQueryCompilingExpressionVisitorFactory _shapedQueryCompilingExpressionVisitorFactory;

	private Dictionary<string, LambdaExpression> _runtimeParameters;

	private static readonly MethodInfo _queryContextAddParameterMethodInfo = typeof(QueryContext).GetTypeInfo().GetDeclaredMethod("AddParameter");

	public virtual bool IsAsync { get; }

	public virtual IModel Model { get; }

	public virtual IDbContextOptions ContextOptions { get; }

	public virtual bool IsTracking { get; internal set; }

	public virtual bool IsBuffering { get; }

	public virtual bool IgnoreQueryFilters { get; internal set; }

	public virtual ISet<string> Tags { get; } = new HashSet<string>();


	public virtual IDiagnosticsLogger<DbLoggerCategory.Query> Logger { get; }

	public virtual Type ContextType { get; }

	public QueryCompilationContext(QueryCompilationContextDependencies dependencies, bool async)
	{
		DbContext context = dependencies.CurrentContext.Context;
		IsAsync = async;
		IsTracking = context.ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.TrackAll;
		IsBuffering = dependencies.IsRetryingExecutionStrategy;
		Model = dependencies.Model;
		ContextOptions = dependencies.ContextOptions;
		ContextType = context.GetType();
		Logger = dependencies.Logger;
		_queryTranslationPreprocessorFactory = dependencies.QueryTranslationPreprocessorFactory;
		_queryableMethodTranslatingExpressionVisitorFactory = dependencies.QueryableMethodTranslatingExpressionVisitorFactory;
		_queryTranslationPostprocessorFactory = dependencies.QueryTranslationPostprocessorFactory;
		_shapedQueryCompilingExpressionVisitorFactory = dependencies.ShapedQueryCompilingExpressionVisitorFactory;
	}

	public virtual void AddTag(string tag)
	{
		Tags.Add(tag);
	}

	public virtual Func<QueryContext, TResult> CreateQueryExecutor<TResult>(Expression query)
	{
		query = _queryTranslationPreprocessorFactory.Create(this).Process(query);
		query = _queryableMethodTranslatingExpressionVisitorFactory.Create(Model).Visit(query);
		query = _queryTranslationPostprocessorFactory.Create(this).Process(query);
		query = _shapedQueryCompilingExpressionVisitorFactory.Create(this).Visit(query);
		query = InsertRuntimeParameters(query);
		Expression<Func<QueryContext, TResult>> expression = Expression.Lambda<Func<QueryContext, TResult>>(query, new ParameterExpression[1] { QueryContextParameter });
		try
		{
			return expression.Compile();
		}
		finally
		{
			Logger.QueryExecutionPlanned(new ExpressionPrinter(), expression);
		}
	}

	public virtual ParameterExpression RegisterRuntimeParameter(string name, LambdaExpression valueExtractor)
	{
		if (valueExtractor.Parameters.Count != 1 || valueExtractor.Parameters[0] != QueryContextParameter)
		{
			throw new ArgumentException("Runtime parameter extraction lambda must have one QueryContext parameter", "valueExtractor");
		}
		if (_runtimeParameters == null)
		{
			_runtimeParameters = new Dictionary<string, LambdaExpression>();
		}
		_runtimeParameters[name] = valueExtractor;
		return Expression.Parameter(valueExtractor.ReturnType, name);
	}

	private Expression InsertRuntimeParameters(Expression query)
	{
		if (_runtimeParameters != null)
		{
			return Expression.Block(_runtimeParameters.Select((KeyValuePair<string, LambdaExpression> kv) => Expression.Call(QueryContextParameter, _queryContextAddParameterMethodInfo, Expression.Constant(kv.Key), Expression.Convert(Expression.Invoke(kv.Value, QueryContextParameter), typeof(object)))).Append(query));
		}
		return query;
	}
}
