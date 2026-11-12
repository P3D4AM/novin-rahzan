using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalQueryTranslationPostprocessor : QueryTranslationPostprocessor
{
	private readonly SqlExpressionOptimizingExpressionVisitor _sqlExpressionOptimizingExpressionVisitor;

	protected virtual RelationalQueryTranslationPostprocessorDependencies RelationalDependencies { get; }

	protected virtual ISqlExpressionFactory SqlExpressionFactory { get; }

	protected virtual bool UseRelationalNulls { get; }

	public RelationalQueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext)
		: base(dependencies)
	{
		RelationalDependencies = relationalDependencies;
		UseRelationalNulls = RelationalOptionsExtension.Extract(queryCompilationContext.ContextOptions).UseRelationalNulls;
		SqlExpressionFactory = relationalDependencies.SqlExpressionFactory;
		_sqlExpressionOptimizingExpressionVisitor = new SqlExpressionOptimizingExpressionVisitor(SqlExpressionFactory, UseRelationalNulls);
	}

	public override Expression Process(Expression query)
	{
		query = base.Process(query);
		query = new SelectExpressionProjectionApplyingExpressionVisitor().Visit(query);
		query = new CollectionJoinApplyingExpressionVisitor().Visit(query);
		query = new TableAliasUniquifyingExpressionVisitor().Visit(query);
		if (!(AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue12729", out var isEnabled) && isEnabled))
		{
			query = new CaseWhenFlatteningExpressionVisitor(SqlExpressionFactory).Visit(query);
		}
		if (!UseRelationalNulls)
		{
			query = new NullSemanticsRewritingExpressionVisitor(SqlExpressionFactory).Visit(query);
		}
		query = OptimizeSqlExpression(query);
		return query;
	}

	protected virtual Expression OptimizeSqlExpression(Expression query)
	{
		return _sqlExpressionOptimizingExpressionVisitor.Visit(query);
	}
}
