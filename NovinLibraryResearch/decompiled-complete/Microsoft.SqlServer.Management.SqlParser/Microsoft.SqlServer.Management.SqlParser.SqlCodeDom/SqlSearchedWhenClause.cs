using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSearchedWhenClause : SqlScalarExpression
{
	private SqlBooleanExpression whenExpression;

	private SqlScalarExpression thenExpression;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => thenExpression.BoundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Case;

	internal override bool IsConstantExpression => false;

	public SqlBooleanExpression WhenExpression => whenExpression;

	public SqlScalarExpression ThenExpression => thenExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return whenExpression;
			yield return thenExpression;
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

	internal SqlSearchedWhenClause(SqlBooleanExpression whenExpression, SqlScalarExpression thenExpression)
	{
		this.whenExpression = whenExpression;
		this.thenExpression = thenExpression;
	}
}
