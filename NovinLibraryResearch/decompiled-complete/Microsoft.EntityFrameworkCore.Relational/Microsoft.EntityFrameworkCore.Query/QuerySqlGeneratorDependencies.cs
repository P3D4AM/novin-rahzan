using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class QuerySqlGeneratorDependencies
{
	public IRelationalCommandBuilderFactory RelationalCommandBuilderFactory { get; }

	public ISqlGenerationHelper SqlGenerationHelper { get; }

	[EntityFrameworkInternal]
	public QuerySqlGeneratorDependencies([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory relationalCommandBuilderFactory, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalCommandBuilderFactory, "relationalCommandBuilderFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		RelationalCommandBuilderFactory = relationalCommandBuilderFactory;
		SqlGenerationHelper = sqlGenerationHelper;
	}

	public QuerySqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory relationalCommandBuilderFactory)
	{
		return new QuerySqlGeneratorDependencies(relationalCommandBuilderFactory, SqlGenerationHelper);
	}

	public QuerySqlGeneratorDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new QuerySqlGeneratorDependencies(RelationalCommandBuilderFactory, sqlGenerationHelper);
	}
}
