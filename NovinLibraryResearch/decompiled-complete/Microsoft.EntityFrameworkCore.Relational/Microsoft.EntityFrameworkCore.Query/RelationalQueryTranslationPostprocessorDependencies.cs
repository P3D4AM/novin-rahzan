using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalQueryTranslationPostprocessorDependencies
{
	public ISqlExpressionFactory SqlExpressionFactory { get; }

	[EntityFrameworkInternal]
	public RelationalQueryTranslationPostprocessorDependencies([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		SqlExpressionFactory = sqlExpressionFactory;
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlExpressionFactory, "sqlExpressionFactory");
	}

	public RelationalQueryTranslationPostprocessorDependencies With([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		return new RelationalQueryTranslationPostprocessorDependencies(sqlExpressionFactory);
	}
}
