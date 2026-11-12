using System;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryCompiler : IQueryCompiler
{
	private readonly IQueryContextFactory _queryContextFactory;

	private readonly ICompiledQueryCache _compiledQueryCache;

	private readonly ICompiledQueryCacheKeyGenerator _compiledQueryCacheKeyGenerator;

	private readonly IDatabase _database;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

	private readonly Type _contextType;

	private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;

	private readonly IModel _model;

	public QueryCompiler([JetBrains.Annotations.NotNull] IQueryContextFactory queryContextFactory, [JetBrains.Annotations.NotNull] ICompiledQueryCache compiledQueryCache, [JetBrains.Annotations.NotNull] ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator, [JetBrains.Annotations.NotNull] IDatabase database, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter, [JetBrains.Annotations.NotNull] IModel model)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryContextFactory, "queryContextFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(compiledQueryCache, "compiledQueryCache");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(compiledQueryCacheKeyGenerator, "compiledQueryCacheKeyGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(database, "database");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(evaluatableExpressionFilter, "evaluatableExpressionFilter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		_queryContextFactory = queryContextFactory;
		_compiledQueryCache = compiledQueryCache;
		_compiledQueryCacheKeyGenerator = compiledQueryCacheKeyGenerator;
		_database = database;
		_logger = logger;
		_contextType = currentContext.Context.GetType();
		_evaluatableExpressionFilter = evaluatableExpressionFilter;
		_model = model;
	}

	public virtual TResult Execute<TResult>(Expression query)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query");
		QueryContext queryContext = _queryContextFactory.Create();
		query = ExtractParameters(query, queryContext, _logger);
		return _compiledQueryCache.GetOrAddQuery(_compiledQueryCacheKeyGenerator.GenerateCacheKey(query, async: false), () => CompileQueryCore<TResult>(_database, query, _model, async: false))(queryContext);
	}

	public virtual Func<QueryContext, TResult> CompileQueryCore<TResult>(IDatabase database, Expression query, IModel model, bool async)
	{
		return database.CompileQuery<TResult>(query, async);
	}

	public virtual Func<QueryContext, TResult> CreateCompiledQuery<TResult>(Expression query)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query");
		query = ExtractParameters(query, _queryContextFactory.Create(), _logger, parameterize: false);
		return CompileQueryCore<TResult>(_database, query, _model, async: false);
	}

	public virtual TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query");
		QueryContext queryContext = _queryContextFactory.Create();
		queryContext.CancellationToken = cancellationToken;
		query = ExtractParameters(query, queryContext, _logger);
		return _compiledQueryCache.GetOrAddQuery(_compiledQueryCacheKeyGenerator.GenerateCacheKey(query, async: true), () => CompileQueryCore<TResult>(_database, query, _model, async: true))(queryContext);
	}

	public virtual Func<QueryContext, TResult> CreateCompiledAsyncQuery<TResult>(Expression query)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query");
		query = ExtractParameters(query, _queryContextFactory.Create(), _logger, parameterize: false);
		return CompileQueryCore<TResult>(_database, query, _model, async: true);
	}

	public virtual Expression ExtractParameters([JetBrains.Annotations.NotNull] Expression query, [JetBrains.Annotations.NotNull] IParameterValues parameterValues, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize = true, bool generateContextAccessors = false)
	{
		return new ParameterExtractingExpressionVisitor(_evaluatableExpressionFilter, parameterValues, _contextType, _model, logger, parameterize, generateContextAccessors).ExtractParameters(query);
	}
}
