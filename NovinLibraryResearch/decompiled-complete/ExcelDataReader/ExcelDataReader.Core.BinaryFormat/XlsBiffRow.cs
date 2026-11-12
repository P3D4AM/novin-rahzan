namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffRow : XlsBiffRecord
{
	internal enum RowHeightFlags : ushort
	{
		OutlineLevelMask = 3,
		Collapsed = 16,
		DyZero = 32,
		Unsynced = 64,
		GhostDirty = 128
	}

	public ushort RowIndex { get; }

	public ushort FirstDefinedColumn { get; }

	public ushort LastDefinedColumn { get; }

	public bool UseDefaultRowHeight { get; }

	public int RowHeight { get; }

	public bool UseXFormat { get; }

	public ushort XFormat { get; }

	internal XlsBiffRow(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
		if (base.Id == BIFFRECORDTYPE.ROW_V2)
		{
			RowIndex = ReadUInt16(0);
			FirstDefinedColumn = ReadUInt16(2);
			LastDefinedColumn = ReadUInt16(4);
			ushort num = ReadUInt16(6);
			UseDefaultRowHeight = (num & 0x8000) != 0;
			RowHeight = num & 0x7FFFF;
			UseXFormat = ReadByte(10) != 0;
			if (UseXFormat)
			{
				XFormat = ReadUInt16(16);
			}
		}
		else
		{
			RowIndex = ReadUInt16(0);
			FirstDefinedColumn = ReadUInt16(2);
			LastDefinedColumn = ReadUInt16(4);
			ushort num2 = ReadUInt16(6);
			UseDefaultRowHeight = (num2 & 0x8000) != 0;
			RowHeight = num2 & 0x7FFFF;
			RowHeightFlags rowHeightFlags = (RowHeightFlags)ReadUInt16(12);
			RowHeight = (((rowHeightFlags & RowHeightFlags.DyZero) == 0) ? RowHeight : 0);
			UseXFormat = (rowHeightFlags & RowHeightFlags.GhostDirty) != 0;
			XFormat = (ushort)(ReadUInt16(14) & 0xFFFu);
		}
	}
}
