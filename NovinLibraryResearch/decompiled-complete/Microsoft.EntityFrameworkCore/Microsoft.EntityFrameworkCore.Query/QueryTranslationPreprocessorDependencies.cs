using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class QueryTranslationPreprocessorDependencies
{
	public IEvaluatableExpressionFilter EvaluatableExpressionFilter { get; }

	[EntityFrameworkInternal]
	public QueryTranslationPreprocessorDependencies([JetBrains.Annotations.NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(evaluatableExpressionFilter, "evaluatableExpressionFilter");
		EvaluatableExpressionFilter = evaluatableExpressionFilter;
	}

	public QueryTranslationPreprocessorDependencies With([JetBrains.Annotations.NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter)
	{
		return new QueryTranslationPreprocessorDependencies(evaluatableExpressionFilter);
	}
}
