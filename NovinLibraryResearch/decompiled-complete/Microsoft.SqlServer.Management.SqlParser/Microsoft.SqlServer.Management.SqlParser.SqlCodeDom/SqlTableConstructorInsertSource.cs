using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableConstructorInsertSource : SqlInsertSource
{
	private readonly SqlTableConstructorExpression tableConstructorExpression;

	internal override bool IsVisitable => true;

	public SqlTableConstructorExpression TableConstructorExpression => tableConstructorExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return tableConstructorExpression;
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

	internal SqlTableConstructorInsertSource(SqlTableConstructorExpression tableConstructorExpression)
	{
		this.tableConstructorExpression = tableConstructorExpression;
	}
}
