using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCompressionPartitionRange : SqlCodeObject
{
	private readonly SqlScalarExpression from;

	private readonly SqlScalarExpression to;

	internal override bool IsVisitable => true;

	public SqlScalarExpression From => from;

	public SqlScalarExpression To => to;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return from;
			if (to != null)
			{
				yield return to;
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

	internal SqlCompressionPartitionRange(SqlScalarExpression from, SqlScalarExpression to)
	{
		this.from = from;
		this.to = to;
	}

	internal SqlCompressionPartitionRange(SqlScalarExpression from)
	{
		this.from = from;
	}
}
