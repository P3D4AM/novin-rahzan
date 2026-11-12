namespace Stimulsoft.Base.Excel;

internal class XlsBiffMulRKCell : XlsBiffBlankCell
{
	public ushort LastColumnIndex => ReadUInt16(base.RecordSize - 2);

	internal XlsBiffMulRKCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffMulRKCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	public ushort GetXF(ushort ColumnIdx)
	{
		int num = 4 + 6 * (ColumnIdx - base.ColumnIndex);
		if (num > base.RecordSize - 2)
		{
			return 0;
		}
		return ReadUInt16(num);
	}

	public double GetValue(ushort ColumnIdx)
	{
		int num = 6 + 6 * (ColumnIdx - base.ColumnIndex);
		if (num > base.RecordSize)
		{
			return 0.0;
		}
		return XlsBiffRKCell.NumFromRK(ReadUInt32(num));
	}
}
