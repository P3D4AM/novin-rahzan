using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class SelectExpressionProjectionApplyingExpressionVisitor : ExpressionVisitor
{
	protected override Expression VisitExtension(Expression node)
	{
		if (node is SelectExpression selectExpression)
		{
			selectExpression.ApplyProjection();
		}
		return base.VisitExtension(node);
	}
}
