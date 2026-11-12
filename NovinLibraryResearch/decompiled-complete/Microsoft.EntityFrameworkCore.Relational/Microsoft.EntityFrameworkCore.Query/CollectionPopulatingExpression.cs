using System;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class CollectionPopulatingExpression : Expression, IPrintableExpression
{
	public override Type Type { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public virtual RelationalCollectionShaperExpression Parent { get; }

	public virtual bool IsInclude { get; }

	public CollectionPopulatingExpression(RelationalCollectionShaperExpression parent, Type type, bool include)
	{
		Parent = parent;
		Type = type;
		IsInclude = include;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		RelationalCollectionShaperExpression relationalCollectionShaperExpression = (RelationalCollectionShaperExpression)visitor.Visit(Parent);
		if (relationalCollectionShaperExpression == Parent)
		{
			return this;
		}
		return new CollectionPopulatingExpression(relationalCollectionShaperExpression, Type, IsInclude);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("PopulateCollection:");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.Append("Parent:");
			expressionPrinter.Visit(Parent);
		}
	}
}
