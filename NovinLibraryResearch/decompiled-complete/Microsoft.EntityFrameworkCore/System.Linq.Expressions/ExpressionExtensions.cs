using System.Diagnostics;

namespace System.Linq.Expressions;

[DebuggerStepThrough]
internal static class ExpressionExtensions
{
	public static LambdaExpression UnwrapLambdaFromQuote(this Expression expression)
	{
		return (LambdaExpression)((expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Quote) ? unaryExpression.Operand : expression);
	}

	public static Expression UnwrapTypeConversion(this Expression expression, out Type convertedType)
	{
		convertedType = null;
		while (expression is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression)
		{
			expression = unaryExpression.Operand;
			if (unaryExpression.Type != typeof(object) && !unaryExpression.Type.IsAssignableFrom(expression.Type))
			{
				convertedType = unaryExpression.Type;
			}
		}
		return expression;
	}
}
