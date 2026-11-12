using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public class EntityShaperExpression : Expression, IPrintableExpression
{
	public virtual IEntityType EntityType { get; }

	public virtual Expression ValueBufferExpression { get; }

	public virtual bool IsNullable { get; }

	public override Type Type => EntityType.ClrType;

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public EntityShaperExpression(IEntityType entityType, Expression valueBufferExpression, bool nullable)
	{
		EntityType = entityType;
		ValueBufferExpression = valueBufferExpression;
		IsNullable = nullable;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		Expression valueBufferExpression = visitor.Visit(ValueBufferExpression);
		return Update(valueBufferExpression);
	}

	public virtual EntityShaperExpression WithEntityType(IEntityType entityType)
	{
		if (entityType == EntityType)
		{
			return this;
		}
		return new EntityShaperExpression(entityType, ValueBufferExpression, IsNullable);
	}

	public virtual EntityShaperExpression MarkAsNullable()
	{
		if (IsNullable)
		{
			return this;
		}
		return new EntityShaperExpression(EntityType, ValueBufferExpression, nullable: true);
	}

	public virtual EntityShaperExpression Update(Expression valueBufferExpression)
	{
		if (valueBufferExpression == ValueBufferExpression)
		{
			return this;
		}
		return new EntityShaperExpression(EntityType, valueBufferExpression, IsNullable);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("EntityShaperExpression: ");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.AppendLine(EntityType);
			expressionPrinter.AppendLine("ValueBufferExpression: ");
			using (expressionPrinter.Indent())
			{
				expressionPrinter.Visit(ValueBufferExpression);
				expressionPrinter.AppendLine();
			}
			expressionPrinter.Append("IsNullable: ");
			expressionPrinter.AppendLine(IsNullable);
		}
	}
}
