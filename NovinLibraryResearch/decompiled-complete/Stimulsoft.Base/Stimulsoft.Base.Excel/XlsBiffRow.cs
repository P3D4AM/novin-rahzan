namespace Stimulsoft.Base.Excel;

internal class XlsBiffRow : XlsBiffRecord
{
	public ushort RowIndex => ReadUInt16(0);

	public ushort FirstDefinedColumn => ReadUInt16(2);

	public ushort LastDefinedColumn => ReadUInt16(4);

	public uint RowHeight => ReadUInt16(6);

	public ushort Flags => ReadUInt16(12);

	public ushort XFormat => ReadUInt16(14);

	internal XlsBiffRow(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffRow(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
