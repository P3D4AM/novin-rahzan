using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInBooleanExpression : SqlBooleanExpression
{
	private SqlInBooleanExpressionValue comparisonValue;

	private SqlScalarExpression inExpression;

	private bool hasNot;

	internal override bool IsVisitable => true;

	public SqlInBooleanExpressionValue ComparisonValue => comparisonValue;

	public SqlScalarExpression InExpression => inExpression;

	public bool HasNot => hasNot;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return inExpression;
			yield return comparisonValue;
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

	internal SqlInBooleanExpression(SqlScalarExpression expression, bool hasNot, SqlInBooleanExpressionValue value)
	{
		inExpression = expression;
		comparisonValue = value;
		this.hasNot = hasNot;
	}

	internal SqlInBooleanExpression(SqlScalarExpression expression, SqlInBooleanExpressionValue value)
		: this(expression, hasNot: false, value)
	{
	}
}
