using System;
using System.Linq.Expressions;

namespace EntityCache;

internal class TransformVisitor<TConcrete, TInterface> : ExpressionVisitor
{
	private readonly ParameterExpression _param = Expression.Parameter(typeof(TConcrete), "param_0");

	public static Expression<Func<TConcrete, bool>> Transform(Expression expression)
	{
		return (Expression<Func<TConcrete, bool>>)new TransformVisitor<TConcrete, TInterface>().Visit(expression);
	}

	protected override Expression VisitLambda<T>(Expression<T> node)
	{
		if (typeof(T).IsAssignableFrom(typeof(Func<TInterface, bool>)))
		{
			return Expression.Lambda<Func<TConcrete, bool>>(Visit(node.Body), new ParameterExpression[1] { _param });
		}
		return base.VisitLambda(node);
	}

	protected override Expression VisitMember(MemberExpression node)
	{
		if (node.Member.DeclaringType.IsAssignableFrom(typeof(TInterface)))
		{
			return Expression.MakeMemberAccess(Visit(node.Expression), typeof(TConcrete).GetProperty(node.Member.Name));
		}
		return base.VisitMember(node);
	}

	protected override Expression VisitParameter(ParameterExpression node)
	{
		if (node.Type.IsAssignableFrom(typeof(TInterface)))
		{
			return _param;
		}
		return base.VisitParameter(node);
	}
}
