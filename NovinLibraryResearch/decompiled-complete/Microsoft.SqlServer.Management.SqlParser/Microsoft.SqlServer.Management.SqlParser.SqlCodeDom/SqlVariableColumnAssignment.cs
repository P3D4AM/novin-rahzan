using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlVariableColumnAssignment : SqlAssignment
{
	private readonly SqlScalarVariableRefExpression variable;

	private readonly SqlScalarRefExpression column;

	private readonly SqlScalarExpression value;

	internal override bool IsVisitable => true;

	public SqlScalarExpression Value => value;

	public SqlScalarVariableRefExpression Variable => variable;

	public SqlScalarRefExpression Column => column;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return variable;
			yield return column;
			yield return Value;
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

	internal SqlVariableColumnAssignment(SqlScalarVariableRefExpression variable, SqlColumnAssignment colAssignment)
		: base(colAssignment.Operator)
	{
		this.variable = variable;
		column = colAssignment.Column;
		value = colAssignment.Value;
	}
}
