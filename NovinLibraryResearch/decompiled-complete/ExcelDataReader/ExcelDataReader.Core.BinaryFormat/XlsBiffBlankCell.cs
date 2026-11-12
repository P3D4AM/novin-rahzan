namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffBlankCell : XlsBiffRecord
{
	public ushort RowIndex { get; }

	public ushort ColumnIndex { get; }

	public ushort XFormat { get; }

	public ushort Format { get; }

	public override bool IsCell => true;

	public bool IsBiff2Cell
	{
		get
		{
			BIFFRECORDTYPE id = base.Id;
			if (id - 1 <= BIFFRECORDTYPE.LABEL_OLD)
			{
				return true;
			}
			return false;
		}
	}

	internal XlsBiffBlankCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		RowIndex = ReadUInt16(0);
		ColumnIndex = ReadUInt16(2);
		if (IsBiff2Cell)
		{
			byte b = ReadByte(4);
			byte b2 = ReadByte(5);
			XFormat = (ushort)(b & 0x3Fu);
			Format = (byte)(b2 & 0x3Fu);
		}
		else
		{
			XFormat = ReadUInt16(4);
		}
	}
}
