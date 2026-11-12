using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class InvocationExpressionRemovingExpressionVisitor : ExpressionVisitor
{
	protected override Expression VisitInvocation(InvocationExpression invocationExpression)
	{
		if (!(StripTrivialConversions(invocationExpression.Expression) is LambdaExpression lambdaExpression))
		{
			return base.VisitInvocation(invocationExpression);
		}
		if (!(AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19511", out var isEnabled) && isEnabled))
		{
			return Visit(InlineLambdaExpression(lambdaExpression, invocationExpression.Arguments));
		}
		return InlineLambdaExpression(lambdaExpression, invocationExpression.Arguments);
	}

	private Expression StripTrivialConversions(Expression expression)
	{
		while (expression is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression && expression.Type == unaryExpression.Operand.Type && unaryExpression.Method == null)
		{
			expression = unaryExpression.Operand;
		}
		return expression;
	}

	private Expression InlineLambdaExpression(LambdaExpression lambdaExpression, ReadOnlyCollection<Expression> arguments)
	{
		return new ReplacingExpressionVisitor(((IEnumerable<Expression>)lambdaExpression.Parameters).ToArray(), arguments.ToArray()).Visit(lambdaExpression.Body);
	}
}
