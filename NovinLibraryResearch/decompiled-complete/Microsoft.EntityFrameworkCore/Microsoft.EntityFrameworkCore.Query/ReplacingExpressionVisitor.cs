using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public class ReplacingExpressionVisitor : ExpressionVisitor
{
	private readonly bool _quirkMode19737;

	private readonly bool _quirkMode19087;

	private readonly Expression[] _originals;

	private readonly Expression[] _replacements;

	private readonly IDictionary<Expression, Expression> _quirkReplacements;

	public static Expression Replace(Expression original, Expression replacement, Expression tree)
	{
		return new ReplacingExpressionVisitor(new Expression[1] { original }, new Expression[1] { replacement }).Visit(tree);
	}

	public ReplacingExpressionVisitor(Expression[] originals, Expression[] replacements)
	{
		_quirkMode19737 = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19737", out var isEnabled) && isEnabled;
		_quirkMode19087 = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19087", out var isEnabled2) && isEnabled2;
		if (_quirkMode19737)
		{
			_quirkReplacements = new Dictionary<Expression, Expression>();
			for (int i = 0; i < originals.Length; i++)
			{
				_quirkReplacements[originals[i]] = replacements[i];
			}
		}
		else
		{
			_originals = originals;
			_replacements = replacements;
		}
	}

	public ReplacingExpressionVisitor(IDictionary<Expression, Expression> replacements)
	{
		_quirkMode19737 = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19737", out var isEnabled) && isEnabled;
		if (_quirkMode19737)
		{
			_quirkReplacements = replacements;
			return;
		}
		_originals = replacements.Keys.ToArray();
		_replacements = replacements.Values.ToArray();
	}

	public override Expression Visit(Expression expression)
	{
		if (expression == null)
		{
			return expression;
		}
		if (_quirkMode19737)
		{
			if (_quirkReplacements.TryGetValue(expression, out var value))
			{
				return value;
			}
		}
		else
		{
			for (int i = 0; i < _originals.Length; i++)
			{
				if (expression.Equals(_originals[i]))
				{
					return _replacements[i];
				}
			}
		}
		return base.Visit(expression);
	}

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		Expression expression = Visit(memberExpression.Expression);
		if (expression is GroupByShaperExpression groupByShaperExpression && memberExpression.Member.Name == "Key")
		{
			return groupByShaperExpression.KeySelector;
		}
		if (expression is NewExpression newExpression)
		{
			int? num = newExpression.Members?.IndexOf(memberExpression.Member);
			if (num >= 0)
			{
				return newExpression.Arguments[num.Value];
			}
		}
		if (_quirkMode19087)
		{
			if (expression is MemberInitExpression memberInitExpression && memberInitExpression.Bindings.SingleOrDefault((MemberBinding mb) => mb.Member == memberExpression.Member) is MemberAssignment memberAssignment)
			{
				return memberAssignment.Expression;
			}
		}
		else if (expression is MemberInitExpression memberInitExpression2 && memberInitExpression2.Bindings.SingleOrDefault((MemberBinding mb) => mb.Member.IsSameAs(memberExpression.Member)) is MemberAssignment memberAssignment2)
		{
			return memberAssignment2.Expression;
		}
		return memberExpression.Update(expression);
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.TryGetEFPropertyArguments(out var entityExpression, out var propertyName))
		{
			Expression expression = Visit(entityExpression);
			if (expression is NewExpression newExpression)
			{
				int? num = newExpression.Members?.Select((MemberInfo m) => m.Name).IndexOf(propertyName);
				if (num >= 0)
				{
					return newExpression.Arguments[num.Value];
				}
			}
			if (expression is MemberInitExpression memberInitExpression && memberInitExpression.Bindings.SingleOrDefault((MemberBinding mb) => mb.Member.Name == propertyName) is MemberAssignment memberAssignment)
			{
				return memberAssignment.Expression;
			}
			return methodCallExpression.Update(null, new Expression[2]
			{
				expression,
				methodCallExpression.Arguments[1]
			});
		}
		return base.VisitMethodCall(methodCallExpression);
	}
}
