using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
{
	private readonly QueryTranslationPostprocessorDependencies _dependencies;

	private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

	public SqlServerQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies)
	{
		_dependencies = dependencies;
		_relationalDependencies = relationalDependencies;
	}

	public virtual QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext)
	{
		return new SqlServerQueryTranslationPostprocessor(_dependencies, _relationalDependencies, queryCompilationContext);
	}
}
