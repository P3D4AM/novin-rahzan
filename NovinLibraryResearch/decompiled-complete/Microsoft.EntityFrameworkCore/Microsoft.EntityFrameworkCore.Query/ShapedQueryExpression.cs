using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class ShapedQueryExpression : Expression, IPrintableExpression
{
	public virtual Expression QueryExpression { get; set; }

	public virtual ResultCardinality ResultCardinality { get; set; }

	public virtual Expression ShaperExpression { get; set; }

	public override Type Type
	{
		get
		{
			if (ResultCardinality != 0)
			{
				return ShaperExpression.Type;
			}
			return typeof(IQueryable<>).MakeGenericType(ShaperExpression.Type);
		}
	}

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public ShapedQueryExpression(Expression queryExpression, Expression shaperExpression)
	{
		QueryExpression = queryExpression;
		ShaperExpression = shaperExpression;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		QueryExpression = visitor.Visit(QueryExpression);
		return this;
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("ShapedQueryExpression: ");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.AppendLine("QueryExpression: ");
			using (expressionPrinter.Indent())
			{
				expressionPrinter.Visit(QueryExpression);
			}
			expressionPrinter.AppendLine().Append("ShaperExpression: ");
			using (expressionPrinter.Indent())
			{
				expressionPrinter.Visit(ShaperExpression);
			}
			expressionPrinter.AppendLine();
		}
	}
}
