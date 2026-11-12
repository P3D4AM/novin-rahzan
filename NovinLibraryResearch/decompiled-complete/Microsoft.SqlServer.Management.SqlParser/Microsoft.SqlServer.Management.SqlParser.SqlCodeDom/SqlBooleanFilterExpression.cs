using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBooleanFilterExpression : SqlFilterExpression
{
	private readonly SqlBooleanExpression expression;

	internal override bool IsVisitable => true;

	public SqlBooleanExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return expression;
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

	internal SqlBooleanFilterExpression(SqlBooleanExpression expression, bool isValid)
		: base(isValid)
	{
		this.expression = expression;
	}

	internal static SqlBooleanFilterExpression Create(SqlComparisonBooleanExpression expression)
	{
		bool flag = expression.Left.IsColumnReference && expression.Right.IsValidFilterExpressionConstant;
		return new SqlBooleanFilterExpression(expression, flag);
	}

	internal static SqlBooleanFilterExpression Create(SqlInBooleanExpression expression)
	{
		bool flag = expression.InExpression.IsColumnReference && expression.ComparisonValue is SqlInBooleanExpressionCollectionValue;
		if (flag)
		{
			foreach (SqlScalarExpression value in (expression.ComparisonValue as SqlInBooleanExpressionCollectionValue).Values)
			{
				if (!value.IsValidFilterExpressionConstant)
				{
					flag = false;
				}
			}
		}
		return new SqlBooleanFilterExpression(expression, flag);
	}

	internal static SqlBooleanFilterExpression Create(SqlIsNullBooleanExpression expression)
	{
		return new SqlBooleanFilterExpression(expression, expression.Expression.IsColumnReference);
	}
}
