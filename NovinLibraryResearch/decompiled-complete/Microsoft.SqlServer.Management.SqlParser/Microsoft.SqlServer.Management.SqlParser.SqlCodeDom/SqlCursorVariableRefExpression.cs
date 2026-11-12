using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCursorVariableRefExpression : SqlCodeObject
{
	private readonly string variableName;

	internal override bool IsVisitable => true;

	public string VariableName => variableName;

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

	internal SqlCursorVariableRefExpression(string variableName)
	{
		this.variableName = variableName;
	}

	public static SqlCursorVariableRefExpression Create(string variableName)
	{
		if (variableName == null)
		{
			return new SqlCursorVariableRefExpressionError();
		}
		return new SqlCursorVariableRefExpression(variableName);
	}
}
