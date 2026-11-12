using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMergeStatement : SqlDmlStatement
{
	private readonly SqlMergeSpecification mergeSpecification;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "MERGE";

	public SqlMergeSpecification MergeSpecification => mergeSpecification;

	internal override SqlDmlSpecification DmlSpecification => mergeSpecification;

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
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

	internal SqlMergeStatement(SqlMergeSpecification mergeSpecification)
		: this(null, mergeSpecification)
	{
	}

	internal SqlMergeStatement(SqlQueryWithClause queryWithClause, SqlMergeSpecification mergeSpecification)
		: base(queryWithClause)
	{
		this.mergeSpecification = mergeSpecification;
	}
}
