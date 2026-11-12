using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDBCCStatement : SqlStatement
{
	private SqlDbccCommandType commandType;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DBCC";

	public SqlDbccCommandType CommandType => commandType;

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

	internal SqlDBCCStatement(string command)
	{
		if (string.Compare(command, "NewAlloc", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.NewAlloc;
		}
		else if (string.Compare(command, "DbRepair", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.DbRepair;
		}
		else if (string.Compare(command, "Rowlock", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.RowLock;
		}
		else if (string.Compare(command, "TextAll", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.TextAll;
		}
		else if (string.Compare(command, "TextAlloc", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.TextAlloc;
		}
		else if (string.Compare(command, "TraceOn", StringComparison.OrdinalIgnoreCase) == 0)
		{
			commandType = SqlDbccCommandType.TraceOn;
		}
		else
		{
			commandType = SqlDbccCommandType.None;
		}
	}
}
