namespace Microsoft.EntityFrameworkCore.Query;

public interface IQueryTranslationPreprocessorFactory
{
	QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext);
}
