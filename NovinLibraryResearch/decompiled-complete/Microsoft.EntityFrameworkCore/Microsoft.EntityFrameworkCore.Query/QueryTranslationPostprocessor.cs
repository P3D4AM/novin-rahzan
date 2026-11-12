using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class QueryTranslationPostprocessor
{
	protected virtual QueryTranslationPostprocessorDependencies Dependencies { get; }

	public QueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual Expression Process(Expression query)
	{
		return query;
	}
}
