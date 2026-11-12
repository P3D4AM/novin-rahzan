using System;

namespace ExcelDataReader.Core.CompoundFormat;

internal class CompoundDirectoryEntry
{
	public string EntryName { get; set; }

	public STGTY EntryType { get; set; }

	public DECOLOR EntryColor { get; set; }

	public uint LeftSiblingSid { get; set; }

	public uint RightSiblingSid { get; set; }

	public uint ChildSid { get; set; }

	public Guid ClassId { get; set; }

	public uint UserFlags { get; set; }

	public DateTime CreationTime { get; set; }

	public DateTime LastWriteTime { get; set; }

	public uint StreamFirstSector { get; set; }

	public uint StreamSize { get; set; }

	public bool IsEntryMiniStream { get; set; }

	public uint PropType { get; set; }
}
