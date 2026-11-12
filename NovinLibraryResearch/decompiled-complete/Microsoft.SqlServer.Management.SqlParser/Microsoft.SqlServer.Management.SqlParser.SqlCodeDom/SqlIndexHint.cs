using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIndexHint : SqlHint
{
	private SqlLiteralExpressionCollection arguments;

	internal override bool IsVisitable => true;

	public SqlLiteralExpressionCollection Arguments => arguments;

	public override bool IsIndexHint => true;

	internal override SqlIndexHint AsSqlIndexHint => this;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlLiteralExpression argument in arguments)
			{
				yield return argument;
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

	internal SqlIndexHint(List<SqlLiteralExpression> arguments)
	{
		this.arguments = new SqlLiteralExpressionCollection(arguments);
		foreach (SqlLiteralExpression argument in this.arguments)
		{
			_ = argument;
		}
	}

	internal SqlIndexHint(SqlLiteralExpression argument)
	{
		arguments = new SqlLiteralExpressionCollection(new List<SqlLiteralExpression> { argument });
	}
}
