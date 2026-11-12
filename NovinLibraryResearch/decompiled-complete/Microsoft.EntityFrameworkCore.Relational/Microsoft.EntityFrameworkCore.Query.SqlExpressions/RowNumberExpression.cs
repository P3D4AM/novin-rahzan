using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class RowNumberExpression : SqlExpression
{
	public virtual IReadOnlyList<SqlExpression> Partitions { get; }

	public virtual IReadOnlyList<OrderingExpression> Orderings { get; }

	public RowNumberExpression(IReadOnlyList<SqlExpression> partitions, IReadOnlyList<OrderingExpression> orderings, RelationalTypeMapping typeMapping)
		: base(typeof(long), typeMapping)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(orderings, "orderings");
		Partitions = partitions;
		Orderings = orderings;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		bool flag = false;
		List<SqlExpression> list = new List<SqlExpression>();
		foreach (SqlExpression partition in Partitions)
		{
			SqlExpression sqlExpression = (SqlExpression)visitor.Visit(partition);
			flag = flag || sqlExpression != partition;
			list.Add(sqlExpression);
		}
		List<OrderingExpression> list2 = new List<OrderingExpression>();
		foreach (OrderingExpression ordering in Orderings)
		{
			OrderingExpression orderingExpression = (OrderingExpression)visitor.Visit(ordering);
			flag = flag || orderingExpression != ordering;
			list2.Add(orderingExpression);
		}
		if (!flag)
		{
			return this;
		}
		return new RowNumberExpression(list, list2, TypeMapping);
	}

	public virtual RowNumberExpression Update(IReadOnlyList<SqlExpression> partitions, IReadOnlyList<OrderingExpression> orderings)
	{
		if (!((Partitions == null) ? (partitions == null) : Partitions.SequenceEqual(partitions)) || !Orderings.SequenceEqual(orderings))
		{
			return new RowNumberExpression(partitions, orderings, TypeMapping);
		}
		return this;
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("ROW_NUMBER() OVER(");
		if (Partitions.Any())
		{
			expressionPrinter.Append("PARTITION BY ");
			expressionPrinter.VisitList(Partitions);
			expressionPrinter.Append(" ");
		}
		expressionPrinter.Append("ORDER BY ");
		expressionPrinter.VisitList(Orderings);
		expressionPrinter.Append(")");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is RowNumberExpression rowNumberExpression)
				{
					return Equals(rowNumberExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(RowNumberExpression rowNumberExpression)
	{
		if (base.Equals((object)rowNumberExpression) && ((Partitions == null) ? (rowNumberExpression.Partitions == null) : Partitions.SequenceEqual(rowNumberExpression.Partitions)))
		{
			return Orderings.SequenceEqual(rowNumberExpression.Orderings);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(base.GetHashCode());
		foreach (SqlExpression partition in Partitions)
		{
			hashCode.Add(partition);
		}
		foreach (OrderingExpression ordering in Orderings)
		{
			hashCode.Add(ordering);
		}
		return hashCode.ToHashCode();
	}
}
