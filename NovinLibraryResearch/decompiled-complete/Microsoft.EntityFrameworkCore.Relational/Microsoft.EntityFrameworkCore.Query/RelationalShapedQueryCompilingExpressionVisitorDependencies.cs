using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalShapedQueryCompilingExpressionVisitorDependencies
{
	public IQuerySqlGeneratorFactory QuerySqlGeneratorFactory { get; }

	public ISqlExpressionFactory SqlExpressionFactory { get; }

	public IParameterNameGeneratorFactory ParameterNameGeneratorFactory { get; }

	[EntityFrameworkInternal]
	public RelationalShapedQueryCompilingExpressionVisitorDependencies([JetBrains.Annotations.NotNull] IQuerySqlGeneratorFactory querySqlGeneratorFactory, [JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory, [JetBrains.Annotations.NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(querySqlGeneratorFactory, "querySqlGeneratorFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlExpressionFactory, "sqlExpressionFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterNameGeneratorFactory, "parameterNameGeneratorFactory");
		QuerySqlGeneratorFactory = querySqlGeneratorFactory;
		SqlExpressionFactory = sqlExpressionFactory;
		ParameterNameGeneratorFactory = parameterNameGeneratorFactory;
	}

	public RelationalShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IQuerySqlGeneratorFactory querySqlGeneratorFactory)
	{
		return new RelationalShapedQueryCompilingExpressionVisitorDependencies(querySqlGeneratorFactory, SqlExpressionFactory, ParameterNameGeneratorFactory);
	}

	public RelationalShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		return new RelationalShapedQueryCompilingExpressionVisitorDependencies(QuerySqlGeneratorFactory, sqlExpressionFactory, ParameterNameGeneratorFactory);
	}

	public RelationalShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory)
	{
		return new RelationalShapedQueryCompilingExpressionVisitorDependencies(QuerySqlGeneratorFactory, SqlExpressionFactory, parameterNameGeneratorFactory);
	}
}
