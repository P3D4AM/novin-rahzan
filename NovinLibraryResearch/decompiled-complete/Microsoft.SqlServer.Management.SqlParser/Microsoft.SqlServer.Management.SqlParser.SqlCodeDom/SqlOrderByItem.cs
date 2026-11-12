using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlOrderByItem : SqlCodeObject
{
	private SqlSortOrder sortOrder;

	private SqlScalarExpression expression;

	internal override bool IsVisitable => true;

	public SqlSortOrder SortOrder => sortOrder;

	public SqlScalarExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return expression;
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

	internal SqlOrderByItem(SqlScalarExpression expression, SqlSortOrder order)
	{
		this.expression = expression;
		sortOrder = order;
	}
}
