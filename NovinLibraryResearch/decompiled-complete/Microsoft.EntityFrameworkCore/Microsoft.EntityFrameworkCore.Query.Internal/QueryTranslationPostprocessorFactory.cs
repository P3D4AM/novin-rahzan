namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
{
	private readonly QueryTranslationPostprocessorDependencies _dependencies;

	public QueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext)
	{
		return new QueryTranslationPostprocessor(_dependencies);
	}
}
