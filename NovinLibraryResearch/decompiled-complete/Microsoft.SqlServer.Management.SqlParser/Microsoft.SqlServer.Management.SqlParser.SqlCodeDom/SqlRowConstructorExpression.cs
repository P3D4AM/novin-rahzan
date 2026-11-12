using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlRowConstructorExpression : SqlCodeObject
{
	private readonly SqlScalarExpressionCollection values;

	internal override bool IsVisitable => true;

	public SqlScalarExpressionCollection Values => values;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlScalarExpression value in values)
			{
				yield return value;
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

	internal SqlRowConstructorExpression(List<SqlScalarExpression> values)
	{
		this.values = new SqlScalarExpressionCollection(values);
	}
}
