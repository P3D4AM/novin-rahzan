namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryTranslationPreprocessorFactory : IQueryTranslationPreprocessorFactory
{
	private readonly QueryTranslationPreprocessorDependencies _dependencies;

	public QueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
	{
		return new QueryTranslationPreprocessor(_dependencies, queryCompilationContext);
	}
}
