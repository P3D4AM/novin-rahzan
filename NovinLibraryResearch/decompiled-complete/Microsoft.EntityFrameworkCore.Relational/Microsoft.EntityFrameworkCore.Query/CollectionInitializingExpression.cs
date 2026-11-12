using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public class CollectionInitializingExpression : Expression, IPrintableExpression
{
	public override Type Type { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public virtual int CollectionId { get; }

	public virtual Expression Parent { get; }

	public virtual Expression ParentIdentifier { get; }

	public virtual Expression OuterIdentifier { get; }

	public virtual INavigation Navigation { get; }

	public CollectionInitializingExpression(int collectionId, Expression parent, Expression parentIdentifier, Expression outerIdentifier, INavigation navigation, Type type)
	{
		CollectionId = collectionId;
		Parent = parent;
		ParentIdentifier = parentIdentifier;
		OuterIdentifier = outerIdentifier;
		Navigation = navigation;
		Type = type;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		Expression expression = visitor.Visit(Parent);
		Expression expression2 = visitor.Visit(ParentIdentifier);
		Expression expression3 = visitor.Visit(OuterIdentifier);
		if (expression == Parent && expression2 == ParentIdentifier && expression3 == OuterIdentifier)
		{
			return this;
		}
		return new CollectionInitializingExpression(CollectionId, expression, expression2, expression3, Navigation, Type);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("InitializeCollection:");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.AppendLine($"CollectionId: {CollectionId}");
			expressionPrinter.AppendLine("Navigation: " + Navigation?.Name);
			expressionPrinter.Append("Parent:");
			expressionPrinter.Visit(Parent);
			expressionPrinter.AppendLine();
			expressionPrinter.Append("ParentIdentifier:");
			expressionPrinter.Visit(ParentIdentifier);
			expressionPrinter.AppendLine();
			expressionPrinter.Append("OuterIdentifier:");
			expressionPrinter.Visit(OuterIdentifier);
			expressionPrinter.AppendLine();
		}
	}
}
