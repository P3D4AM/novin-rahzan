using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlLikeBooleanExpression : SqlBooleanExpression
{
	private bool hasNot;

	private SqlScalarExpression expression;

	private SqlScalarExpression likePattern;

	private SqlScalarExpression escapeClause;

	internal override bool IsVisitable => true;

	public bool HasNot => hasNot;

	public SqlScalarExpression Expression => expression;

	public SqlScalarExpression LikePattern => likePattern;

	public SqlScalarExpression EscapeClause => escapeClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return expression;
			yield return likePattern;
			if (escapeClause != null)
			{
				yield return escapeClause;
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

	internal SqlLikeBooleanExpression(SqlScalarExpression toCompare, bool not, SqlScalarExpression withPattern, SqlScalarExpression escape)
	{
		expression = toCompare;
		likePattern = withPattern;
		hasNot = not;
		if (escape != null)
		{
			escapeClause = escape;
		}
	}
}
