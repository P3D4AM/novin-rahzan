using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTryCatchStatement : SqlStatement
{
	private SqlStatementCollection tryBlock;

	private SqlStatementCollection catchBlock;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "TRY CATCH";

	public SqlStatementCollection TryBlock => tryBlock;

	public SqlStatementCollection CatchBlock => catchBlock;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlStatement item in tryBlock)
			{
				yield return item;
			}
			foreach (SqlStatement item2 in catchBlock)
			{
				yield return item2;
			}
		}
	}

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

	internal SqlTryCatchStatement(List<SqlStatement> tryStatements, List<SqlStatement> catchStatements)
	{
		tryBlock = new SqlStatementCollection(tryStatements);
		catchBlock = ((catchStatements != null) ? new SqlStatementCollection(catchStatements) : new SqlStatementCollection());
	}
}
