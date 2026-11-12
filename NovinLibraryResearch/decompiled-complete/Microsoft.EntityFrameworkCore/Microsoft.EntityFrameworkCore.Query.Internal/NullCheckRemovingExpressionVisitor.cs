using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NullCheckRemovingExpressionVisitor : ExpressionVisitor
{
	private class NullSafeAccessVerifyingExpressionVisitor : ExpressionVisitor
	{
		private readonly ISet<Expression> _nullSafeAccesses = new HashSet<Expression>(ExpressionEqualityComparer.Instance);

		public virtual bool Verify(Expression caller, Expression result)
		{
			_nullSafeAccesses.Clear();
			_nullSafeAccesses.Add(caller);
			Visit(result);
			return _nullSafeAccesses.Contains(result);
		}

		public override Expression Visit(Expression expression)
		{
			if (expression != null && !_nullSafeAccesses.Contains(expression))
			{
				return base.Visit(expression);
			}
			return expression;
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			Expression item = Visit(memberExpression.Expression);
			if (_nullSafeAccesses.Contains(item))
			{
				_nullSafeAccesses.Add(memberExpression);
			}
			return memberExpression;
		}

		protected override Expression VisitUnary(UnaryExpression unaryExpression)
		{
			Expression item = Visit(unaryExpression.Operand);
			if ((unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked) && _nullSafeAccesses.Contains(item))
			{
				_nullSafeAccesses.Add(unaryExpression);
			}
			return unaryExpression;
		}
	}

	private readonly NullSafeAccessVerifyingExpressionVisitor _nullSafeAccessVerifyingExpressionVisitor = new NullSafeAccessVerifyingExpressionVisitor();

	protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
	{
		if (Visit(conditionalExpression.Test) is BinaryExpression binaryExpression && (binaryExpression.NodeType == ExpressionType.Equal || binaryExpression.NodeType == ExpressionType.NotEqual))
		{
			bool flag = IsNullConstant(binaryExpression.Left);
			bool flag2 = IsNullConstant(binaryExpression.Right);
			if (flag == flag2 || (binaryExpression.NodeType == ExpressionType.Equal && !IsNullConstant(conditionalExpression.IfTrue)) || (binaryExpression.NodeType == ExpressionType.NotEqual && !IsNullConstant(conditionalExpression.IfFalse)))
			{
				return conditionalExpression;
			}
			Expression caller = (flag ? binaryExpression.Right : binaryExpression.Left);
			Expression result = ((binaryExpression.NodeType == ExpressionType.Equal) ? conditionalExpression.IfFalse : conditionalExpression.IfTrue);
			if (_nullSafeAccessVerifyingExpressionVisitor.Verify(caller, result))
			{
				return result;
			}
		}
		return base.VisitConditional(conditionalExpression);
	}

	private bool IsNullConstant(Expression expression)
	{
		if (expression is ConstantExpression constantExpression)
		{
			return constantExpression.Value == null;
		}
		return false;
	}
}
