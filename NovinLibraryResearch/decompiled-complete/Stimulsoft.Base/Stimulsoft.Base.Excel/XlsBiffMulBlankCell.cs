namespace Stimulsoft.Base.Excel;

internal class XlsBiffMulBlankCell : XlsBiffBlankCell
{
	public ushort LastColumnIndex => ReadUInt16(base.RecordSize - 2);

	internal XlsBiffMulBlankCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffMulBlankCell(byte[] bytes, uint offset)
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
}
