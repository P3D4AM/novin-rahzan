namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffDefaultRowHeight : XlsBiffRecord
{
	internal enum DefaultRowHeightFlags : ushort
	{
		Unsynced = 1,
		DyZero = 2,
		ExAsc = 4,
		ExDsc = 8
	}

	public int RowHeight { get; }

	public XlsBiffDefaultRowHeight(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		if (biffVersion == 2)
		{
			RowHeight = ReadUInt16(0) & 0x7FFF;
			return;
		}
		DefaultRowHeightFlags defaultRowHeightFlags = (DefaultRowHeightFlags)ReadUInt16(0);
		RowHeight = (((defaultRowHeightFlags & DefaultRowHeightFlags.DyZero) == 0) ? ReadUInt16(2) : 0);
	}
}
