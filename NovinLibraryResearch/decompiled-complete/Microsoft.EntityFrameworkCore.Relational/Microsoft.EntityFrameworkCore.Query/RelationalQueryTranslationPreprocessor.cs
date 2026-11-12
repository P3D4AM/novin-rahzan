namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalQueryTranslationPreprocessor : QueryTranslationPreprocessor
{
	protected virtual RelationalQueryTranslationPreprocessorDependencies RelationalDependencies { get; }

	public RelationalQueryTranslationPreprocessor(QueryTranslationPreprocessorDependencies dependencies, RelationalQueryTranslationPreprocessorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext)
		: base(dependencies, queryCompilationContext)
	{
		RelationalDependencies = relationalDependencies;
	}
}
