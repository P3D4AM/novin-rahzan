using System;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ProjectionExpression : Expression, IPrintableExpression
{
	public virtual string Alias { get; }

	public virtual SqlExpression Expression { get; }

	public override Type Type => Expression.Type;

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public ProjectionExpression(SqlExpression expression, string alias)
	{
		Expression = expression;
		Alias = alias;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((SqlExpression)visitor.Visit(Expression));
	}

	public virtual ProjectionExpression Update(SqlExpression expression)
	{
		if (expression == Expression)
		{
			return this;
		}
		return new ProjectionExpression(expression, Alias);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Visit(Expression);
		if (!string.Equals(string.Empty, Alias) && (!(Expression is ColumnExpression columnExpression) || !string.Equals(columnExpression.Name, Alias)))
		{
			expressionPrinter.Append(" AS " + Alias);
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is ProjectionExpression projectionExpression)
				{
					return Equals(projectionExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(ProjectionExpression projectionExpression)
	{
		if (string.Equals(Alias, projectionExpression.Alias))
		{
			return Expression.Equals(projectionExpression.Expression);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Alias, Expression);
	}
}
