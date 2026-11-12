using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBooleanExpressionError : SqlBooleanExpression, ISqlErrorObject
{
	internal List<SqlCodeObject> children;

	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children => children.AsReadOnly();

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

	internal SqlBooleanExpressionError(SqlErrorCodeObject errorCodeObject)
	{
		children = new List<SqlCodeObject>();
		children.AddRange(errorCodeObject.Children);
	}

	internal SqlBooleanExpressionError(SqlScalarExpression scalarExpression)
	{
		children = new List<SqlCodeObject>();
		children.Add(scalarExpression);
		scalarExpression.ReportParseError(SR.BooleanConditionExpected);
	}
}
