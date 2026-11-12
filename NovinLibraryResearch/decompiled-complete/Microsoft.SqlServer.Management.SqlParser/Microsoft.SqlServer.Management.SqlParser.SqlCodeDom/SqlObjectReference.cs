using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlObjectReference : SqlCodeObject
{
	private readonly SqlObjectIdentifier objectIdentifier;

	private readonly SqlScalarVariableRefExpression variable;

	internal override bool IsVisitable => true;

	public SqlObjectIdentifier ObjectIdentifier => objectIdentifier;

	public SqlScalarVariableRefExpression Variable => variable;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (objectIdentifier != null)
			{
				yield return objectIdentifier;
			}
			else
			{
				yield return variable;
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

	internal SqlObjectReference(SqlObjectIdentifier objectIdentifier)
	{
		this.objectIdentifier = objectIdentifier;
	}

	internal SqlObjectReference(SqlScalarVariableRefExpression variable)
	{
		this.variable = variable;
	}
}
