using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlComparisonBooleanExpression : SqlBooleanExpression
{
	private SqlScalarExpression left;

	private SqlScalarExpression right;

	private SqlComparisonBooleanExpressionType comparisonOperator;

	internal override bool IsVisitable => true;

	public SqlScalarExpression Left => left;

	public SqlScalarExpression Right => right;

	public SqlComparisonBooleanExpressionType ComparisonOperator => comparisonOperator;

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

	internal SqlComparisonBooleanExpression(SqlScalarExpression leftExpression, SqlComparisonBooleanExpressionType comparisonOp, SqlScalarExpression rightExpression)
	{
		left = leftExpression;
		comparisonOperator = comparisonOp;
		right = rightExpression;
	}
}
