using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlGroupingSetItemsCollection : SqlGroupingSet
{
	private SqlGroupingSetItemCollection items;

	internal override bool IsVisitable => true;

	public SqlGroupingSetItemCollection Items => items;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlGroupingSetItem item in items)
			{
				yield return item;
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

	internal SqlGroupingSetItemsCollection(List<SqlGroupingSetItem> items)
	{
		this.items = new SqlGroupingSetItemCollection(items);
	}

	internal SqlGroupingSetItemsCollection(SqlGroupingSetItem item)
	{
		items = new SqlGroupingSetItemCollection(item);
	}
}
