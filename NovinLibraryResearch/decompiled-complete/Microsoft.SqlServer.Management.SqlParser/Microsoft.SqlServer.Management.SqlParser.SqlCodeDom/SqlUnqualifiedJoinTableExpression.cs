using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUnqualifiedJoinTableExpression : SqlJoinTableExpression
{
	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Left;
			yield return base.Right;
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

	internal SqlUnqualifiedJoinTableExpression(SqlTableExpression left, SqlJoinOperatorType joinOperator, SqlTableExpression right)
		: base(left, joinOperator, right)
	{
	}
}
