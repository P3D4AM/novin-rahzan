using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public class ColumnExpression : SqlExpression
{
	public string Name { get; }

	public TableExpressionBase Table { get; }

	public bool IsNullable { get; }

	internal ColumnExpression(IProperty property, TableExpressionBase table, bool nullable)
		: this(property.GetColumnName(), table, property.ClrType, property.GetRelationalTypeMapping(), nullable || property.IsColumnNullable())
	{
	}

	internal ColumnExpression(ProjectionExpression subqueryProjection, TableExpressionBase table)
		: this(subqueryProjection.Alias, table, subqueryProjection.Type, subqueryProjection.Expression.TypeMapping, IsNullableProjection(subqueryProjection))
	{
	}

	private static bool IsNullableProjection(ProjectionExpression projectionExpression)
	{
		SqlExpression expression = projectionExpression.Expression;
		if (!(expression is ColumnExpression { IsNullable: var isNullable }))
		{
			if (expression is SqlConstantExpression sqlConstantExpression)
			{
				return sqlConstantExpression.Value == null;
			}
			return true;
		}
		return isNullable;
	}

	private ColumnExpression(string name, TableExpressionBase table, Type type, RelationalTypeMapping typeMapping, bool nullable)
		: base(type, typeMapping)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table.Alias, "table.Alias");
		Name = name;
		Table = table;
		IsNullable = nullable;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public ColumnExpression MakeNullable()
	{
		return new ColumnExpression(Name, Table, Type.MakeNullable(), TypeMapping, nullable: true);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append(Table.Alias).Append(".");
		expressionPrinter.Append(Name);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is ColumnExpression columnExpression)
				{
					return Equals(columnExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(ColumnExpression columnExpression)
	{
		if (base.Equals((object)columnExpression) && string.Equals(Name, columnExpression.Name) && Table.Equals(columnExpression.Table))
		{
			return IsNullable == columnExpression.IsNullable;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Name, Table, IsNullable);
	}

	private string DebuggerDisplay()
	{
		return Table.Alias + "." + Name;
	}
}
