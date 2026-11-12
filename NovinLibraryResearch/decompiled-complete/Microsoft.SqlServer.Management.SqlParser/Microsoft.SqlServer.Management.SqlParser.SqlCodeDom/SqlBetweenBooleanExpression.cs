using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBetweenBooleanExpression : SqlBooleanExpression
{
	private bool hasNot;

	private SqlScalarExpression testExpression;

	private SqlScalarExpression beginExpression;

	private SqlScalarExpression endExpression;

	internal override bool IsVisitable => true;

	public bool HasNot => hasNot;

	public SqlScalarExpression TestExpression => testExpression;

	public SqlScalarExpression BeginExpression => beginExpression;

	public SqlScalarExpression EndExpression => endExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return testExpression;
			yield return beginExpression;
			yield return endExpression;
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

	internal SqlBetweenBooleanExpression(SqlScalarExpression test, SqlScalarExpression begin, SqlScalarExpression end)
		: this(test, hasNot: false, begin, end)
	{
	}

	internal SqlBetweenBooleanExpression(SqlScalarExpression test, bool hasNot, SqlScalarExpression begin, SqlScalarExpression end)
	{
		testExpression = test;
		beginExpression = begin;
		endExpression = end;
		this.hasNot = hasNot;
	}
}
