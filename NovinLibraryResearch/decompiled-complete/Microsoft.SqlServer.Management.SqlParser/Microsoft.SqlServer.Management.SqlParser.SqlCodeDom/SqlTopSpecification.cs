using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTopSpecification : SqlCodeObject
{
	private SqlScalarExpression value;

	private bool isPercent;

	private bool isWithTies;

	internal override bool IsVisitable => true;

	public SqlScalarExpression Value => value;

	public bool IsPercent => isPercent;

	public bool IsWithTies => isWithTies;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return value;
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

	internal SqlTopSpecification(SqlScalarExpression value, bool isPercent)
		: this(value, isPercent, isWithTies: false)
	{
	}

	internal SqlTopSpecification(SqlScalarExpression value, bool isPercent, bool isWithTies)
	{
		this.value = value;
		this.isPercent = isPercent;
		this.isWithTies = isWithTies;
	}
}
