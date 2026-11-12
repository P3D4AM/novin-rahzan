using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public class IncludeExpression : Expression, IPrintableExpression
{
	public virtual Expression EntityExpression { get; set; }

	public virtual Expression NavigationExpression { get; set; }

	public virtual INavigation Navigation { get; set; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public override Type Type { get; }

	public IncludeExpression(Expression entityExpression, Expression navigationExpression, INavigation navigation)
	{
		EntityExpression = entityExpression;
		NavigationExpression = navigationExpression;
		Navigation = navigation;
		Type = EntityExpression.Type;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		Expression entityExpression = visitor.Visit(EntityExpression);
		Expression navigationExpression = visitor.Visit(NavigationExpression);
		return Update(entityExpression, navigationExpression);
	}

	public virtual IncludeExpression Update(Expression entityExpression, Expression navigationExpression)
	{
		if (entityExpression == EntityExpression && navigationExpression == NavigationExpression)
		{
			return this;
		}
		return new IncludeExpression(entityExpression, navigationExpression, Navigation);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("IncludeExpression(");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.Visit(EntityExpression);
			expressionPrinter.AppendLine(", ");
			expressionPrinter.Visit(NavigationExpression);
			expressionPrinter.AppendLine(", " + Navigation.Name + ")");
		}
	}
}
