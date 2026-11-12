using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public class MaterializeCollectionNavigationExpression : Expression, IPrintableExpression
{
	public virtual Expression Subquery { get; }

	public virtual INavigation Navigation { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public override Type Type => Navigation.ClrType;

	public MaterializeCollectionNavigationExpression(Expression subquery, INavigation navigation)
	{
		Subquery = subquery;
		Navigation = navigation;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update(visitor.Visit(Subquery));
	}

	public virtual MaterializeCollectionNavigationExpression Update(Expression subquery)
	{
		if (subquery == Subquery)
		{
			return this;
		}
		return new MaterializeCollectionNavigationExpression(subquery, Navigation);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("MaterializeCollectionNavigation(");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.AppendLine("navigation: " + Navigation.ToDebugString(singleLine: true, includeIndexes: false, "", detailed: false) + ",");
			expressionPrinter.Append("subquery: ");
			expressionPrinter.Visit(Subquery);
		}
	}
}
