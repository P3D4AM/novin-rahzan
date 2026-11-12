namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffMulBlankCell : XlsBiffBlankCell
{
	public ushort LastColumnIndex => ReadUInt16(base.RecordSize - 2);

	internal XlsBiffMulBlankCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
	}

	public ushort GetXF(ushort columnIdx)
	{
		int num = 4 + 6 * (columnIdx - base.ColumnIndex);
		if (num > base.RecordSize - 2)
		{
			return 0;
		}
		return ReadUInt16(num);
	}
}
