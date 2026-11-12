using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SqlTableHintType
{
	None = 0,
	FastFirstRow = 1,
	ForceSeek = 2,
	HoldLock = 4,
	KeepDefaults = 8,
	KeepIdentity = 0x10,
	NoExpand = 0x20,
	NoLock = 0x40,
	NoWait = 0x80,
	PageLock = 0x100,
	ReadCommitted = 0x200,
	ReadCommittedLock = 0x400,
	ReadPast = 0x800,
	ReadUncommitted = 0x1000,
	RepeatableRead = 0x2000,
	Rowlock = 0x4000,
	Serializable = 0x8000,
	TabLock = 0x10000,
	TabLockX = 0x20000,
	UpdateLock = 0x40000,
	XLock = 0x80000,
	SpatialWindowMaxCells = 0x100000,
	Snapshot = 0x200000
}
