namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffMulRKCell : XlsBiffBlankCell
{
	public ushort LastColumnIndex => ReadUInt16(base.RecordSize - 2);

	internal XlsBiffMulRKCell(byte[] bytes, uint offset, int biffVersion)
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

	public double GetValue(ushort columnIdx)
	{
		int num = 6 + 6 * (columnIdx - base.ColumnIndex);
		if (num > base.RecordSize)
		{
			return 0.0;
		}
		return XlsBiffRKCell.NumFromRK(ReadUInt32(num));
	}
}
