using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

[Obsolete("Use ConditionalExpression with null check instead")]
public class NullConditionalExpression : Expression, IPrintableExpression
{
	public virtual Expression Caller { get; }

	public virtual Expression AccessOperation { get; }

	public override bool CanReduce => true;

	public override Type Type { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public NullConditionalExpression([JetBrains.Annotations.NotNull] Expression caller, [JetBrains.Annotations.NotNull] Expression accessOperation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(caller, "caller");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(accessOperation, "accessOperation");
		Caller = caller;
		AccessOperation = accessOperation;
		Type = (accessOperation.Type.IsNullableType() ? accessOperation.Type : accessOperation.Type.MakeNullable());
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update(visitor.Visit(Caller), visitor.Visit(AccessOperation));
	}

	public override Expression Reduce()
	{
		Type type = Caller.Type;
		ParameterExpression parameterExpression = Expression.Parameter(type, "__caller");
		ParameterExpression parameterExpression2 = Expression.Parameter(Type, "__result");
		Expression replacement = ((Caller.Type != parameterExpression.Type) ? ((Expression)Expression.Convert(parameterExpression, Caller.Type)) : ((Expression)parameterExpression));
		Expression expression = ReplacingExpressionVisitor.Replace(Caller, replacement, AccessOperation);
		if (expression.Type != Type)
		{
			expression = Expression.Convert(expression, Type);
		}
		return Expression.Block(new ParameterExpression[2] { parameterExpression, parameterExpression2 }, Expression.Assign(parameterExpression, Caller), Expression.Assign(parameterExpression2, Expression.Default(Type)), Expression.IfThen(Expression.NotEqual(parameterExpression, Expression.Default(type)), Expression.Assign(parameterExpression2, expression)), parameterExpression2);
	}

	public virtual Expression Update(Expression newCaller, Expression newAccessOperation)
	{
		if (newCaller == Caller && (newAccessOperation == AccessOperation || ExpressionEqualityComparer.Instance.Equals((newAccessOperation as NullConditionalExpression)?.AccessOperation, AccessOperation)))
		{
			return this;
		}
		return new NullConditionalExpression(newCaller, newAccessOperation);
	}

	protected virtual bool Equals([JetBrains.Annotations.CanBeNull] NullConditionalExpression other)
	{
		return object.Equals(AccessOperation, other?.AccessOperation);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (obj != this)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((NullConditionalExpression)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return AccessOperation.GetHashCode();
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		if (AccessOperation is MemberExpression memberExpression)
		{
			expressionPrinter.Visit(Caller);
			expressionPrinter.Append("?." + memberExpression.Member.Name);
			return;
		}
		if (AccessOperation is MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Object != null)
			{
				expressionPrinter.Visit(Caller);
				expressionPrinter.Append("?." + methodCallExpression.Method.Name + "(");
				VisitArguments(expressionPrinter, methodCallExpression.Arguments);
				expressionPrinter.Append(")");
				return;
			}
			if (methodCallExpression.TryGetEFPropertyArguments(out var _, out var propertyName))
			{
				MethodInfo method = methodCallExpression.Method;
				expressionPrinter.Append(method.DeclaringType?.Name + "." + method.Name + "(?");
				expressionPrinter.Visit(Caller);
				expressionPrinter.Append("?, ");
				expressionPrinter.Visit(Expression.Constant(propertyName));
				expressionPrinter.Append(")");
				return;
			}
		}
		expressionPrinter.Append("?");
		expressionPrinter.Visit(Caller);
		expressionPrinter.Append(" | ");
		expressionPrinter.Visit(AccessOperation);
		expressionPrinter.Append("?");
	}

	private static void VisitArguments(ExpressionPrinter expressionPrinter, IReadOnlyList<Expression> arguments)
	{
		for (int i = 0; i < arguments.Count; i++)
		{
			expressionPrinter.Visit(arguments[i]);
			expressionPrinter.Append((i == arguments.Count - 1) ? "" : ", ");
		}
	}

	public override string ToString()
	{
		if (AccessOperation is MemberExpression memberExpression)
		{
			return Caller?.ToString() + "?." + memberExpression.Member.Name;
		}
		if (AccessOperation is MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Object != null)
			{
				return Caller?.ToString() + "?." + methodCallExpression.Method.Name + "(" + string.Join(",", methodCallExpression.Arguments) + ")";
			}
			if (methodCallExpression.TryGetEFPropertyArguments(out var _, out var propertyName))
			{
				MethodInfo method = methodCallExpression.Method;
				return method.DeclaringType?.Name + "." + method.Name + "(?" + Caller?.ToString() + "?, " + propertyName + ")";
			}
		}
		return $"?{Caller} | {AccessOperation}?";
	}
}
