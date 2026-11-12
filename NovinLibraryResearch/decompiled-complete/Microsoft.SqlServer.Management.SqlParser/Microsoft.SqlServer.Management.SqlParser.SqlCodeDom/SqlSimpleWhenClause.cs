using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSimpleWhenClause : SqlCodeObject
{
	private SqlScalarExpression whenExpression;

	private SqlScalarExpression thenExpression;

	internal override bool IsVisitable => true;

	public SqlScalarExpression WhenExpression => whenExpression;

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

	internal SqlSimpleWhenClause(SqlScalarExpression whenExpression, SqlScalarExpression thenExpression)
	{
		this.whenExpression = whenExpression;
		this.thenExpression = thenExpression;
	}
}
