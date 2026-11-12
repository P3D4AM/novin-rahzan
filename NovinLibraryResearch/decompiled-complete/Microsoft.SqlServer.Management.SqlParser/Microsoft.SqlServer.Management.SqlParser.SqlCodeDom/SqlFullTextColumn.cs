using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFullTextColumn : SqlCodeObject
{
	private SqlScalarRefExpression columnRefExpression;

	private SqlSelectStarExpression selectStarExpression;

	internal override bool IsVisitable => true;

	public SqlScalarRefExpression ColumnRefExpression => columnRefExpression;

	public SqlSelectStarExpression SelectStarExpression => selectStarExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (columnRefExpression != null)
			{
				yield return columnRefExpression;
			}
			else
			{
				yield return selectStarExpression;
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

	internal SqlFullTextColumn(SqlScalarRefExpression columnRefExpression)
	{
		this.columnRefExpression = columnRefExpression;
	}

	internal SqlFullTextColumn(SqlSelectStarExpression selectStarExpression)
	{
		this.selectStarExpression = selectStarExpression;
	}
}
