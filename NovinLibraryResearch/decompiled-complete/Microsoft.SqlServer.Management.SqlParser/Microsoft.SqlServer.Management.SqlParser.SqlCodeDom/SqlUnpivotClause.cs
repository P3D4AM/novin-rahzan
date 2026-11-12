using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUnpivotClause : SqlCodeObject
{
	private readonly SqlColumnRefExpression valueColumn;

	private readonly SqlColumnRefExpression pivotedColumn;

	private readonly SqlColumnRefExpressionCollection columnList;

	internal override bool IsVisitable => true;

	public SqlColumnRefExpression ValueColumn => valueColumn;

	public SqlColumnRefExpression PivotedColumn => pivotedColumn;

	public SqlColumnRefExpressionCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return valueColumn;
			yield return pivotedColumn;
			foreach (SqlColumnRefExpression column in columnList)
			{
				yield return column;
			}
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		foreach (SqlColumnRefExpression column in columnList)
		{
			column.Bind(bindingContext);
			base.BindingState |= column.BindingState;
		}
	}

	internal SqlUnpivotClause(SqlColumnRefExpression valueColumn, SqlColumnRefExpression pivotedColumn, List<SqlColumnRefExpression> columnList)
	{
		this.valueColumn = valueColumn;
		this.pivotedColumn = pivotedColumn;
		this.columnList = new SqlColumnRefExpressionCollection(columnList);
	}
}
