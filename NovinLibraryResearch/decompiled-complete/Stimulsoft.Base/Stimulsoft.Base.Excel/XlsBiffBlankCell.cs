namespace Stimulsoft.Base.Excel;

internal class XlsBiffBlankCell : XlsBiffRecord
{
	public ushort RowIndex => ReadUInt16(0);

	public ushort ColumnIndex => ReadUInt16(2);

	public ushort XFormat => ReadUInt16(4);

	internal XlsBiffBlankCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffBlankCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
