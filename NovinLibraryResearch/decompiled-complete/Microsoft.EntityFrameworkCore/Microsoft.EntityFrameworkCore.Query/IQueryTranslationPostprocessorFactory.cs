namespace Microsoft.EntityFrameworkCore.Query;

public interface IQueryTranslationPostprocessorFactory
{
	QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext);
}
