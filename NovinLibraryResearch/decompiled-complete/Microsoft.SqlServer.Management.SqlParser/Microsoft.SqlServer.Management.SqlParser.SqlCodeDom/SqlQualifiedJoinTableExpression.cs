using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlQualifiedJoinTableExpression : SqlJoinTableExpression
{
	private readonly SqlConditionClause onClause;

	internal override bool IsVisitable => true;

	public SqlConditionClause OnClause => onClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Left;
			yield return base.Right;
			yield return onClause;
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

	internal SqlQualifiedJoinTableExpression(SqlTableExpression left, SqlJoinOperatorType joinOperator, SqlTableExpression right, SqlConditionClause onClause)
		: base(left, joinOperator, right)
	{
		this.onClause = onClause;
	}
}
