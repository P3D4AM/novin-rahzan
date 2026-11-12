using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalEvaluatableExpressionFilter : EvaluatableExpressionFilter
{
	protected virtual RelationalEvaluatableExpressionFilterDependencies RelationalDependencies { get; }

	public RelationalEvaluatableExpressionFilter([JetBrains.Annotations.NotNull] EvaluatableExpressionFilterDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalEvaluatableExpressionFilterDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	public override bool IsEvaluatableExpression(Expression expression, IModel model)
	{
		if (expression is MethodCallExpression methodCallExpression && model.FindDbFunction(methodCallExpression.Method) != null)
		{
			return false;
		}
		return base.IsEvaluatableExpression(expression, model);
	}
}
