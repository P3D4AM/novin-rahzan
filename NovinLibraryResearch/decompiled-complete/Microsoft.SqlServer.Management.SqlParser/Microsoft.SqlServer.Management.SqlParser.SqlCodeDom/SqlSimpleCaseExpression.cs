using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSimpleCaseExpression : SqlCaseExpression
{
	private SqlScalarExpression testExpression;

	private SqlSimpleWhenClauseCollection whenClauses;

	internal override bool IsVisitable => true;

	public SqlScalarExpression TestExpression => testExpression;

	public SqlSimpleWhenClauseCollection WhenClauses => whenClauses;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return testExpression;
			foreach (SqlSimpleWhenClause whenClause in whenClauses)
			{
				yield return whenClause;
			}
			if (elseExpression != null)
			{
				yield return elseExpression;
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

	internal override IEnumerable<SqlScalarExpression> RetrieveResultScalarExpressions()
	{
		foreach (SqlSimpleWhenClause whenClause in whenClauses)
		{
			yield return whenClause.ThenExpression;
		}
		if (elseExpression != null)
		{
			yield return elseExpression;
		}
	}

	internal SqlSimpleCaseExpression(SqlScalarExpression testExpression, List<SqlSimpleWhenClause> whenClauses, SqlScalarExpression elseExpression)
		: base(elseExpression)
	{
		this.testExpression = testExpression;
		this.whenClauses = new SqlSimpleWhenClauseCollection(whenClauses);
	}
}
