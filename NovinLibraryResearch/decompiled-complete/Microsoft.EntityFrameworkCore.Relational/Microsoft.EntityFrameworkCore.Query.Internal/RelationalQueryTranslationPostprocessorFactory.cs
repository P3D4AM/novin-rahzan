namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class RelationalQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
{
	private readonly QueryTranslationPostprocessorDependencies _dependencies;

	private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

	public RelationalQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies)
	{
		_dependencies = dependencies;
		_relationalDependencies = relationalDependencies;
	}

	public virtual QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext)
	{
		return new RelationalQueryTranslationPostprocessor(_dependencies, _relationalDependencies, queryCompilationContext);
	}
}
