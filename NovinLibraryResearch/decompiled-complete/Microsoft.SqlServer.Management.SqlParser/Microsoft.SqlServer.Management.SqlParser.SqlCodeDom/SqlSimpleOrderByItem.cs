using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSimpleOrderByItem : SqlCodeObject
{
	private SqlIdentifier columnName;

	private SqlSortOrder sortOrder;

	internal override bool IsVisitable => true;

	public SqlSortOrder SortOrder => sortOrder;

	public SqlIdentifier ColumnName => columnName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return columnName;
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

	internal SqlSimpleOrderByItem(SqlIdentifier columnName, SqlSortOrder order)
	{
		this.columnName = columnName;
		sortOrder = order;
	}
}
