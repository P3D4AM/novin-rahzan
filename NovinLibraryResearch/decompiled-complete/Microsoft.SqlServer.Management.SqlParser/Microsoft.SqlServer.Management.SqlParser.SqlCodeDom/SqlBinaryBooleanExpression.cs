using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBinaryBooleanExpression : SqlBooleanExpression
{
	private SqlBooleanOperatorType operatorType;

	private SqlBooleanExpression left;

	private SqlBooleanExpression right;

	internal override bool IsVisitable => true;

	public SqlBooleanOperatorType Operator => operatorType;

	public SqlBooleanExpression Left => left;

	public SqlBooleanExpression Right => right;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return left;
			yield return right;
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

	internal SqlBinaryBooleanExpression(SqlBooleanExpression leftOp, SqlBooleanOperatorType boolOp, SqlBooleanExpression rightOp)
	{
		left = leftOp;
		right = rightOp;
		operatorType = boolOp;
	}
}
