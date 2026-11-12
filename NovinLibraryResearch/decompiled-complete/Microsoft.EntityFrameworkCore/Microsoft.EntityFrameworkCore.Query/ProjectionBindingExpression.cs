using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public class ProjectionBindingExpression : Expression, IPrintableExpression
{
	public virtual Expression QueryExpression { get; }

	public virtual ProjectionMember ProjectionMember { get; }

	public virtual int? Index { get; }

	public virtual IDictionary<IProperty, int> IndexMap { get; }

	public override Type Type { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public ProjectionBindingExpression(Expression queryExpression, ProjectionMember projectionMember, Type type)
	{
		QueryExpression = queryExpression;
		ProjectionMember = projectionMember;
		Type = type;
	}

	public ProjectionBindingExpression(Expression queryExpression, int index, Type type)
	{
		QueryExpression = queryExpression;
		Index = index;
		Type = type;
	}

	public ProjectionBindingExpression(Expression queryExpression, IDictionary<IProperty, int> indexMap)
	{
		QueryExpression = queryExpression;
		IndexMap = indexMap;
		Type = typeof(ValueBuffer);
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("ProjectionBindingExpression: ");
		if (ProjectionMember != null)
		{
			expressionPrinter.Append(ProjectionMember);
			return;
		}
		if (Index.HasValue)
		{
			expressionPrinter.Append(Index);
			return;
		}
		using (expressionPrinter.Indent())
		{
			foreach (KeyValuePair<IProperty, int> item in IndexMap)
			{
				expressionPrinter.AppendLine($"{item.Key.Name}:{item.Value},");
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is ProjectionBindingExpression projectionBindingExpression)
				{
					return Equals(projectionBindingExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(ProjectionBindingExpression projectionBindingExpression)
	{
		if (QueryExpression.Equals(projectionBindingExpression.QueryExpression) && Type == projectionBindingExpression.Type && (ProjectionMember?.Equals(projectionBindingExpression.ProjectionMember) ?? (projectionBindingExpression.ProjectionMember == null)) && Index == projectionBindingExpression.Index)
		{
			return IndexMap == projectionBindingExpression.IndexMap;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(QueryExpression, ProjectionMember, Index, IndexMap);
	}
}
