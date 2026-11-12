using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlOffsetFetchClause : SqlCodeObject
{
	internal override bool IsVisitable => true;

	public SqlScalarExpression Offset { get; private set; }

	public SqlScalarExpression Fetch { get; private set; }

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return Offset;
			if (Fetch != null)
			{
				yield return Fetch;
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

	public SqlOffsetFetchClause(SqlScalarExpression offset, SqlScalarExpression fetch)
	{
		Offset = offset;
		Fetch = fetch;
	}
}
