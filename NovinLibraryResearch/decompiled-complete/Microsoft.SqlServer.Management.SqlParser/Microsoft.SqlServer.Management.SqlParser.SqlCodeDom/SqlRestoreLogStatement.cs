using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlRestoreLogStatement : SqlBackupRestoreLogStatement
{
	private class SqlLoadLogStatement : SqlRestoreLogStatement, ISqlDumpLoadStatement, ISqlDeprecatedStatement
	{
	}

	private class SqlRestoreTransactionStatement : SqlRestoreLogStatement, ISqlBackupRestoreTransactionStatement, ISqlDeprecatedStatement
	{
	}

	private class SqlLoadTransactionStatement : SqlRestoreLogStatement, ISqlDumpLoadStatement, ISqlDeprecatedStatement, ISqlBackupRestoreTransactionStatement
	{
	}

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "RESTORE LOG";

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

	internal static SqlRestoreLogStatement Create(bool isLoadStatement, bool isLoadTransaction)
	{
		if (isLoadStatement)
		{
			if (isLoadTransaction)
			{
				return new SqlLoadTransactionStatement();
			}
			return new SqlLoadLogStatement();
		}
		if (isLoadTransaction)
		{
			return new SqlRestoreTransactionStatement();
		}
		return new SqlRestoreLogStatement();
	}
}
