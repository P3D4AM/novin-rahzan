using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlGroupBySets : SqlGroupByItem
{
	private SqlGroupingSetsCollection sets;

	internal override bool IsVisitable => true;

	public SqlGroupingSetsCollection Sets => sets;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlGroupingSet set in sets)
			{
				yield return set;
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

	internal SqlGroupBySets(List<SqlGroupingSet> sets)
	{
		this.sets = new SqlGroupingSetsCollection(sets);
	}
}
