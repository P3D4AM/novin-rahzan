using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCollateScalarExpression : SqlScalarExpression
{
	private SqlCollation collation;

	private SqlScalarExpression expression;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => expression.BoundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Collate | expression.ExpressionType;

	internal override bool IsConstantExpression => expression.IsConstantExpression;

	public SqlCollation Collation => collation;

	public SqlScalarExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return Expression;
			yield return Collation;
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

	internal SqlCollateScalarExpression(SqlScalarExpression expression, SqlCollation collation)
	{
		this.collation = collation;
		this.expression = expression;
	}
}
