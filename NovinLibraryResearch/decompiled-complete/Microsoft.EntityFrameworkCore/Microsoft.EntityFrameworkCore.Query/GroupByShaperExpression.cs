using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class GroupByShaperExpression : Expression, IPrintableExpression
{
	public virtual Expression KeySelector { get; }

	public virtual Expression ElementSelector { get; }

	public override Type Type => typeof(IGrouping<, >).MakeGenericType(KeySelector.Type, ElementSelector.Type);

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public GroupByShaperExpression(Expression keySelector, Expression elementSelector)
	{
		KeySelector = keySelector;
		ElementSelector = elementSelector;
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("GroupByShaperExpression:");
		expressionPrinter.Append("KeySelector: ");
		expressionPrinter.Visit(KeySelector);
		expressionPrinter.AppendLine(", ");
		expressionPrinter.Append("ElementSelector:");
		expressionPrinter.Visit(ElementSelector);
		expressionPrinter.AppendLine();
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		Expression keySelector = visitor.Visit(KeySelector);
		Expression elementSelector = visitor.Visit(ElementSelector);
		return Update(keySelector, elementSelector);
	}

	public virtual GroupByShaperExpression Update(Expression keySelector, Expression elementSelector)
	{
		if (keySelector == KeySelector && elementSelector == ElementSelector)
		{
			return this;
		}
		return new GroupByShaperExpression(keySelector, elementSelector);
	}
}
