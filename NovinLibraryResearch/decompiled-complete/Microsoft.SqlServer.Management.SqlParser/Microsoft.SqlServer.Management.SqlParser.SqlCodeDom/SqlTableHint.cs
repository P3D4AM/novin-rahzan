using System;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableHint : SqlHint
{
	private SqlTableHintType type;

	internal override bool IsVisitable => true;

	public SqlTableHintType Type => type;

	public override bool IsTableHint => true;

	internal override SqlTableHint AsSqlTableHint => this;

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

	internal SqlTableHint(SqlIdentifier name)
		: this(name.Value)
	{
	}

	internal SqlTableHint(string name)
	{
		type = GetHintFromString(name);
		if (type == SqlTableHintType.None)
		{
			ReportParseError(SR.InvalidTableHint(name));
		}
	}

	internal static SqlTableHintType GetHintFromString(string hintName)
	{
		return StringTokenLookup.Find(hintName) switch
		{
			StringToken.FastFirstRow => SqlTableHintType.FastFirstRow, 
			StringToken.ForceSeek => SqlTableHintType.ForceSeek, 
			StringToken.HoldLock => SqlTableHintType.HoldLock, 
			StringToken.KeepDefaults => SqlTableHintType.KeepDefaults, 
			StringToken.KeepIdentity => SqlTableHintType.KeepIdentity, 
			StringToken.NoExpand => SqlTableHintType.NoExpand, 
			StringToken.NoLock => SqlTableHintType.NoLock, 
			StringToken.NoWait => SqlTableHintType.NoWait, 
			StringToken.PagLock => SqlTableHintType.PageLock, 
			StringToken.ReadCommitted => SqlTableHintType.ReadCommitted, 
			StringToken.ReadCommittedLock => SqlTableHintType.ReadCommittedLock, 
			StringToken.ReadPast => SqlTableHintType.ReadPast, 
			StringToken.ReadUncommitted => SqlTableHintType.ReadUncommitted, 
			StringToken.RepeatableRead => SqlTableHintType.RepeatableRead, 
			StringToken.RowLock => SqlTableHintType.Rowlock, 
			StringToken.Serializable => SqlTableHintType.Serializable, 
			StringToken.SpatialWindowMaxCells => SqlTableHintType.SpatialWindowMaxCells, 
			StringToken.TabLock => SqlTableHintType.TabLock, 
			StringToken.TabLockX => SqlTableHintType.TabLockX, 
			StringToken.UpdLock => SqlTableHintType.UpdateLock, 
			StringToken.Snapshot => SqlTableHintType.Snapshot, 
			StringToken.XLock => SqlTableHintType.XLock, 
			_ => SqlTableHintType.None, 
		};
	}
}
