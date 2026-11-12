namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class RelationalQueryTranslationPreprocessorFactory : IQueryTranslationPreprocessorFactory
{
	private readonly QueryTranslationPreprocessorDependencies _dependencies;

	private readonly RelationalQueryTranslationPreprocessorDependencies _relationalDependencies;

	public RelationalQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies, RelationalQueryTranslationPreprocessorDependencies relationalDependencies)
	{
		_dependencies = dependencies;
		_relationalDependencies = relationalDependencies;
	}

	public virtual QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
	{
		return new RelationalQueryTranslationPreprocessor(_dependencies, _relationalDependencies, queryCompilationContext);
	}
}
