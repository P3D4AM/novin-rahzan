using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class QueryCompilationContextDependencies
{
	private readonly IExecutionStrategyFactory _executionStrategyFactory;

	public ICurrentDbContext CurrentContext { get; }

	public IModel Model { get; }

	public IQueryTranslationPreprocessorFactory QueryTranslationPreprocessorFactory { get; }

	public IQueryableMethodTranslatingExpressionVisitorFactory QueryableMethodTranslatingExpressionVisitorFactory { get; }

	public IQueryTranslationPostprocessorFactory QueryTranslationPostprocessorFactory { get; }

	public IShapedQueryCompilingExpressionVisitorFactory ShapedQueryCompilingExpressionVisitorFactory { get; }

	public bool IsRetryingExecutionStrategy { get; }

	public IDbContextOptions ContextOptions { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Query> Logger { get; }

	[EntityFrameworkInternal]
	public QueryCompilationContextDependencies([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IQueryTranslationPreprocessorFactory queryTranslationPreprocessorFactory, [JetBrains.Annotations.NotNull] IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory, [JetBrains.Annotations.NotNull] IQueryTranslationPostprocessorFactory queryTranslationPostprocessorFactory, [JetBrains.Annotations.NotNull] IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDbContextOptions contextOptions, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryTranslationPreprocessorFactory, "queryTranslationPreprocessorFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryableMethodTranslatingExpressionVisitorFactory, "queryableMethodTranslatingExpressionVisitorFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(queryTranslationPostprocessorFactory, "queryTranslationPostprocessorFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(shapedQueryCompilingExpressionVisitorFactory, "shapedQueryCompilingExpressionVisitorFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executionStrategyFactory, "executionStrategyFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextOptions, "contextOptions");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		CurrentContext = currentContext;
		Model = model;
		QueryTranslationPreprocessorFactory = queryTranslationPreprocessorFactory;
		QueryableMethodTranslatingExpressionVisitorFactory = queryableMethodTranslatingExpressionVisitorFactory;
		QueryTranslationPostprocessorFactory = queryTranslationPostprocessorFactory;
		ShapedQueryCompilingExpressionVisitorFactory = shapedQueryCompilingExpressionVisitorFactory;
		_executionStrategyFactory = executionStrategyFactory;
		IsRetryingExecutionStrategy = executionStrategyFactory.Create().RetriesOnFailure;
		ContextOptions = contextOptions;
		Logger = logger;
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IModel model)
	{
		return new QueryCompilationContextDependencies(model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IQueryTranslationPreprocessorFactory queryTranslationPreprocessorFactory)
	{
		return new QueryCompilationContextDependencies(Model, queryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, queryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IQueryTranslationPostprocessorFactory queryTranslationPostprocessorFactory)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, queryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, shapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, executionStrategyFactory, CurrentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, currentContext, ContextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions contextOptions)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, contextOptions, Logger);
	}

	public QueryCompilationContextDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger)
	{
		return new QueryCompilationContextDependencies(Model, QueryTranslationPreprocessorFactory, QueryableMethodTranslatingExpressionVisitorFactory, QueryTranslationPostprocessorFactory, ShapedQueryCompilingExpressionVisitorFactory, _executionStrategyFactory, CurrentContext, ContextOptions, logger);
	}
}
